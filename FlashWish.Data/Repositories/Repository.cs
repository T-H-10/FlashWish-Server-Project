using FlashWish.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Data.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T?> UpdateAsync(int id, T entity)
        {
            if (entity == null)
            {
                return null;
            }
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
            {
                return null;
            }
            var properties = typeof(T).GetProperties();
            var keyProperties = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            foreach (var property in properties)
            {
                // בדוק אם המאפיין הוא ID
                if (!keyProperties.Any(k => k.Name == property.Name) 
                    && property.CanWrite 
                    && property.Name != "Password"
                    )
                {
                    var newValue = property.GetValue(entity);
                    var currentValue = property.GetValue(existingEntity);

                    if ((property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) &&
    (DateTime)newValue == default)
                    {
                        continue;
                    }


                    if (newValue!=null && !Equals(newValue, currentValue))
                    {
                        property.SetValue(existingEntity, newValue);
                    }
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return existingEntity;
        }
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
