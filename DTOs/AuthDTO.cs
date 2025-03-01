using RunClubAPI.Models; // Importing models for potential user-related references

namespace RunClubAPI.DTOs
{
    //  Data Transfer Object (DTO) for login requests.
    //  This class is used when a user attempts to log in.
    public class LoginDTO
    {
        public string Email { get; set; } // Stores the user's email address (used for authentication)
        public string Password { get; set; } // Stores the user's password (used for authentication)
    }

    //  DTO for sending authentication responses to the client.
    //  When a user successfully logs in, they receive a token to access protected resources.
    public class AuthResponseDTO
    {
        public string Token { get; set; } // Stores the JWT (JSON Web Token) used for authentication
        public string RefreshToken { get; set; } // Stores a refresh token for generating new access tokens
    }

    // DTO for handling refresh token requests.
    // If the access token expires, a user can request a new token using this object.
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } // Stores the refresh token used to generate a new access token
    }

    // DTO for revoking a user's refresh token.
    // Used when an admin or the user themselves wants to revoke access.
    public class RevokeTokenRequest
    {
        public string UserId { get; set; }  // Stores the ID of the user whose refresh token will be revoked
    }
}

/* In my ASP.NET Core Web API, DTOs (Data Transfer Objects) are used to facilitate secure and efficient communication between the client and server. The LoginDTO class is used when a user attempts to log in, containing the required Email and Password fields.

Upon successful authentication, the server responds with an AuthResponseDTO, which contains a JWT (JSON Web Token) and a Refresh Token. The JWT is used for stateless authentication, allowing users to access protected resources without storing session data on the server.

When a JWT expires, instead of forcing the user to log in again, they can send a RefreshTokenRequest, providing their refresh token to get a new access token. This enhances both security and user experience.

Additionally, if an admin or the user themselves wants to revoke access, they can send a RevokeTokenRequest, specifying the UserId. This prevents unauthorized access if, for example, a user logs out or their account is compromised.

By using DTOs, I ensure that the API only exposes necessary fields, reducing unnecessary data exposure and improving security, maintainability, and scalability of the authentication system. */