using System.Net;
using System.Text;

namespace Comex.Testes;

// Utilizado apenas pelos testes e pela demonstração explicitamente offline.
internal class RespostaHttpSimulada : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder;

    public RespostaHttpSimulada(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder)
    {
        this.responder = responder;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => responder(request, cancellationToken);

    public static HttpResponseMessage CriarJson(string json, HttpStatusCode status = HttpStatusCode.OK) => new(status)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
    };
}
