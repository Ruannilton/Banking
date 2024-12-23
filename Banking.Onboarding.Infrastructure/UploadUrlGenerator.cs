using Amazon.S3;
using Amazon.S3.Model;
using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Banking.Onboarding.Infrastructure;
internal class UploadUrlGenerator : IUploadUrlGenerator
{
    private readonly IFileKeyGenerator keyGenerator;
    private readonly IAmazonS3 s3Client;
    private readonly IConfiguration configuration;
    public UploadUrlGenerator(IFileKeyGenerator keyGenerator, IAmazonS3 s3Client, IConfiguration configuration)
    {
        this.keyGenerator = keyGenerator;
        this.s3Client = s3Client;
        this.configuration = configuration;
    }
    public string GenerateUploadUrl(string cpf, DocumentType type, DocumentSide side)
    {
        var key = keyGenerator.GetFileKey(cpf, type, side);
        var bucketName = configuration["ONBOARDINGIDENTITYBUCKET_BUCKET_NAME"];
        var expirationTimeInMinutesVar = configuration["EXPIRATION_TIME_IN_MINUTES"];

        if (string.IsNullOrEmpty(bucketName))
        {
            throw new Exception("invalid ONBOARDINGIDENTITYBUCKET_BUCKET_NAME");
        }

        if (!int.TryParse(expirationTimeInMinutesVar, out var expirationTime))
        {
            expirationTime = 15;
        }

        var presignedRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(expirationTime),
            ContentType = "image/jpeg"
        };

        var url = s3Client.GetPreSignedURL(presignedRequest);

        return url;
    }
}

