// Frontend/Identity/TokenAuthenticationStateProvider.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using ZaposliMe.Frontend.Identity.Models;

namespace ZaposliMe.Frontend.Identity
{
    /// <summary>
    /// Token-based auth that calls the same endpoints as CookieAuthenticationStateProvider:
    ///   POST /api/account/register
    ///   POST /api/account/login?useCookies=false   (expects token JSON)
    ///   GET  /api/account/info
    ///   POST /api/account/logout
    /// Stores tokens in sessionStorage and sets Authorization header on "Backend" HttpClient.
    /// </summary>
    public sealed class TokenAuthenticationStateProvider
        : AuthenticationStateProvider, IAccountManagement
    {
        // --- endpoints (same base as your cookie provider, token mode on login) ---
        private const string RegisterUrl = "/api/account/register";
        private const string LoginUrl = "/api/account/login?useCookies=false";
        private const string InfoUrl = "/api/account/info";
        private const string LogoutUrl = "/api/account/logout";
        // Optional refresh (wire up if you add it server-side)
        private const string RefreshUrl = "/api/account/refresh";

        // --- sessionStorage keys ---
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string ExpiresAtKey = "expires_at_utc"; // ISO8601 UTC

        private readonly IHttpClientFactory _httpFactory;
        private readonly IJSRuntime _js;
        private readonly ILogger<TokenAuthenticationStateProvider> _logger;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        private readonly JsonSerializerOptions _json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private HttpClient Http => _httpFactory.CreateClient("Backend");

        public TokenAuthenticationStateProvider(
            IHttpClientFactory httpFactory,
            IJSRuntime js,
            ILogger<TokenAuthenticationStateProvider> logger)
        {
            _httpFactory = httpFactory;
            _js = js;
            _logger = logger;
        }

        // ---------------- IAccountManagement ----------------

        public async Task<FormResult> RegisterAsync(
            string firstName, string lastName, string email, string password, string role, long? age, string? phoneNumber)
        {
            string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

            try
            {
                var res = await Http.PostAsJsonAsync(RegisterUrl, new
                {
                    firstName,
                    lastName,
                    email,
                    password,
                    role,
                    age,
                    phoneNumber
                }, _json);

                if (res.IsSuccessStatusCode) return new FormResult { Succeeded = true };

                var details = await ReadBodySafeAsync(res);
                // Try to unpack ProblemDetails:errors[] if present
                try
                {
                    using var doc = JsonDocument.Parse(details);
                    if (doc.RootElement.TryGetProperty("errors", out var errorsEl))
                    {
                        var errors = new List<string>();
                        foreach (var prop in errorsEl.EnumerateObject())
                        {
                            if (prop.Value.ValueKind == JsonValueKind.String)
                                errors.Add(prop.Value.GetString() ?? string.Empty);
                            else if (prop.Value.ValueKind == JsonValueKind.Array)
                                errors.AddRange(prop.Value.EnumerateArray()
                                    .Select(x => x.GetString() ?? string.Empty)
                                    .Where(x => !string.IsNullOrWhiteSpace(x)));
                        }
                        if (errors.Count > 0)
                            return new FormResult { Succeeded = false, ErrorList = [.. errors] };
                    }
                }
                catch { /* fall back to details string */ }

                return new FormResult { Succeeded = false, ErrorList = [string.IsNullOrWhiteSpace(details) ? defaultDetail[0] : details] };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                return new FormResult { Succeeded = false, ErrorList = defaultDetail };
            }
        }

        public async Task<FormResult> LoginAsync(string email, string password)
        {
            var res = await Http.PostAsJsonAsync(LoginUrl, new { email, password }, _json);
            if (!res.IsSuccessStatusCode)
            {
                var err = await ReadBodySafeAsync(res);
                return new FormResult { Succeeded = false, ErrorList = [string.IsNullOrWhiteSpace(err) ? "Invalid email and/or password." : err] };
            }

            // Read raw JSON and parse case-insensitively
            var payload = await res.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(payload);
            var root = doc.RootElement;

            string? access = GetStringCI(root, "access_token", "accessToken", "token");
            string? refresh = GetStringCI(root, "refresh_token", "refreshToken");
            int? expiresIn = GetIntCI(root, "expires_in", "expiresIn");

            if (string.IsNullOrWhiteSpace(access))
                return new FormResult { Succeeded = false, ErrorList = ["Login response missing access_token."] };

            await SetItemAsync(AccessTokenKey, access);
            if (!string.IsNullOrWhiteSpace(refresh))
                await SetItemAsync(RefreshTokenKey, refresh);

            var expUtc = ComputeExpiryUtc(access, expiresIn);
            await SetItemAsync(ExpiresAtKey, expUtc.ToString("o"));

            Http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new FormResult { Succeeded = true };
        }

        public async Task LogoutAsync()
        {
            try
            {
                using var empty = new StringContent("{}", Encoding.UTF8, "application/json");
                await Http.PostAsync(LogoutUrl, empty);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Logout call failed, clearing client tokens anyway.");
            }

            await RemoveItemAsync(AccessTokenKey);
            await RemoveItemAsync(RefreshTokenKey);
            await RemoveItemAsync(ExpiresAtKey);

            Http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.Identity?.IsAuthenticated == true;
        }

        // ---------------- AuthenticationStateProvider ----------------

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetItemAsync(AccessTokenKey);
            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(_anonymous);

            var ensured = await EnsureValidAccessTokenAsync(token);
            if (!ensured.Success)
                return new AuthenticationState(_anonymous);

            Http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ensured.Token);

