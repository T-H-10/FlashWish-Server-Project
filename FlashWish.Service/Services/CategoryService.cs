using AutoMapper;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;

namespace FlashWish.Service.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public CategoryService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO category)
        {
            var categoryToAdd = _mapper.Map<Category>(category);
            if (category != null)
            {
                await _repositoryManager.Categories.AddAsync(categoryToAdd);
                await _repositoryManager.SaveAsync();
                return _mapper.Map<CategoryDTO>(categoryToAdd);
            }
            return null;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryDTO = _repositoryManager.Categories.GetByIdAsync(id);
            if (categoryDTO == null)
            {
                return false;
            }
            var categoryToDelete = _mapper.Map<Category>(categoryDTO);
            if (categoryToDelete == null)
            {
                return false;
            }
            await _repositoryManager.Categories.DeleteAsync(categoryToDelete);
            await _repositoryManager.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _repositoryManager.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _repositoryManager.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO?> UpdateCategoryAsync(int id, CategoryDTO category)
        {
            if (category == null)
            {
                return null;
            }
            var categoryToUpdate = _mapper.Map<Category>(category);
            await _repositoryManager.Categories.UpdateAsync(id, categoryToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<CategoryDTO?>(categoryToUpdate);
        }
    }
}
