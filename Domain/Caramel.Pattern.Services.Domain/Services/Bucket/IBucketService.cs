namespace Caramel.Pattern.Services.Domain.Services.Bucket
{
    public interface IBucketService
    {
        Task<string> UploadFileAsync(string base64Image, string key);
    }
}
