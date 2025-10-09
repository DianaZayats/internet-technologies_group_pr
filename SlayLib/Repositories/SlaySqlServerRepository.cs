using Microsoft.EntityFrameworkCore;
using SlayLib.Models;
namespace SlayLib.Repositories
{
    public class SlaySqlServerRepository<TDbContext> : BaseSqlServerRepository<TDbContext>
        where TDbContext : DbContext
    {
        public SlaySqlServerRepository(TDbContext db) : base(db)
        {
        }

       
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var users = await FindAsync<ApplicationUser>(u => u.Email == email);
            return users.FirstOrDefault();
        }
    }
}
