namespace Banking.Onboarding.Domain.Models;
public class DocumentoscopyRequest
{
    public string Cpf { get; set; }
    public string Name { get; set; }
    public DocumentType DocumentType { get; set; }
    public DocumentStream[] Documents { get; set; }

    public string CallbackUrl { get; set; }
}

public record DocumentStream(Stream Stream, DocumentSide Side);
