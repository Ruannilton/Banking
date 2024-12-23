using Amazon.SQS;
using Amazon.SQS.Model;
using Banking.Onboarding.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Banking.Onboarding.Infrastructure;
internal class DocumentoscopyResponseQueue : IDocumentoscopyResponseQueue
{
    private const string queueUrlEnvName = "DOCUMENTOSCOPYRESPONSEQUEUE_QUEUE_URL";
    private readonly IAmazonSQS sqsClient;
    private readonly string queueUrl;

    public DocumentoscopyResponseQueue(IAmazonSQS sqsClient, IConfiguration configuration)
    {
        this.sqsClient = sqsClient;
        queueUrl = configuration[queueUrlEnvName] ?? throw new Exception($"invalid env: {queueUrlEnvName} ");

    }

    public async Task EnqueueResponse(string content)
    {
        var request = new SendMessageRequest(queueUrl, content);

        await sqsClient.SendMessageAsync(request);
    }

}
