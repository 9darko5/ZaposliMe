using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace ZaposliMe.Frontend.Identity
{

    public class JwtHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public JwtHandler(IJSRuntime js) => _js = js;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            // Get token just-in-time from sessionStorage
            var token = await _js.InvokeAsync<string?>("sessionStorage.getItem", "access_token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, ct);
        }
    }

}
