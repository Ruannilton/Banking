using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.UseCases;
public class NotifyEventToCustomerEmail
{
    private readonly IEmailSender emailSender;

    public NotifyEventToCustomerEmail(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }

    public async Task Execute(OnboardingEvent e)
    {
        var subject = MapSubject(e);
        var body = BuildBody(e);
        await emailSender.SendEmail(e.CustomerEmail, subject, body);
    }

    private string MapSubject(OnboardingEvent e)
    {
        return "";
    }

    private string BuildBody(OnboardingEvent e) {
        return "";
    }
}
