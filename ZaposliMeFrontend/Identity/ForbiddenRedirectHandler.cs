using Microsoft.AspNetCore.Components;
using System.Net;

namespace ZaposliMe.Frontend.Identity
{
    public class ForbiddenRedirectHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;

        public ForbiddenRedirectHandler(NavigationManager navigation)
        {
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                _navigation.NavigateTo("/forbidden");
            }

            return response;
        }
    }

}