            try
            {
                var userInfo = await Http.GetFromJsonAsync<UserInfo>(InfoUrl, _json);
                if (userInfo is null || !userInfo.IsAuthenticated)
                    return new AuthenticationState(_anonymous);

                var claims = new List<Claim>
                {
                    // UI-facing name/email (server will still authorize by token)
                    new(ClaimTypes.Email, userInfo.Email),
                    new(ClaimTypes.Name,  userInfo.Email),
                };

                if (userInfo.Roles is { Count: > 0 })
                    foreach (var r in userInfo.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, r));

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
                return new AuthenticationState(principal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching /api/account/info failed");
                return new AuthenticationState(_anonymous);
            }
        }

        // ---------------- Token helpers ----------------

        private async Task<(bool Success, string Token)> EnsureValidAccessTokenAsync(string token)
        {
            // Check stored expiry first
            var expIso = await GetItemAsync(ExpiresAtKey);
            if (DateTime.TryParse(expIso, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var expUtc))
            {
                if (DateTime.UtcNow < expUtc.AddSeconds(-60))
                    return (true, token);
            }
            else
            {
                var jwtExp = TryReadJwtExpUtc(token);
                if (jwtExp.HasValue && DateTime.UtcNow < jwtExp.Value.AddSeconds(-60))
                    return (true, token);
            }

            // Try refresh if we have a refresh token and endpoint
            var refresh = await GetItemAsync(RefreshTokenKey);
            if (string.IsNullOrWhiteSpace(refresh))
                return (false, token);

            try
            {
                var res = await Http.PostAsJsonAsync(RefreshUrl, new { refreshToken = refresh }, _json);
                if (!res.IsSuccessStatusCode) return (false, token);

                var doc = await res.Content.ReadFromJsonAsync<JsonElement>(_json);

                string? access = GetStringCI(doc, "access_token", "accessToken", "token");
                string? newRef = GetStringCI(doc, "refresh_token", "refreshToken");
                int? expiresIn = GetIntCI(doc, "expires_in", "expiresIn");

                if (string.IsNullOrWhiteSpace(access)) return (false, token);

                await SetItemAsync(AccessTokenKey, access);
                if (!string.IsNullOrWhiteSpace(newRef))
                    await SetItemAsync(RefreshTokenKey, newRef);

                var newExp = ComputeExpiryUtc(access, expiresIn);
                await SetItemAsync(ExpiresAtKey, newExp.ToString("o"));

                return (true, access);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token refresh failed");
                return (false, token);
            }
        }

        private static DateTime ComputeExpiryUtc(string jwt, int? expiresIn)
        {
            var fromJwt = TryReadJwtExpUtc(jwt);
            if (fromJwt.HasValue) return fromJwt.Value;
            return expiresIn.HasValue ? DateTime.UtcNow.AddSeconds(expiresIn.Value)
                                      : DateTime.UtcNow.AddMinutes(30);
        }

        private static DateTime? TryReadJwtExpUtc(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2) return null;

                string payload = parts[1]
                    .PadRight(parts[1].Length + (4 - parts[1].Length % 4) % 4, '=')
                    .Replace('-', '+').Replace('_', '/');

                var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("exp", out var exp) && exp.TryGetInt64(out var v))
                    return DateTimeOffset.FromUnixTimeSeconds(v).UtcDateTime;

                return null;
            }
            catch { return null; }
        }

        private static string? GetStringCI(JsonElement obj, params string[] names)
        {
            foreach (var prop in obj.EnumerateObject())
                foreach (var n in names)
                    if (string.Equals(prop.Name, n, StringComparison.OrdinalIgnoreCase))
                        return prop.Value.ValueKind == JsonValueKind.String ? prop.Value.GetString() : prop.Value.ToString();
            return null;
        }

        private static int? GetIntCI(JsonElement obj, params string[] names)
        {
            foreach (var prop in obj.EnumerateObject())
                foreach (var n in names)
                    if (string.Equals(prop.Name, n, StringComparison.OrdinalIgnoreCase))
                        return prop.Value.ValueKind == JsonValueKind.Number && prop.Value.TryGetInt32(out var v) ? v
                             : int.TryParse(prop.Value.ToString(), out var v2) ? v2 : null;
            return null;
        }

        private static async Task<string> ReadBodySafeAsync(HttpResponseMessage res)
        {
            try
            {
                var txt = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(txt)) return $"{(int)res.StatusCode} {res.ReasonPhrase}";
                return txt.Length > 800 ? txt[..800] + "..." : txt;
            }
            catch { return $"{(int)res.StatusCode} {res.ReasonPhrase}"; }
        }

        // ---------------- sessionStorage helpers ----------------
        private Task<string?> GetItemAsync(string key)
            => _js.InvokeAsync<string?>("sessionStorage.getItem", key).AsTask();

        private Task SetItemAsync(string key, string value)
            => _js.InvokeVoidAsync("sessionStorage.setItem", key, value).AsTask();

        private Task RemoveItemAsync(string key)
            => _js.InvokeVoidAsync("sessionStorage.removeItem", key).AsTask();
    }
}
