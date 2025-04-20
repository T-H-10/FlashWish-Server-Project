using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FlashWish.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Service.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                var publicId = ExtractPublicIdFromUrl(imageUrl);

                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);
                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image from Cloudinary: {ex.Message}");
                return false;
            }
        }

        private string ExtractPublicIdFromUrl(string url)
        {
            // דוגמה להוצאת publicId מתוך URL
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex < 0 || uploadIndex + 1 >= segments.Length)
            {
                throw new ArgumentException("Invalid Cloudinary URL");
            }
            var pathParts = segments.Skip(uploadIndex + 1).ToArray();
            var filePath = string.Join("/", pathParts);

            // הסר רק את הסיומת האחרונה
            var lastDotIndex = filePath.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                return filePath.Substring(0, lastDotIndex);
            }

            return filePath;
            //return Path.Combine(fileName[..fileName.LastIndexOf('.')]);
        }
    }
}
