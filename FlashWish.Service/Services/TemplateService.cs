using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DotNetEnv;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
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
        public async Task<TemplateDTO> AddTemplateAsync(TemplateDTO template)
        {
            var templateToAdd = _mapper.Map<Template>(template);
            if (templateToAdd == null || templateToAdd.ImageURL == null)
            {
                return null;
            }
            var url = await UploadToCloud(templateToAdd.ImageURL,templateToAdd.TemplateName);
            if(url == null) {
                return null;
            }
            templateToAdd.ImageURL = url.ToString();
            await _repositoryManager.Templates.AddAsync(templateToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<TemplateDTO>(templateToAdd);

        }

        public async Task<bool> DeleteTemplateAsync(int id)
        {
            var templateDTO = await _repositoryManager.Templates.GetByIdAsync(id);
            if (templateDTO == null)
            {
                return false;
            }
            var templateToDelete = _mapper.Map<Template>(templateDTO);
            if (templateToDelete == null)
            {
                return false;
            }
            await _repositoryManager.Templates.DeleteAsync(templateToDelete);
            await _repositoryManager.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<TemplateDTO>> GetAllTemplatesAsync()
        {
            var templates = await _repositoryManager.Templates.GetAllAsync();
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

        private async Task<Uri> UploadToCloud(string path, string name)
        {
            // טען את קובץ ה-.env
            Env.Load();
            Console.WriteLine(Env.GetString("CLOUDINARY_CLOUD_NAME"));
            // קבל את הערכים
            var cloudName = Env.GetString("CLOUDINARY_CLOUD_NAME");
            var apiKey = Env.GetString("CLOUDINARY_API_KEY");
            var apiSecret = Env.GetString("CLOUDINARY_API_SECRET");
            Console.WriteLine("cloudName "+ cloudName);
            Console.WriteLine("apiKey " + apiKey);
            Console.WriteLine("apiSecret " + apiSecret);
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
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    PublicId = name,  // שם הקובץ בענן
                    Overwrite = false,  // מחיקת קובץ קודם עם אותו שם
                    Transformation = new Transformation().Width(500).Height(500).Crop("limit") // שינוי גודל
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl;
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
