using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

public class ProxyController : ApiController
{
    private readonly HttpClient _httpClient;

    public ProxyController()
    {
        _httpClient = new HttpClient();
    }

    [Route("exotel-proxy/{*path}")]
    [HttpPut, HttpPost, HttpGet, HttpDelete, HttpPatch, HttpHead, HttpOptions]
    public async Task<IHttpActionResult> ProxyRequest()
    {
        var exotelApiUrl = "https://ccm-api.exotel.in";
        var request = Request;
        var method = new HttpMethod(request.Method.Method);

        var uri = new Uri($"{exotelApiUrl}/{request.RequestUri.PathAndQuery}");

        using (var proxyRequest = new HttpRequestMessage(method, uri))
        {
            // Copy headers
            foreach (var header in request.Headers)
            {
                proxyRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Copy content
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsByteArrayAsync();
                proxyRequest.Content = new ByteArrayContent(content);
                proxyRequest.Content.Headers.Clear();
                foreach (var header in request.Content.Headers)
                {
                    proxyRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Send the request to the Exotel API
            var response = await _httpClient.SendAsync(proxyRequest);

            // Copy the response back to the client
            var proxyResponse = new HttpResponseMessage(response.StatusCode)
            {
                Content = response.Content
            };

            foreach (var header in response.Headers)
            {
                proxyResponse.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return ResponseMessage(proxyResponse);
        }
    }
}
