using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } // Optional: If you want to confirm the password during registration
    }
}
