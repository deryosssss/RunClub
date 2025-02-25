using RunClubAPI.Models;
using RunClub.DTOs;

namespace RunClubAPI.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize); // ✅ Add parameters
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(UserDTO user);
        Task UpdateUserAsync(int id, UserDTO user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int roleId); // ✅ Add missing method
    }

}
