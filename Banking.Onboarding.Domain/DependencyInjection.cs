using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Services;
using Banking.Onboarding.Domain.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Onboarding.Domain;
public static class DependencyInjection
{
    public static void InjectDomain(this IServiceCollection services)
    {
        services.AddScoped<DocumentoscopyResponseCallback>();
        services.AddScoped<GenerateDocumentUploadUrl>();
        services.AddScoped<HandleDocumentoscopyService>();
        services.AddScoped<NotifyEventToCustomerEmail>();
        services.AddScoped<SendToDocumentoscopy>();
        services.AddSingleton<IFileKeyGenerator, FileKeyGenerator>();
    }
}
