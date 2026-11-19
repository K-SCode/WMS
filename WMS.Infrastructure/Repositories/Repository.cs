using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces.Repositories;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity,cancellationToken);
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
                _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) =>
            await _dbSet.ToListAsync(cancellationToken);

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _dbSet.FindAsync([id], cancellationToken: cancellationToken);

        public void SetExpectedVersion(T entity, byte[] version)
        {
            var token = _dbSet.Entry(entity).Properties.FirstOrDefault(p => p.Metadata.IsConcurrencyToken) ??
                throw new InvalidOperationException($"{typeof(T).Name} has no concurrency token configured.");
            token.OriginalValue = version;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
