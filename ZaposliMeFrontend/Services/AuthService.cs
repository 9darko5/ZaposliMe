using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ZaposliMe.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginData = new { email = email, password = password };
            var response = await _httpClient.PostAsJsonAsync("/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                // Notify auth state changed so UI updates
                ((CookieAuthStateProvider)_authStateProvider).NotifyUserAuthentication();
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var registerData = new { email = email, password = password };
            var response = await _httpClient.PostAsJsonAsync("/register", registerData);

            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            // If your API has a logout endpoint, call it here
            // Or clear cookies server-side when session ends

            // Notify auth state changed for UI update
            ((CookieAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
