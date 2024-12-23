using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using System.Net.Http.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Mock.DocumentoscopyService;

public record DocumentoscopyRequest(string cpf, string srcDocumentFront, string srcDocumentBack, string callbackRoute);
public record DocumentoscopyResponse(string cpf, bool valid);

public class Functions
{
    private readonly HttpClient httpClient;

    public Functions(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }


    [LambdaFunction()]
    [RestApi(LambdaHttpMethod.Post, "/validate-document")]
    public IHttpResult Post([FromBody] DocumentoscopyRequest request, ILambdaContext context)
    {
        var unmaskCpf = string.Join("",request.cpf.Where(c => char.IsDigit(c)));
        var cpfNum = long.Parse(unmaskCpf);

        var approve = cpfNum % 2 == 0;

        var response = new DocumentoscopyResponse(unmaskCpf, approve);

        _ = httpClient.PostAsJsonAsync(request.callbackRoute, response);

        return HttpResults.Ok();
    }
}
