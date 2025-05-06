using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DotNetEnv;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Service.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public TemplateService(IRepositoryManager repositoryManager, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<TemplateDTO> AddTemplateAsync(TemplateDTO template, IFormFile imageFile)
        {
            var templateToAdd = _mapper.Map<Template>(template);
            if (templateToAdd == null || imageFile == null)
            {
                return null;
            }
            var url = await UploadToCloud(imageFile);
            if (url == null)
            {
                return null;
            }
            var segments = url.Segments;
            var index = Array.FindIndex(segments, s => s.Trim('/').Equals("upload", StringComparison.OrdinalIgnoreCase));
            if (index>=0 && index+1<segments.Length)
            {
                var relativePath= string.Join("", segments.Skip(index + 1));
                templateToAdd.ImageURL = relativePath;
            }
            templateToAdd.CreatedAt = DateTime.UtcNow;
            templateToAdd.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Templates.AddAsync(templateToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<TemplateDTO>(templateToAdd);

        }

        //public static string ExtractCloudinaryRelativePath(string fullUrl)//יחזיר רק את החלק שצריך לשמור בDB.
        //{
        //    if (string.IsNullOrEmpty(fullUrl))
        //        return string.Empty;

        //    var uri = new Uri(fullUrl);
        //    var segments = uri.Segments;

        //    // חיפוש תחילת הנתיב הרלוונטי - אחרי "upload/"
        //    var index = Array.FindIndex(segments, s => s.Trim('/').Equals("upload", StringComparison.OrdinalIgnoreCase));
        //    if (index >= 0 && index + 1 < segments.Length)
        //    {
        //        // מחזיר את כל מה שאחרי /upload/
        //        var relativePath = string.Join("", segments.Skip(index + 1));
        //        return Uri.UnescapeDataString(relativePath);
        //    }

        //    return string.Empty;
        //}

        //מחיקה מוחלטת מענן
        //public async Task<bool> DeleteTemplateAsync(int id)
        //{
        //    var template = await _repositoryManager.Templates.GetByIdAsync(id);
        //    //var templateDTO = _mapper.Map<TemplateDTO>(template);
        //    if (template == null)
        //    {
        //        throw new Exception("הרקע לא נמצא.");
        //    }
        //    if(!template.MarkedForDeletion)
        //    {
        //        return false;
        //    }
        //    var cardsUsingTemplate = await _repositoryManager.GreetingCards.GetAllAsync(card => card.TemplateID == id);
        //    if (cardsUsingTemplate.Any())
        //    {
        //        return false;
        //    }
        //    // מחיקת התמונה מהענן
        //    if (!string.IsNullOrEmpty(template.ImageURL))
        //    {
        //        await _cloudinaryService.DeleteImageAsync(template.ImageURL);
        //    }

        //    //var templateToDelete = _mapper.Map<Template>(template);
        //    //if (templateToDelete == null)
        //    //{
        //    //    return false;
        //    //}
        //    await _repositoryManager.Templates.DeleteAsync(template);
        //    await _repositoryManager.SaveAsync();
        //    return true;
        //}


        //private async Task DeleteImageFromCloud(string imageURL)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> MarkTemplateForDeletionAsync(int id)
        {
            var template = await _repositoryManager.Templates.GetByIdAsync(id);
            if (template == null)
            { return false; }

            template.MarkedForDeletion = true;
            await _repositoryManager.Templates.UpdateAsync(id, template);
            await _repositoryManager.SaveAsync();

            var cardsUsingTemplate = await _repositoryManager.GreetingCards.GetAllAsync(card => card.TemplateID == id);
            if(!cardsUsingTemplate.Any())
            {
                // מחיקת התמונה מהענן
                if (!string.IsNullOrEmpty(template.ImageURL))
                {
                    await _cloudinaryService.DeleteImageAsync(template.ImageURL);
                }
                await _repositoryManager.Templates.DeleteAsync(template);
                await _repositoryManager.SaveAsync();
            }
            return true;
        }


        public async Task<IEnumerable<TemplateDTO>> GetAllTemplatesAsync()
        {
            var templates = await _repositoryManager.Templates.GetAllAsync(template => !template.MarkedForDeletion);
            return _mapper.Map<IEnumerable<TemplateDTO>>(templates);
        }

        public async Task<TemplateDTO?> GetTemplateByIdAsync(int id)
        {
            var template = await _repositoryManager.Templates.GetByIdAsync(id);
            return _mapper.Map<TemplateDTO>(template);
        }

        public async Task<TemplateDTO?> UpdateTemplateAsync(int id, TemplateDTO template)
        {
            if (template == null)
            {
                return null;
            }
            var templateToUpdate = _mapper.Map<Template>(template);
            templateToUpdate.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Templates.UpdateAsync(id, templateToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<TemplateDTO?>(templateToUpdate);

        }

        private async Task<Uri> UploadToCloud(IFormFile imageFile)
        {
            // טען את קובץ ה-.env
            Env.Load();
            // קבל את הערכים
            var cloudName = Env.GetString("CLOUDINARY_CLOUD_NAME");
            var apiKey = Env.GetString("CLOUDINARY_API_KEY");
            var apiSecret = Env.GetString("CLOUDINARY_API_SECRET");

            // הגדרות עם הנתונים שלך (החלף עם הערכים האמיתיים שלך)
            var account = new Account(
                cloudName,      // שנה לערך מה-Cloudinary Dashboard
                apiKey,         // שנה לערך מה-Cloudinary Dashboard
                apiSecret       // שנה לערך מה-Cloudinary Dashboard
            );

            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true; // שימוש ב-HTTPS
            try
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream); // העתקת הקובץ ל-MemoryStream
                    stream.Position = 0; // החזרת המצביע להתחלה

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imageFile.FileName, stream),
                        PublicId = imageFile.FileName,  // שם הקובץ בענן
                        Overwrite = false,  // מחיקת קובץ קודם עם אותו שם
                        Transformation = new Transformation().Width(500).Height(500).Crop("limit") // שינוי גודל
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
