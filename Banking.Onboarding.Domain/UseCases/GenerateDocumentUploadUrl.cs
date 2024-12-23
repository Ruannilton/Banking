using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.UseCases;
public class GenerateDocumentUploadUrl
{
    private readonly IUploadUrlGenerator urlGenerator;

    public GenerateDocumentUploadUrl(IUploadUrlGenerator urlGenerator)
    {
        this.urlGenerator = urlGenerator;
    }

    public DocumetUploadUrl Generate(CustomerInfo customerInfo, DocumentType documentType)
    {
        var frontUrl = urlGenerator.GenerateUploadUrl(customerInfo.Cpf, documentType, DocumentSide.Front);
        var backUrl = urlGenerator.GenerateUploadUrl(customerInfo.Cpf, documentType, DocumentSide.Back);

        return new DocumetUploadUrl
        {
            BackSideUrl = backUrl,
            FrontSideUrl = frontUrl,
        };
    }
}
