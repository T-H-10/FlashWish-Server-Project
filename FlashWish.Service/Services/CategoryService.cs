using AutoMapper;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;

namespace FlashWish.Service.Services
{
    public class CategoryService : ICategoryService
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
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return null;
            }
            var existingCategory = await _repositoryManager.Categories.GetByNameAsync(category.CategoryName);
            if (existingCategory != null)
            {
                return null;
            }
            var categoryToAdd = _mapper.Map<Category>(category);
            categoryToAdd.CreatedAt = DateTime.UtcNow;
            categoryToAdd.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Categories.AddAsync(categoryToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<CategoryDTO>(categoryToAdd);

        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryDTO =await _repositoryManager.Categories.GetByIdAsync(id);
            //if (categoryDTO == null)
            //{
            //    return false;
            //}
            //var categoryToDelete = _mapper.Map<Category>(categoryDTO);
            if (categoryDTO == null)
            {
                return false;
            }
            await _repositoryManager.Categories.DeleteAsync(categoryDTO);
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
            categoryToUpdate.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Categories.UpdateAsync(id, categoryToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<CategoryDTO?>(categoryToUpdate);
        }
    }
}
