using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SQS;
using Banking.Onboarding.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Onboarding.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IServiceCollection services)
    {
        services.AddAWSService<AmazonS3Client>();
        services.AddAWSService<AmazonCognitoIdentityProviderClient>();
        services.AddAWSService<AmazonDynamoDBClient>();
        services.AddAWSService<AmazonSimpleEmailServiceClient>();
        services.AddAWSService<AmazonSQSClient>();
        services.AddScoped<HttpClient>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDocumentoscopyResponseQueue, DocumentoscopyResponseQueue>();
        services.AddScoped<IDocumentoscopyService, DocumentoscopyService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IOnboardingEventQueue, OnboardingEventQueue>();
        services.AddScoped<IUploadUrlGenerator, UploadUrlGenerator>();
    }
}
