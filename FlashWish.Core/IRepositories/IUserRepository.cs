using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IRepositories
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<User?> GetByEmailAsync(string email);

        public Task<IEnumerable<UserWithRolesDTO>> GetUsersWithRolesAsync();

        public Task<bool> AddAdminRoleAsync(int userId);
        public Task<bool> RemoveAdminRoleAsync(int userId);
        //public Task<bool> UserEmailExistsAsync(string email);
        //Task<string?> CreateUserAsync(string email, string password);
    }
}
