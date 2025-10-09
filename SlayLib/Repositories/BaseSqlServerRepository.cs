using Microsoft.EntityFrameworkCore;
using SlayLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SlayLib.Repositories
{
    public class BaseSqlServerRepository<TDbContext> : IRepository
        where TDbContext : DbContext
    {
        protected readonly TDbContext Db;

        public BaseSqlServerRepository(TDbContext db)
        {
            Db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await Db.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await Db.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await Db.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            Db.Set<T>().Update(entity);
            await SaveChangesAsync();
        }

        public async Task RemoveAsync<T>(T entity) where T : class
        {
            Db.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await Db.Set<T>().AnyAsync(predicate);
        }
    }
}
