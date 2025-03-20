using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Data.Repositories
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<Category> GetByNameAsync(string categoryName)
        {
            if (categoryName == null)
            {
                return null;
            }
            return await _context.Categories.FirstOrDefaultAsync(category => category.CategoryName == categoryName);
        }
    }
}
