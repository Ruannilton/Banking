using Banking.Onboarding.Domain.Abstractions;

namespace Banking.Onboarding.Domain.UseCases;
public class DocumentoscopyResponseCallback
{
    private readonly IDocumentoscopyResponseQueue responseQueue;

    public DocumentoscopyResponseCallback(IDocumentoscopyResponseQueue responseQueue)
    {
        this.responseQueue = responseQueue;
    }

    async Task EnqueueMessage(string message)
    {
        await responseQueue.EnqueueResponse(message);
    }
}
