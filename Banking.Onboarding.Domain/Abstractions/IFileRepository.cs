namespace Banking.Onboarding.Domain.Abstractions;
public interface IFileRepository
{
    Task<Stream> GetFile(string key);
    Task<bool> ExistsFile(string key);
}
