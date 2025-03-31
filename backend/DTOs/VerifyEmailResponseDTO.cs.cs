using RunClubAPI.Models; // Ensure correct model imports

namespace RunClubAPI.DTOs
{
    public class VerifyEmailResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
