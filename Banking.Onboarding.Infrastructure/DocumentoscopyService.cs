using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Banking.Onboarding.Infrastructure;
record DocumentoscopyRequestBody(string cpf, string srcDocumentFront, string srcDocumentBack, string callbackRoute);
internal class DocumentoscopyService : IDocumentoscopyService
{
    private readonly HttpClient httpClient;
    private string documentoscopyServiceEndpoint;
    const string endpointEnvName = "DOCUMENTOSCOPY_SERVICE_ENDPOINT";
    public DocumentoscopyService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        documentoscopyServiceEndpoint = configuration[endpointEnvName] ?? throw new Exception($"invalid {endpointEnvName}");
    }
    public async Task RequestDocumentoscopy(DocumentoscopyRequest request)
    {
        var docFront = request.Documents.Where(x => x.Side == DocumentSide.Front).Select(x => x.Stream).FirstOrDefault();
        var docBack = request.Documents.Where(x => x.Side == DocumentSide.Back).Select(x => x.Stream).FirstOrDefault();

        var b64Front = StreamToBase64(docFront);
        var b64Back = StreamToBase64(docBack);

        var body = new DocumentoscopyRequestBody(request.Cpf, b64Front, b64Back, "");
        await httpClient.PostAsJsonAsync(documentoscopyServiceEndpoint, body);
    }

    private string StreamToBase64(Stream? stream)
    {
        if (stream == null) return string.Empty;
        var buff = new byte[stream.Length];
        stream.Read(buff);
        return Convert.ToBase64String(buff);
    }
}
