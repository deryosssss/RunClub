using RunClubAPI.Models;  // For the User model.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Interfaces  // Use the appropriate namespace for interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
