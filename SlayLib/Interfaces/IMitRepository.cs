using System.Threading.Tasks;
using SlayLib.Models;

namespace SlayLib.Interfaces
{
    public interface IMitRepository : IRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}
