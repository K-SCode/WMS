using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void Delete(T entity, CancellationToken cancellationToken);
        void Update(T entity);
    }
}
