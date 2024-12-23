using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Services;
internal class FileKeyGenerator : IFileKeyGenerator
{
    public string GetFileKey(string cpf, DocumentType type, DocumentSide side)
    {
        var typeName = Enum.GetName(type)!.ToLower();
        var sideName = Enum.GetName(side)!.ToLower();

        return $"{cpf}/{typeName}/{sideName}";
    }
}
