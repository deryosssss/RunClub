using RunClubAPI.Models;
using RunClub.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RunClub.Services
{
    public class UserService : IUserService
    {
        private readonly RunClubContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(RunClubContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Fetch all users with pagination
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation($"Fetching users - Page: {pageNumber}, Size: {pageSize}");

            var users = await _context.Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!users.Any())
            {
                _logger.LogWarning("No users found.");
            }

            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                RoleId = u.RoleId
            }).ToList();
        }

        // ✅ Fetch user by ID
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching user with ID {id}");

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return null;
            }

            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId
            };
        }

        // ✅ Fetch users by Role ID
        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int roleId)
        {
            _logger.LogInformation($"Fetching users with Role ID {roleId}");

            var users = await _context.Users.AsNoTracking().Where(u => u.RoleId == roleId).ToListAsync();

            if (!users.Any())
            {
                _logger.LogWarning($"No users found with Role ID {roleId}.");
            }

            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                RoleId = u.RoleId
            }).ToList();
        }

        // ✅ Create a new user
        public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
        {
            _logger.LogInformation("Creating new user.");

            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                RoleId = userDto.RoleId
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                RoleId = newUser.RoleId
            };
        }

        // ✅ Update an existing user
        public async Task UpdateUserAsync(int id, UserDTO userDto)
        {
            _logger.LogInformation($"Updating user with ID {id}");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogError($"User with ID {id} not found.");
                return;
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.RoleId = userDto.RoleId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // ✅ Delete a user
        public async Task DeleteUserAsync(int id)
        {
            _logger.LogInformation($"Deleting user with ID {id}");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError($"User with ID {id} not found.");
                return;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}


