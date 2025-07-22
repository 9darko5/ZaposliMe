using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text;

namespace ZaposliMe.Frontend.Helpers
{
    public class FetchWithCredentialsHttpHandler : HttpMessageHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public FetchWithCredentialsHttpHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Prepare headers dictionary
            var headers = new Dictionary<string, string>();
            foreach (var header in request.Headers)
            {
                // Skip headers restricted by fetch, like Host, Content-Length, etc.
                // Only add headers that are allowed to be set.
                headers[header.Key] = string.Join(",", header.Value);
            }

            // Prepare body to send via JS interop
            object? body = null;
            if (request.Content != null)
            {
                // Read content and determine type
                var contentHeaders = request.Content.Headers;

                if (contentHeaders.ContentType != null)
                {
                    var mediaType = contentHeaders.ContentType.MediaType;

                    if (mediaType == "application/json")
                    {
                        // Read content as string (JSON)
                        body = await request.Content.ReadAsStringAsync(cancellationToken);
                    }
                    else if (mediaType == "application/x-www-form-urlencoded")
                    {
                        // Form URL encoded content
                        body = await request.Content.ReadAsStringAsync(cancellationToken);
                    }
                    else if (mediaType.StartsWith("text/"))
                    {
                        // Text content
                        body = await request.Content.ReadAsStringAsync(cancellationToken);
                    }
                    else
                    {
                        // Other content types (e.g. multipart/form-data, octet-stream)
                        var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                        // Convert bytes to Base64 string because JS fetch expects strings/blobs
                        body = Convert.ToBase64String(bytes);
                    }
                }
                else
                {
                    // No Content-Type set, just read as string fallback
                    body = await request.Content.ReadAsStringAsync(cancellationToken);
                }
            }

            // Prepare fetch options for JS interop
            var fetchOptions = new
            {
                method = request.Method.Method,
                headers = headers,
                body = body,
                credentials = "include"
            };

            // Call JS fetch wrapper
            var fetchResponse = await _jsRuntime.InvokeAsync<FetchResponse>(
                "fetchWithCredentials.send",
                request.RequestUri!.ToString(),
                fetchOptions);

            // Create HttpResponseMessage from fetchResponse
            var response = new HttpResponseMessage((System.Net.HttpStatusCode)fetchResponse.Status)
            {
                RequestMessage = request,
                ReasonPhrase = fetchResponse.StatusText,
            };

            // Copy headers from fetch response
            foreach (var header in fetchResponse.Headers)
            {
                if (!response.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    response.Content ??= new ByteArrayContent(Array.Empty<byte>());
                    response.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Set content if body present
            if (!string.IsNullOrEmpty(fetchResponse.Body))
            {
                if (fetchResponse.IsBase64)
                {
                    var contentBytes = Convert.FromBase64String(fetchResponse.Body);
                    response.Content = new ByteArrayContent(contentBytes);
                    if (fetchResponse.ContentType != null)
                        response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(fetchResponse.ContentType);
                }
                else
                {
                    response.Content = new StringContent(fetchResponse.Body, Encoding.UTF8, fetchResponse.ContentType ?? "text/plain");
                }
            }
            else
            {
                response.Content = new StringContent("");
            }

            return response;
        }

        private class FetchResponse
        {
            public int Status { get; set; }
            public string StatusText { get; set; } = "";
            public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
            public string Body { get; set; } = "";
            public string? ContentType { get; set; }
            public bool IsBase64 { get; set; } = false;
        }
    }
}
