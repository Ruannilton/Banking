using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.SQSEvents;
using Banking.Onboarding.Domain.UseCases;
using System.Text.Json;

namespace Banking.Onboarding.Lambdas;

record DocumentoscopyResponseParams(string cpf, bool valid);

internal class HandleDocumentoscopyResponse
{
    private readonly HandleDocumentoscopyService handleDocumentoscopyService;
    const string policies = "OnboardingReadDocumentoscopyResponseQueuePolicy,OnboardingDynamoDbRWPolicy,CognitoCreateUser,OnboardingSSMParameterAccessPolicy";
    public HandleDocumentoscopyResponse(HandleDocumentoscopyService handleDocumentoscopyService)
    {
        this.handleDocumentoscopyService = handleDocumentoscopyService;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingHandleDocumentoscopyResponse")]
    public async Task HandleMessages(SQSEvent sqsEvent)
    {
        foreach (var record in sqsEvent.Records)
        {
            var body = record.Body;
            var docParams = GetParams(body);

            if(docParams == null)
            {
                // handle error
                continue;
            }

            await handleDocumentoscopyService.HandleDocumentoscopyResponse(docParams.cpf, docParams.valid);
        }


    }

    // should be customized for the real documentoscopy response
    private static DocumentoscopyResponseParams? GetParams(string body)
    {
        var responseDocument = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);

        if (responseDocument == null)
        {
            return null;
        }

        var cpf = responseDocument["cpf"].GetString();

        if (cpf is null)
        {
            return null;
        }

        var valid = responseDocument["valid"].GetBoolean();

        return new DocumentoscopyResponseParams(cpf, valid);
    }
}
