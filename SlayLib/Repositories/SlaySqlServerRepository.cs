using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Interfaces;
using SlayLib.Models;
namespace SlayLib.Repositories
{
    public class SlaySqlServerRepository : BaseSqlServerRepository<ApplicationDbContext>, IMitRepository
    {
        public SlaySqlServerRepository(ApplicationDbContext db) : base(db)
        {
        }
       
/*        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var users = await FindAsync<ApplicationUser>(u => u.Email == email);
            return users.FirstOrDefault();
        }*/
    }
}
