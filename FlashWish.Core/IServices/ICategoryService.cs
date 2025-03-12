using FlashWish.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO category);
        Task<CategoryDTO?> UpdateCategoryAsync(int id, CategoryDTO category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
