namespace Banking.Onboarding.Domain.Abstractions;
public interface IEmailSender
{
    Task SendEmail(string receiver, string subject, string body);
}
