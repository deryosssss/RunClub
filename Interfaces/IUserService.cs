using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.DTOs;

namespace RunClubAPI.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> CreateUserAsync(UserDTO userDto);  // ✅ Ensure role is included
        Task<bool> UpdateUserAsync(int id, UserDTO user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId);
    }
}


