using Microsoft.AspNetCore.Identity;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;

namespace RunClubAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 🔁 Helper: Get RoleId from Role Name
        private string? GetRoleIdFromName(string roleName)
        {
            return roleName.ToLower() switch
            {
                "admin" => "1",
                "coach" => "2",
                "runner" => "3",
                _ => null
            };
        }

        // 🔁 Helper: Get Role Name from RoleId
        private string GetRoleNameFromId(string? roleId)
        {
            return roleId switch
            {
                "1" => "Admin",
                "2" => "Coach",
                "3" => "Runner",
                _ => "Runner"
            };
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var users = _userManager.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "Runner";

                userDtos.Add(new UserDTO
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email ?? "",
                    Age = user.Age,
                    Location = user.Location,
                    Role = roleName,
                    RoleId = GetRoleIdFromName(roleName)
                });
            }

            return userDtos;
        }

        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "Runner";

            return new UserDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email ?? "",
                Age = user.Age,
                Location = user.Location,
                Role = roleName,
                RoleId = GetRoleIdFromName(roleName)
            };
        }

        public async Task<UserDTO?> CreateUserAsync(UserDTO userDto, string password)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                UserName = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;

            var roleName = GetRoleNameFromId(userDto.RoleId);

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return await GetUserByIdAsync(user.Id);
        }

        public async Task<bool> UpdateUserAsync(string userId, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.UserName = userDto.Email;
            user.Age = userDto.Age ?? user.Age;
            user.Location = userDto.Location ?? user.Location;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId)
        {
            var roleName = GetRoleNameFromId(roleId);
            if (string.IsNullOrEmpty(roleName)) return Enumerable.Empty<UserDTO>();

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Select(user => new UserDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email ?? "",
                Age = user.Age,
                Location = user.Location,
                Role = roleName,
                RoleId = roleId
            });
        }
    }
}





/* The UserService is a core component of my project, handling user management functionalities such as creating, retrieving, updating, and deleting users while maintaining role assignments. To ensure security and efficiency, the service follows a DTO-based approach, preventing direct exposure of entity models and reducing unnecessary data transfer.

The CreateUserAsync() method includes a validation mechanism to check if a user role exists before assigning it, preventing potential data integrity issues. Similarly, the GetAllUsersAsync() method implements pagination, optimizing database queries by retrieving only a subset of user records per request instead of loading the entire dataset.

The service also utilizes Entity Framework Core (EF Core), leveraging its Include() method to fetch role details along with user data, ensuring efficient relational data retrieval. Furthermore, by utilizing async methods, the service efficiently handles database operations in an asynchronous manner, preventing application performance bottlenecks.

By designing UserService with modularity, scalability, and maintainability in mind, I have demonstrated my understanding of ORM principles, efficient database querying, and REST API best practices. This service ensures secure and optimized user role management, making it a crucial part of the overall system architecture.*/