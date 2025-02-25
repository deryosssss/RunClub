using RunClubAPI.Models;

namespace RunClubAPI.DTOs
    {
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
