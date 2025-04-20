using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface ICloudinaryService
    {
        //Task<string> UploadImageAsync(IFormFile imageFile);
        Task<bool> DeleteImageAsync(string imageUrl);
        //Task<string> GetImageUrlAsync(string publicId);
    }
}
