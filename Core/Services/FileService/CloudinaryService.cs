using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services.FileService
{
    public class CloudinaryService : IImageStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private readonly string _cloudName;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _uploadFolder;

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
        {
            _cloudName = configuration ["Cloudinary:CloudName"] ?? string.Empty;
            _apiKey = configuration ["Cloudinary:ApiKey"] ?? string.Empty;
            _apiSecret = configuration ["Cloudinary:ApiSecret"] ?? string.Empty;
            _uploadFolder = configuration ["Cloudinary:UploadFolder"] ?? "unknows";

            var account = new Account(_cloudName, _apiKey, _apiSecret);
            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }

        /// <summary>
        /// Stores an image file to Cloudinary and returns the image URL
        /// </summary>
        /// <param name="file">The image file to upload</param>
        /// <returns>URL of the uploaded image</returns>
        public async Task<string> StoreFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file was provided for upload");
            ValidateFileType(file);

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString(), stream),
                Folder = _uploadFolder,
                UseFilename = true,
                UniqueFilename = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Error uploading image to Cloudinary: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }

        /// <summary>
        /// Changes an existing image with a new one and returns the new image URL
        /// </summary>
        /// <param name="oldImageUrl">URL of the existing image to replace</param>
        /// <param name="newFile">The new image file to upload</param>
        /// <returns>URL of the new uploaded image</returns>
        public async Task<string> ChangeImageAsync(string oldImageUrl, IFormFile newFile)
        {
            ValidateFileType(newFile);

            // First delete the old image if provided
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                await DeleteForeverAsync(oldImageUrl);
            }

            // Then upload the new image
            return await StoreFileAsync(newFile);
        }

        /// <summary>
        /// Permanently deletes an image from Cloudinary
        /// </summary>
        /// <param name="imageUrl">URL of the image to delete</param>
        /// <returns>True if deletion was successful</returns>
        public async Task<bool> DeleteForeverAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            // Extract public ID from the URL
            string publicId = ExtractPublicIdFromUrl(imageUrl);

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }

        /// <summary>
        /// Extracts the public ID from a Cloudinary URL
        /// </summary>
        private string ExtractPublicIdFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            try
            {
                // Handle different URL formats from Cloudinary
                // Example URL: https://res.cloudinary.com/cloud-name/image/upload/v1234567890/products/image-name.jpg

                var uri = new Uri(imageUrl);

                // Verify this is a Cloudinary URL
                if (!uri.Host.Contains("cloudinary.com"))
                    throw new ArgumentException("Not a valid Cloudinary URL");

                var pathSegments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                // We need to find "upload" segment and skip the version segment
                int uploadIndex = Array.IndexOf(pathSegments, "upload");

                if (uploadIndex == -1)
                    throw new ArgumentException("Could not find 'upload' segment in Cloudinary URL");

                // Skip the version segment (starts with 'v') and join the rest
                int startIndex = uploadIndex + 2; // Skip "upload" and version segment

                if (startIndex >= pathSegments.Length)
                    throw new ArgumentException("URL format doesn't match expected Cloudinary structure");

                // Join remaining segments to form the public ID
                string publicId = string.Join("/", pathSegments [startIndex..]);

                // Remove file extension if present
                if (publicId.Contains('.'))
                {
                    publicId = publicId[..publicId.LastIndexOf('.')];
                }

                return publicId;
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                // Log the exception
                _logger.LogError(ex, "Error extracting public ID");
                throw new ArgumentException($"Failed to parse Cloudinary URL: {imageUrl}", ex);
            }
        }

        private void ValidateFileType(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string [] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException($"File type {extension} is not supported");
        }
    }
}
