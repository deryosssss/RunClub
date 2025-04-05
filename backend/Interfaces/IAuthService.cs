using RunClubAPI.DTOs;

namespace RunClubAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(RegisterDTO model);
    }
}


/* The IAuthService interface defines the authentication contract for the application, ensuring that all authentication-related operations are implemented consistently. It follows a structured approach for user authentication, registration, token management, and security.

LoginAsync: Validates user credentials and returns an authentication token.
RegisterAsync: Creates a new user account securely.
AuthenticateUserAsync: Another method for verifying user credentials before issuing a token.
RefreshTokenAsync: Allows a user to renew their authentication session without re-entering login details.
RevokeRefreshTokenAsync: Enhances security by allowing administrators to invalidate refresh tokens, forcing users to re-authenticate if needed.
By using interfaces, we ensure that multiple implementations (e.g., JWT authentication, OAuth, or custom authentication mechanisms) can be used interchangeably without modifying the core business logic. This promotes scalability, security, and maintainability in the authentication system. */