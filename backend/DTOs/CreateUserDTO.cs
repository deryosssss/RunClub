namespace RunClubAPI.DTOs
{
    public class CreateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = "Runner"; // Default role
        public string Password { get; set; } = string.Empty;
    }
}
