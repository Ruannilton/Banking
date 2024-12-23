using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Banking.Onboarding.Domain.Abstractions;

namespace Banking.Onboarding.Lambdas;
public class ReceiveDocumentoscopyResponse
{
    private readonly IDocumentoscopyResponseQueue documentoscopyResponseQueue;
    const string policies = "OnboardingWriteDocumentoscopyResponseQueuePolicy,OnboardingSSMParameterAccessPolicy";
    public ReceiveDocumentoscopyResponse(IDocumentoscopyResponseQueue documentoscopyResponseQueue)
    {
        this.documentoscopyResponseQueue = documentoscopyResponseQueue;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingReceiveDocumentoscopyResponse")]
    [HttpApi(LambdaHttpMethod.Post, "/documentoscopy-callback")]
    public async Task<IHttpResult> GenerateUploadLink(APIGatewayHttpApiV2ProxyRequest request)
    {
        var body = request.Body;

        await documentoscopyResponseQueue.EnqueueResponse(body);

        return HttpResults.Ok();
    }
}
