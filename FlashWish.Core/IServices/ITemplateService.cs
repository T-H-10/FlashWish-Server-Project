using FlashWish.Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface ITemplateService
    {
        Task<IEnumerable<TemplateDTO>> GetAllTemplatesAsync();
        Task<TemplateDTO?> GetTemplateByIdAsync(int id);
        Task<TemplateDTO> AddTemplateAsync(TemplateDTO template, IFormFile imageFile);
        Task<TemplateDTO?> UpdateTemplateAsync(int id, TemplateDTO template);
        Task<bool> DeleteTemplateAsync(int id);

    }
}
