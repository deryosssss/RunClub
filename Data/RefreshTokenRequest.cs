using RunClubAPI.Models;

namespace RunClub.DTOs
{
    public class RefreshTokenRequest
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
//  (This API allows users to refresh their expired tokens instead of logging in again.)