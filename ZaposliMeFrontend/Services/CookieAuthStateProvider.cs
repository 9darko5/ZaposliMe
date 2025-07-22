using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ZaposliMe.Frontend.Services
{
    public class CookieAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CookieAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Call /manage/info GET endpoint to get user info from cookie-based auth
                var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("/manage/info");

                if (userInfo?.IsAuthenticated == true)
                {
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userInfo.Claims, "Cookies"));
                    return new AuthenticationState(claimsPrincipal);
                }
            }
            catch
            {
                // ignored — user not authenticated or error
            }

            return new AuthenticationState(_anonymous);
        }

        public void NotifyUserAuthentication()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public Claim[] Claims { get; set; }
    }
}
