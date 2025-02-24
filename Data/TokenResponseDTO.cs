using RunClubAPI.Models;

namespace RunClub.DTOs
    {
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
