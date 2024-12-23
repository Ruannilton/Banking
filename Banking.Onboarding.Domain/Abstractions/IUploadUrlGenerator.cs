using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;
public interface IUploadUrlGenerator
{
     string GenerateUploadUrl(string cpf, DocumentType type, DocumentSide side);
}
