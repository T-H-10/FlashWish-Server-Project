using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IRepository
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<User?> GetByEmailAsync(string email);

    }
}
