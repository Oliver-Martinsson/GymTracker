using System.Net;
using System.Text;

namespace GymTracker.Tests.TestHelpers;

public class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _json;

    public Uri? LastRequestUri { get; private set; }

    public StubHttpMessageHandler(HttpStatusCode statusCode, string json = "")
    {
        _statusCode = statusCode;
        _json = json;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequestUri = request.RequestUri;

        var response = new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_json, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(response);
    }
}