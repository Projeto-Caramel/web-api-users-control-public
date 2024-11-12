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

        public async Task<bool> ImageExistsAsync(string key)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                var response = await _s3Client.GetObjectMetadataAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<string> GetImageAsBase64Async(string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            using var response = await _s3Client.GetObjectAsync(request);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            // Converte a imagem para Base64
            return Convert.ToBase64String(imageBytes);
        }

        private string GetImageUrl(string key)
        {
            return $"https://{_bucketName}.s3.sa-east-1.amazonaws.com/{key}";
        }
    }
}
