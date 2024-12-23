using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Banking.Onboarding.Domain.UseCases;



namespace Banking.Onboarding.Lambdas;

public record GenerateUploadLinkRequest(string Name, string Cpf, string Email, string DocumentType);

public class DocumentUploadLink
{
    private const string policies = "AWSLambdaBasicExecutionRole,OnboardingSSMParameterAccessPolicy,OnboardingDynamoDbRWPolicy";
    private readonly GenerateDocumentUploadUrl generateDocumentUploadUrl;
    private readonly ICustomerRepository customerRepository;
    public DocumentUploadLink(GenerateDocumentUploadUrl generateDocumentUploadUrl, ICustomerRepository customerRepository)
    {
        this.generateDocumentUploadUrl = generateDocumentUploadUrl;
        this.customerRepository = customerRepository;
    }

    [LambdaFunction(ResourceName = "LambdaOnboardingGenerateUploadLink")]
    [HttpApi(LambdaHttpMethod.Post, "/costumer-validation")]
    public async Task<IHttpResult> GenerateUploadLink([FromBody] GenerateUploadLinkRequest request)
    {
        var customerInfo = new CustomerInfo(request.Name, request.Cpf, request.Email);

        var docType = Enum.Parse<DocumentType>(request.DocumentType, true);

        var links = generateDocumentUploadUrl.Generate(customerInfo, docType);

        await customerRepository.AddCustomer(customerInfo);

        return HttpResults.Ok(links);
    }
}
