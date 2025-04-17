using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using DiarioObras.Configurations;
using Microsoft.Extensions.Options;

namespace DiarioObras.Services;

public class S3Service
{
    private readonly AwsS3Settings _settings;

    public S3Service(IOptions<AwsS3Settings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> UploadArquivoAsync(Stream arquivoStream, string nomeArquivo, string contentType)
    {
        var s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = arquivoStream,
            Key = nomeArquivo, // Ex: "fotos/obra123/minhafoto.jpg"
            BucketName = _settings.BucketName,
            ContentType = contentType,
            //CannedACL = S3CannedACL.PublicRead // para deixar acessível publicamente
        };

        var transferUtility = new TransferUtility(s3Client);
        await transferUtility.UploadAsync(uploadRequest);

        return $"https://{_settings.BucketName}.s3.us-east-2.amazonaws.com/{nomeArquivo}";

    }
}
