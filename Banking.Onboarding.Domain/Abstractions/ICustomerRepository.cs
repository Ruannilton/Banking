using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;

public interface ICustomerRepository
{
    public Task AddCustomer(CustomerInfo costumerInfo);
    public Task<CustomerInfo> GetCustomer(string cpf);
}