using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if(entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);
        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
