using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;

public class UserService : IUserService
{
    private readonly RunClubContext _context;

    public UserService(RunClubContext context)
    {
        _context = context;
    }

    // ✅ Create User (With Role)
    public async Task<UserDTO?> CreateUserAsync(UserDTO userDto)
    {
        if (string.IsNullOrEmpty(userDto.RoleId))
        {
            return null; // 🚨 Ensure RoleId is provided
        }

        // ✅ Fetch the Role from the database
        var role = await _context.Roles.FindAsync(userDto.RoleId);
        if (role == null)
        {
            return null; // 🚨 Role not found
        }

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            RoleId = userDto.RoleId,  // ✅ Assign RoleId
            Role = role  // ✅ Assign Role object
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            RoleId = user.RoleId,
            Role = new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleNormalizedName = role.RoleNormalizedName
            }
        };
    }

    // ✅ Get All Users (With Pagination)
    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId,
                Role = user.Role != null ? new RoleDTO
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.RoleName,
                    RoleNormalizedName = user.Role.RoleNormalizedName
                } : null
            })
            .ToListAsync();
    }

    // ✅ Get User by ID
    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null) return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            RoleId = user.RoleId,
            Role = user.Role != null ? new RoleDTO
            {
                RoleId = user.Role.RoleId,
                RoleName = user.Role.RoleName,
                RoleNormalizedName = user.Role.RoleNormalizedName
            } : null
        };
    }

    // ✅ Update User
    public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.RoleId = userDto.RoleId;

        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ Delete User
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ Get Users by Role
    public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId)
    {
        return await _context.Users
            .Where(u => u.RoleId == roleId)
            .Include(u => u.Role)
            .Select(user => new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId,
                Role = user.Role != null ? new RoleDTO
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.RoleName,
                    RoleNormalizedName = user.Role.RoleNormalizedName
                } : null
            })
            .ToListAsync();
    }
}
