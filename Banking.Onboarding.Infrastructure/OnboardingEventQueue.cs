using Amazon.SQS;
using Amazon.SQS.Model;
using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Banking.Onboarding.Infrastructure;

internal class OnboardingEventQueue : IOnboardingEventQueue
{
    private readonly IAmazonSQS sqsClient;
    private readonly string queueUrl;
    private const string queueUrlEnvName = "ONBOARDINGEVENTQUEUE_QUEUE_URL";

    public OnboardingEventQueue(IAmazonSQS sqsClient, IConfiguration configuration)
    {
        this.sqsClient = sqsClient;
        queueUrl = configuration[queueUrlEnvName] ?? throw new Exception($"invalid env:{queueUrlEnvName}");
    }

    public async Task PushEvent<E>(E e) where E : OnboardingEvent
    {
        var content = JsonSerializer.Serialize(e);
        var request = new SendMessageRequest(queueUrl, content);
        await sqsClient.SendMessageAsync(request);
    }
}
