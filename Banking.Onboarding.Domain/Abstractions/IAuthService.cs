using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;
public interface IAuthService
{
    Task<bool> CreateCustomerAccount(CustomerInfo customerInfo);
}
