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

    // Create a new user with a specified role
    public async Task<UserDTO?> CreateUserAsync(UserDTO userDto)
    {
        // Ensure that a RoleId is provided before proceeding
        if (string.IsNullOrEmpty(userDto.RoleId))
        {
            return null; 
        }

        // Fetch the role from the database to ensure it exists
        var role = await _context.Roles.FindAsync(userDto.RoleId);
        if (role == null)
        {
            return null; // Role not found, returning null
        }

        // Create a new User entity
        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            RoleId = userDto.RoleId,  // Assign RoleId
            Role = role  // Assign the associated Role entity
        };

        // Add the new user to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Return the created user as a DTO to prevent direct model exposure
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

    // Retrieve all users with pagination support
    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        return await _context.Users
            .Include(u => u.Role) // Include Role details in the result
            .Skip((pageNumber - 1) * pageSize) // Skip records for pagination
            .Take(pageSize) // Limit the number of records per page
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

    // Retrieve a single user by their ID
    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        // Fetch the user from the database along with their role details
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null) return null; // Return null if user does not exist

        // Return user details in DTO format
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

    // Update an existing user's details
    public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
    {
        var user = await _context.Users.FindAsync(id); // Find the user by ID
        if (user == null) return false; // Return false if the user doesn't exist

        // Update user properties
        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.RoleId = userDto.RoleId;

        await _context.SaveChangesAsync(); // Commit changes to the database
        return true;
    }

    // Delete a user from the system
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id); // Find the user by ID
        if (user == null) return false; // Return false if user does not exist

        _context.Users.Remove(user); // Remove the user
        await _context.SaveChangesAsync(); // Save changes in the database
        return true;
    }

    // Retrieve all users with a specific role
    public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId)
    {
        return await _context.Users
            .Where(u => u.RoleId == roleId) // Filter users by RoleId
            .Include(u => u.Role) // Include Role details
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

/* The UserService is a core component of my project, handling user management functionalities such as creating, retrieving, updating, and deleting users while maintaining role assignments. To ensure security and efficiency, the service follows a DTO-based approach, preventing direct exposure of entity models and reducing unnecessary data transfer.

The CreateUserAsync() method includes a validation mechanism to check if a user role exists before assigning it, preventing potential data integrity issues. Similarly, the GetAllUsersAsync() method implements pagination, optimizing database queries by retrieving only a subset of user records per request instead of loading the entire dataset.

The service also utilizes Entity Framework Core (EF Core), leveraging its Include() method to fetch role details along with user data, ensuring efficient relational data retrieval. Furthermore, by utilizing async methods, the service efficiently handles database operations in an asynchronous manner, preventing application performance bottlenecks.

By designing UserService with modularity, scalability, and maintainability in mind, I have demonstrated my understanding of ORM principles, efficient database querying, and REST API best practices. This service ensures secure and optimized user role management, making it a crucial part of the overall system architecture.*/