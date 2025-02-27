using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; } // ✅ Always required
        public RoleDTO? Role { get; set; } // ✅ Optional (only included when fetching users)
    }

}
