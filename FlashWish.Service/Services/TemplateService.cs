using AutoMapper;
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
    public class TemplateService:ITemplateService
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
            if (template != null)
            {
                await _repositoryManager.Templates.AddAsync(templateToAdd);
                await _repositoryManager.SaveAsync();
                return _mapper.Map<TemplateDTO>(templateToAdd);
            }
            return null;
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
    }
}
