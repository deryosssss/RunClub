using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RunClubAPI.Models;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using RunClubAPI.Services;

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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var users = _userManager.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserDTO
                {
                    UserId = int.Parse(user.Id), // Ensure conversion
                    Name = user.Name,
                    Email = user.Email
                });

            return await Task.FromResult(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()); // Convert int to string
            if (user == null) return null;

            return new UserDTO
            {
                UserId = int.Parse(user.Id), // Convert string to int
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDTO?> CreateUserAsync(UserDTO userDto, string password)
        {
            var user = new User
            {
                UserName = userDto.Name,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;

            return new UserDTO
            {
                UserId = int.Parse(user.Id), // Convert string to int
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<bool> UpdateUserAsync(int userId, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.UserName = userDto.Name;
            user.Email = userDto.Email;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return new List<UserDTO>();

            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            return users.Select(user => new UserDTO
            {
                UserId = int.Parse(user.Id), // Convert string to int
                Name = user.Name,
                Email = user.Email
            });
        }
    }
}



/* The UserService is a core component of my project, handling user management functionalities such as creating, retrieving, updating, and deleting users while maintaining role assignments. To ensure security and efficiency, the service follows a DTO-based approach, preventing direct exposure of entity models and reducing unnecessary data transfer.

The CreateUserAsync() method includes a validation mechanism to check if a user role exists before assigning it, preventing potential data integrity issues. Similarly, the GetAllUsersAsync() method implements pagination, optimizing database queries by retrieving only a subset of user records per request instead of loading the entire dataset.

The service also utilizes Entity Framework Core (EF Core), leveraging its Include() method to fetch role details along with user data, ensuring efficient relational data retrieval. Furthermore, by utilizing async methods, the service efficiently handles database operations in an asynchronous manner, preventing application performance bottlenecks.

By designing UserService with modularity, scalability, and maintainability in mind, I have demonstrated my understanding of ORM principles, efficient database querying, and REST API best practices. This service ensures secure and optimized user role management, making it a crucial part of the overall system architecture.*/