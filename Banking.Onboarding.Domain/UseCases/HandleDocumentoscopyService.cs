using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banking.Onboarding.Domain.UseCases;
public class HandleDocumentoscopyService
{
    private readonly IOnboardingEventQueue eventNotifyer;
    private readonly ICustomerRepository customerRepository;
    private readonly IAuthService authService;
    public HandleDocumentoscopyService(IOnboardingEventQueue eventNotifyer, IAuthService authService, ICustomerRepository customerRepository)
    {
        this.eventNotifyer = eventNotifyer;
        this.authService = authService;
        this.customerRepository = customerRepository;
    }

    public async Task HandleDocumentoscopyResponse(string cpf, bool valid)
    {
        
        var customerInfo = await customerRepository.GetCustomer(cpf);

        //TODO: handle error
        if (customerInfo is null)
        {
            return;
        }

        if (!valid)
        {
            _ = eventNotifyer.PushEvent(new DocumentoscopyResponseInvalidEvent(customerInfo.Name, customerInfo.Email));
            return;
        }

        var created = await authService.CreateCustomerAccount(customerInfo);

        if (created)
        {
            _ = eventNotifyer.PushEvent(new CustomerCredentialsCreatedEvent(customerInfo.Name, customerInfo.Email));
        }
    }
}
