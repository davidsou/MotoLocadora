using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.BuildingBlocks.Options;

namespace MotoLocadora.Infrastructure.Services;


public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Options _options;

    public S3StorageService(IAmazonS3 s3Client, IOptions<S3Options> options)
    {
        _s3Client = s3Client;
        _options = options.Value;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var request = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _options.BucketName,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(request);

        return $"{_options.BaseUrl}/{fileName}";
    }

    public async Task<Stream?> GetAsync(string fileName)
    {
        var response = await _s3Client.GetObjectAsync(_options.BucketName, fileName);
        return response.ResponseStream;
    }

    public async Task DeleteAsync(string fileName)
    {
        await _s3Client.DeleteObjectAsync(_options.BucketName, fileName);
    }
}
