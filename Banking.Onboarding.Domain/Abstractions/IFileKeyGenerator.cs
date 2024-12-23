using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;

public interface IFileKeyGenerator
{
    string GetFileKey(string cpf, DocumentType type, DocumentSide side);
}