using Amazon.Lambda.Annotations;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;

namespace Banking.Onboarding.Lambdas;
public class NotificationWorker
{
    private readonly IAmazonSimpleNotificationService amazonSNS;
    private readonly string topicArn;
    private const string topicArnEnvName = "notifyEventCustomerTopic";
    private const string policies = "OnboardingRaedDocumentoscopyResponseQueuePolicy,OnboardingWriteToNotifyEventCustomerTopicPolicy";

    public NotificationWorker(IAmazonSimpleNotificationService amazonSNS, IConfiguration configuration)
    {
        this.amazonSNS = amazonSNS;
        topicArn = configuration[topicArnEnvName] ?? throw new Exception($"invalid {topicArnEnvName}"); ;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingNotificationWorker")]
    public async Task HandleNotification(SQSEvent sqsEvent)
    {
        foreach (var record in sqsEvent.Records) { 
            var body = record.Body;
            await amazonSNS.PublishAsync(topicArn, body);
        }
    }
}
