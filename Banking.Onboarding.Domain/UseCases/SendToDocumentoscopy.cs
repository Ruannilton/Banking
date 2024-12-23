using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Events;
using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.UseCases;
public class SendToDocumentoscopy
{
    private readonly IDocumentoscopyService documentoscopyService;
    private readonly IFileRepository fileRepository;
    private readonly IFileKeyGenerator fileKeyGenerator;
    private readonly ICustomerRepository customerRepository;
    private readonly IOnboardingEventQueue onboardingEventQueue;
    private readonly string callbackUrl;

    public SendToDocumentoscopy(IDocumentoscopyService documentoscopyService, IFileRepository fileRepository, IFileKeyGenerator fileKeyGenerator, ICustomerRepository customerRepository, IOnboardingEventQueue onboardingEventQueue)
    {
        this.documentoscopyService = documentoscopyService;
        this.fileRepository = fileRepository;
        this.fileKeyGenerator = fileKeyGenerator;
        this.customerRepository = customerRepository;
        callbackUrl = Environment.GetEnvironmentVariable("callbackUrl") ?? throw new ArgumentNullException("callbackUrl env missing");
        this.onboardingEventQueue = onboardingEventQueue;
    }

    public async Task Execute(string cpf, DocumentType documentType)
    {
        var keyFront = fileKeyGenerator.GetFileKey(cpf, documentType, DocumentSide.Front);
        var keyBack = fileKeyGenerator.GetFileKey(cpf, documentType, DocumentSide.Back);

        if (!await fileRepository.ExistsFile(keyBack)) return;
        if (!await fileRepository.ExistsFile(keyFront)) return;

        var fileFront = fileRepository.GetFile(keyFront);
        var fileBack = fileRepository.GetFile(keyBack);

        var streams = await Task.WhenAll(fileFront,fileBack);

        var customerInfo = await customerRepository.GetCustomer(cpf);
        var documentoscopyRequest = new DocumentoscopyRequest()
        {
            Cpf = cpf,
            Documents = new[]
            {
                new DocumentStream(streams[0],DocumentSide.Front),
                new DocumentStream(streams[1],DocumentSide.Back),
            },
            DocumentType = documentType,
            Name = customerInfo.Name,
            CallbackUrl = callbackUrl
        };

       await documentoscopyService.RequestDocumentoscopy(documentoscopyRequest);
        _ = onboardingEventQueue.PushEvent(new DocumentSentToDocumentoscopyEvent(customerInfo.Name, customerInfo.Email));
    }
}
