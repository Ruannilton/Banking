using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Banking.Onboarding.Domain.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Banking.Onboarding.Infrastructure;
internal class EmailSender : IEmailSender
{
    private const string senderEnvName = "EMAIL_SENDER_ADDRESS";
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly string sender;

    public EmailSender(IAmazonSimpleEmailService sesClient, IConfiguration configuration)
    {
        _sesClient = sesClient;
        sender = configuration[senderEnvName] ?? throw new Exception($"invalid env:{senderEnvName}");
    }
    public async Task SendEmail(string receiver, string subject, string body)
    {
        var emailRequest = new SendEmailRequest()
        {
            Source = sender,
            Destination = new Destination([receiver]),
            Message = new Message(new(subject), new(new(body)))
        };

        var sendResponse = await _sesClient.SendEmailAsync(emailRequest);
    }
}
