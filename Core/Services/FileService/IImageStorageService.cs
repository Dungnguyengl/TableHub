using Microsoft.AspNetCore.Http;

namespace Core.Services.FileService
{
    public interface IImageStorageService
    {
        Task<string> StoreFileAsync(IFormFile file);
        Task<string> ChangeImageAsync(string oldImageUrl, IFormFile newFile);
        Task<bool> DeleteForeverAsync(string imageUrl);
    }
}