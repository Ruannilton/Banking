using Amazon.Lambda.Annotations;
using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.SQSEvents;
using Banking.Onboarding.Domain.Models;
using Banking.Onboarding.Domain.UseCases;
using System.Text.Json;

namespace Banking.Onboarding.Lambdas;
internal class EmailDispatcher
{
    private const string policies = "OnboardingSendEmailPolicy,OnboardingSSMParameterAccessPolicy";
    private readonly NotifyEventToCustomerEmail notifyEventToCustomer;

    public EmailDispatcher(NotifyEventToCustomerEmail notifyEventToCustomer)
    {
        this.notifyEventToCustomer = notifyEventToCustomer;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingEmailDispatcher")]
    public async Task Dispatch(SQSEvent snsEvent)
    {
        foreach (var record in snsEvent.Records)
        {
            var body = record.Body;
            var onboardingEvent = JsonSerializer.Deserialize<OnboardingEvent>(body);
            
            if(onboardingEvent is null)
            {
                // TODO: handle error
                continue;
            }

            await notifyEventToCustomer.Execute(onboardingEvent);
        }
    }
}
