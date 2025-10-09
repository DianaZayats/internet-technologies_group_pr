using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SlayLib.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task RemoveAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();
        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    }
}
