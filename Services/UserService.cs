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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users...");

            try
            {
                var users = await _context.Users.Include(u => u.Role).ToListAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No users found in the database.");
                }

                return users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                throw; // Re-throw the exception to be handled by higher layers
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            try
            {
                var user = await _context.Users.Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", id);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID {UserId}", id);
                throw; // Re-throw the exception for higher-level handling
            }
        }
    }
}

