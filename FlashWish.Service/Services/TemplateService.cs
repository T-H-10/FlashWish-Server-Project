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
        public TemplateService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
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
            templateToAdd.ImageURL = url.ToString();
            await _repositoryManager.Templates.AddAsync(templateToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<TemplateDTO>(templateToAdd);

        }

        //מחיקה מוחלטת מענן
        public async Task<bool> DeleteTemplateAsync(int id)
        {
            var template = await _repositoryManager.Templates.GetByIdAsync(id);
            //var templateDTO = _mapper.Map<TemplateDTO>(template);
            if (template == null)
            {
                throw new Exception("הרקע לא נמצא.");
            }
            if(!template.MarkedForDeletion)
            {
                return false;
            }
            var cardsUsingTemplate = await _repositoryManager.GreetingCards.GetAllAsync(card => card.TemplateID == id);
            if (cardsUsingTemplate.Any())
            {
                return false;
            }
            // מחיקת התמונה מהענן
            if (!string.IsNullOrEmpty(template.ImageURL))
            {
                await DeleteImageFromCloud(template.ImageURL);
            }

            //var templateToDelete = _mapper.Map<Template>(template);
            //if (templateToDelete == null)
            //{
            //    return false;
            //}
            await _repositoryManager.Templates.DeleteAsync(template);
            await _repositoryManager.SaveAsync();
            return true;
        }

        private async Task DeleteImageFromCloud(string imageURL)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MarkTemplateForDeletionAsync(int id)
        {
            var template = await _repositoryManager.Templates.GetByIdAsync(id);
            if (template == null)
            { return false; }
            template.MarkedForDeletion = true;
            await _repositoryManager.Templates.UpdateAsync(template.TemplateID, template);
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
            await _repositoryManager.Templates.UpdateAsync(id, templateToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<TemplateDTO?>(templateToUpdate);

        }

        private async Task<Uri> UploadToCloud(IFormFile imageFile)
        {
            // טען את קובץ ה-.env
            Env.Load();
            Console.WriteLine(Env.GetString("CLOUDINARY_CLOUD_NAME"));
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
                //Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
