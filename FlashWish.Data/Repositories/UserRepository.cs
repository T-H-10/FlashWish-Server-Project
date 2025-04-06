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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return await _dbSet.Include(u=>u.Roles).FirstOrDefaultAsync(user => user.Email == email);
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
                PasswordHash =password, //do hash before!
                CreatedAt = DateTime.Now,
                GreetingCards = new List<GreetingCard>(),
                UpdatedAt = DateTime.Now
            };

            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id.ToString();
        }
    }
}
