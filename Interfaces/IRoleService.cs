using RunClubAPI.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
    }
}