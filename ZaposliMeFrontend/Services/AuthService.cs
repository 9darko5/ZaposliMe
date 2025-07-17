using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ZaposliMe.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, NavigationManager navigationManager, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                (_authStateProvider as CookieAuthStateProvider)?.NotifyAuthChanged();
                return true;
            }

            return false;
        }

        public async Task<bool> Register(string email, string password, string confirmPassword)
        {
            var response = await _httpClient.PostAsJsonAsync("register", new
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            });

            return response.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("logout", null);
            (_authStateProvider as CookieAuthStateProvider)?.NotifyAuthChanged();
            _navigationManager.NavigateTo("/");
        }

        public async Task<bool> RefreshToken()
        {
            var response = await _httpClient.PostAsync("refresh", null);
            if (response.IsSuccessStatusCode)
            {
                (_authStateProvider as CookieAuthStateProvider)?.NotifyAuthChanged();
                return true;
            }

            return false;
        }

        public async Task<UserInfo?> GetCurrentUser()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserInfo>("user");
            }
            catch
            {
                return null;
            }
        }
    }
}
