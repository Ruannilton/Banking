namespace Banking.Onboarding.Domain.Abstractions;
public interface IDocumentoscopyResponseQueue
{
    Task EnqueueResponse(string content);
}
