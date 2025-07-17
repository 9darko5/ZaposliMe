using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ZaposliMe.Frontend.Services
{
    public class CookieAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CookieAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("user");
                if (userInfo?.IsAuthenticated ?? false)
                {
                    var identity = new ClaimsIdentity(userInfo.Claims.Select(c => new Claim(c.Type, c.Value)), "cookies");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch
            {
                // fallback to anonymous
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyAuthChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public List<UserClaim> Claims { get; set; } = new();
    }

    public class UserClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
