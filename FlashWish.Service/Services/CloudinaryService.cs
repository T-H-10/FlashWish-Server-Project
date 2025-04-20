using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FlashWish.Core.IServices;
using System;
using System.Linq;
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
                var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
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
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');

            // אנחנו מחפשים את החלק אחרי "/upload/"
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex < 0 || uploadIndex + 1 >= segments.Length)
            {
                throw new ArgumentException("Invalid Cloudinary URL");
            }

            // חותכים את החלק של ה-upload
            var pathParts = segments.Skip(uploadIndex + 1).ToArray();
            var filePath = string.Join("/", pathParts);

            // הסר את ה-version אם קיים
            var versionIndex = filePath.IndexOf("v", StringComparison.OrdinalIgnoreCase);
            if (versionIndex >= 0)
            {
                // נסיר את ה-version על ידי חיתוך החלק שמופיע אחרי ה-v
                filePath = filePath.Substring(filePath.IndexOf("/", versionIndex) + 1);
            }

            // הסר את הסיומת האחרונה
            var lastDotIndex = filePath.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                // במקרה של שתי סיומות, כמו .jpeg.jpg, נוודא שנסיר את .jpg בלבד
                if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && filePath.Contains(".jpeg"))
                {
                    return filePath.Substring(0, filePath.Length - 4); // הסר .jpg
                }
                return filePath.Substring(0, lastDotIndex); // הסר את הסיומת
            }

            return filePath;
        }
    }
}


//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
//using FlashWish.Core.IServices;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FlashWish.Service.Services
//{
//    public class CloudinaryService : ICloudinaryService
//    {
//        private readonly Cloudinary _cloudinary;
//        public CloudinaryService(Cloudinary cloudinary)
//        {
//            _cloudinary = cloudinary;
//        }
//        public async Task<bool> DeleteImageAsync(string imageUrl)
//        {
//            try
//            {
//                var publicId = ExtractPublicIdFromUrl(imageUrl);

//                // נסה למחוק עם השם המלא
//                var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
//                if (result.Result == "ok")
//                    return true;

//                //// נסה שוב בלי הסיומת האחרונה
//                //var withoutLastExtension = RemoveLastExtension(publicId);
//                //if (withoutLastExtension != publicId)
//                //{
//                //    result = await _cloudinary.DestroyAsync(new DeletionParams(withoutLastExtension));
//                //    if (result.Result == "ok")
//                //        return true;
//                //}

//                //// נסה בלי שתי הסיומות (אם יש)
//                //var withoutAllExtensions = RemoveAllExtensions(publicId);
//                //if (withoutAllExtensions != publicId)
//                //{
//                //    result = await _cloudinary.DestroyAsync(new DeletionParams(withoutAllExtensions));
//                //    return result.Result == "ok";
//                //}

//                return false;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error deleting image from Cloudinary: {ex.Message}");
//                return false;
//            }
//        }

//        private string ExtractPublicIdFromUrl(string url)
//        {
//            var uri = new Uri(url);
//            var segments = uri.AbsolutePath.Split('/');
//            var uploadIndex = Array.IndexOf(segments, "upload");
//            if (uploadIndex < 0 || uploadIndex + 1 >= segments.Length)
//            {
//                throw new ArgumentException("Invalid Cloudinary URL");
//            }

//            var pathParts = segments.Skip(uploadIndex + 1).ToArray();
//            var filePath = string.Join("/", pathParts);

//            // אם הקובץ נגמר ב .jpg אבל לפני כן יש כבר סיומת כמו .jpeg -> נסיר רק את .jpg
//            if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && filePath.Contains(".jpeg"))
//            {
//                return filePath.Substring(0, filePath.Length - 4); // בלי .jpg
//            }

//            // אחרת – הסר את הסיומת הרגילה
//            var lastDotIndex = filePath.LastIndexOf('.');
//            return lastDotIndex > 0 ? filePath.Substring(0, lastDotIndex) : filePath;
//        }

//    }
//}
