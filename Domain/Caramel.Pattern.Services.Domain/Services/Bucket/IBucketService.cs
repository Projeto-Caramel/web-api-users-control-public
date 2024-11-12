namespace Caramel.Pattern.Services.Domain.Services.Bucket
{
    public interface IBucketService
    {
        Task<string> UploadFileAsync(string base64Image, string key);
        Task<bool> ImageExistsAsync(string key);
        Task<string> GetImageAsBase64Async(string key);
    }
}
