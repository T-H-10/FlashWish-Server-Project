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
    class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _dbSet;
        public TemplateRepository(DataContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }
    }
}
