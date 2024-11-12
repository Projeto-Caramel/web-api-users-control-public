using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Caramel.Pattern.Services.Domain.Services.Bucket;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application.Services.Bucket
{
    [ExcludeFromCodeCoverage]
    public class BucketService : IBucketService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public BucketService(IConfiguration configuration)
        {
            var accessKey = configuration["BucketSettings:AccessKey"];
            var secretKey = configuration["BucketSettings:SecretKey"];
            _bucketName = configuration["BucketSettings:BucketName"];

            _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.SAEast1);
        }

        public async Task<string> UploadFileAsync(string base64Image, string key)
        {
            // Converte a string base64 para um array de bytes
            byte[] imageData = Convert.FromBase64String(base64Image);

            using var memoryStream = new MemoryStream(imageData);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = memoryStream,
                ContentType = "image/jpeg"
            };

            var response = await _s3Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return GetImageUrl(key);
            else
                throw new Exception("Falha ao salvar a imagem na Base.");
        }

        private string GetImageUrl(string key)
        {
            return $"https://{_bucketName}.s3.sa-east-1.amazonaws.com/{key}";
        }
    }
}
