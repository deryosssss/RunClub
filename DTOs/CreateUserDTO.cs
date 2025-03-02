namespace RunClubAPI.DTOs
{
    public class CreateUserDTO
    {
        public string Name { get; set; } // User's full name
        public string Email { get; set; } // User's email address
        public string RoleId { get; set; } = "1"; // Default to "runner"
        public string Password { get; set; }  // ✅ Add this line
    }
}
