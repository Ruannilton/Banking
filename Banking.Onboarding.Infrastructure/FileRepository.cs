using Amazon.S3;
using Banking.Onboarding.Domain.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Banking.Onboarding.Infrastructure;
internal class FileRepository : IFileRepository
{
    private readonly IAmazonS3 s3Client;
    private readonly string? bucketName;

    public FileRepository(IAmazonS3 s3Client, IConfiguration configuration)
    {
        this.s3Client = s3Client;
        bucketName = configuration["ONBOARDINGIDENTITYBUCKET_BUCKET_NAME"];

        if (string.IsNullOrEmpty(bucketName))
        {
            throw new Exception("invalid ONBOARDINGIDENTITYBUCKET_BUCKET_NAME");
        }
    }

    public async Task<bool> ExistsFile(string key)
    {
        try
        {
            _ = await s3Client.GetObjectMetadataAsync(bucketName, key);
            return true;
        }
        catch (Exception ex)
        {
            return false;
            throw;
        }
    }

    public async Task<Stream> GetFile(string key)
    {
        var response = await s3Client.GetObjectAsync(bucketName, key);
        var stream = new MemoryStream();
        response.ResponseStream.CopyTo(stream);
        return stream;
    }
}
