using RunClubAPI.Models;

namespace RunClubAPI.DTOs
    {
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

        public class RevokeTokenRequest
    {
        public string UserId { get; set; }  // The ID of the user whose refresh token should be revoked
    }
}