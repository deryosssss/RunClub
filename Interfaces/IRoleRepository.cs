using RunClubAPI.Models;  // For the Role model.
using Microsoft.EntityFrameworkCore;
using RunClubAPI.DTOs;


namespace RunClubAPI.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
    }
}
