using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.Models;
using Microsoft.AspNetCore.Identity;


namespace RunClubAPI.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<bool> RoleExistsAsync(string roleName);
        Task AddRoleAsync(IdentityRole role);
        Task DeleteRoleAsync(string roleId);
    }
}
