using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;
public interface IDocumentoscopyService
{
    public Task RequestDocumentoscopy(DocumentoscopyRequest request);
}
