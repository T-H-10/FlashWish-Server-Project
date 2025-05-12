using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return await _dbSet.Include(u => u.Roles).FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<string?> CreateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = new User
            {
                Email = email,
                Password =password, //do hash before!
                //CreatedAt = DateTime.UtcNow,
                GreetingCards = new List<GreetingCard>(),
                //UpdatedAt = DateTime.UtcNow
            };

            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id.ToString();
        }

        public async Task<IEnumerable<UserWithRolesDTO>> GetUsersWithRolesAsync()
        {
            var users= await _dbSet.Include(u => u.Roles).ToListAsync();
            return users.Select(u => new UserWithRolesDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.Roles.Select(r => r.RoleName).ToList(),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }

        public async Task<bool> AddAdminRoleAsync(int userId)
        {
            var user = await _dbSet.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
            if(adminRole == null)
            {
                return false;
            }
            if (!user.Roles.Any(r => r.RoleName == "Admin"))
            {
                user.Roles.Add(adminRole);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAdminRoleAsync(int userId)
        {
            var user= await _dbSet.Include(u=>u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                return false;
            }

            var adminRole = user.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            if( adminRole == null)
            {
                return false;
            }
            var totalAdmins = await _dbSet.Where(u => u.Roles.Any(r => r.RoleName == "Admin")).CountAsync();
            if (totalAdmins <= 1)
            {
                return false;
            }
            user.Roles.Remove(adminRole);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> UserEmailExistsAsync(string email)
        //{
        //    return await _dbSet.AnyAsync(u => u.Email == email);
        //}
    }
}
