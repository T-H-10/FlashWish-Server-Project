using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IRepositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<Category> GetByNameAsync(string categoryName);
    }
}
