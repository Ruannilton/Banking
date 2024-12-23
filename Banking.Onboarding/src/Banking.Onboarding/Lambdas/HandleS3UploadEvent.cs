using Amazon.Lambda.Annotations;
using Amazon.Lambda.S3Events;
using Banking.Onboarding.Domain.Models;
using Banking.Onboarding.Domain.UseCases;

namespace Banking.Onboarding.Lambdas;

public record SendToDocumentoscopyParams(string cpf, DocumentType documentType);

public class HandleS3UploadEvent
{
    private readonly SendToDocumentoscopy sendToDocumentoscopy;
    private const string policies = "DynamoDbRWPolicy,IdentityDocumentBucketPolicy,WriteOnboardingEventQueuePolicy,OnboardingSSMParameterAccessPolicy";
    public HandleS3UploadEvent(SendToDocumentoscopy sendToDocumentoscopy)
    {
        this.sendToDocumentoscopy = sendToDocumentoscopy;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingHandleS3UploadEvent")]
    public async Task ListenIdentityDocuments(S3Event evnt)
    {
        var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();

        var events = eventRecords.Select(x => x.S3).Where(x => x != null);

        foreach (var s3Event in events)
        {
            var parameters = GetParams(s3Event);

            await sendToDocumentoscopy.Execute(parameters.cpf, parameters.documentType);
        }
    }

    private static SendToDocumentoscopyParams GetParams(S3Event.S3Entity s3Event)
    {
        var objectKey = s3Event.Object.Key;
        var keyPars = objectKey.Split('/');
        var cpf = keyPars[0];
        var documentTypeStr = keyPars[1];
        var docType = Enum.Parse<DocumentType>(documentTypeStr, true);
        var parameters = new SendToDocumentoscopyParams(cpf, docType);
        return parameters;
    }
}
