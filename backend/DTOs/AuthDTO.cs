namespace RunClubAPI.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

namespace RunClubAPI.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}


namespace RunClubAPI.DTOs
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}

namespace RunClubAPI.DTOs
{
    public class RevokeTokenRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
}



/* In my ASP.NET Core Web API, DTOs (Data Transfer Objects) are used to facilitate secure and efficient communication between the client and server. The LoginDTO class is used when a user attempts to log in, containing the required Email and Password fields.

Upon successful authentication, the server responds with an AuthResponseDTO, which contains a JWT (JSON Web Token) and a Refresh Token. The JWT is used for stateless authentication, allowing users to access protected resources without storing session data on the server.

When a JWT expires, instead of forcing the user to log in again, they can send a RefreshTokenRequest, providing their refresh token to get a new access token. This enhances both security and user experience.

Additionally, if an admin or the user themselves wants to revoke access, they can send a RevokeTokenRequest, specifying the UserId. This prevents unauthorized access if, for example, a user logs out or their account is compromised.

By using DTOs, I ensure that the API only exposes necessary fields, reducing unnecessary data exposure and improving security, maintainability, and scalability of the authentication system. */