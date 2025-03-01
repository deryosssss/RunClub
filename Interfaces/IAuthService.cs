using System.Threading.Tasks; //  Required for asynchronous operations
using RunClubAPI.DTOs; //  Importing Data Transfer Objects

namespace RunClubAPI.Interfaces
{
    // Interface for authentication-related operations
    public interface IAuthService
    {
        // Authenticates a user using username and password, returns an authentication response with a token
        Task<AuthResponseDTO> LoginAsync(string username, string password);

        // Registers a new user with a username and password, returns true if successful
        Task<bool> RegisterAsync(string username, string password);

        // Verifies user credentials and returns an authentication response with a token
        Task<AuthResponseDTO> AuthenticateUserAsync(string username, string password);

        // Handles token refreshing, allowing users to stay logged in without re-entering credentials
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request);

        // Revokes a refresh token to log out a user or handle token misuse
        Task RevokeRefreshTokenAsync(string userId);
    }
}

/* The IAuthService interface defines the authentication contract for the application, ensuring that all authentication-related operations are implemented consistently. It follows a structured approach for user authentication, registration, token management, and security.

LoginAsync: Validates user credentials and returns an authentication token.
RegisterAsync: Creates a new user account securely.
AuthenticateUserAsync: Another method for verifying user credentials before issuing a token.
RefreshTokenAsync: Allows a user to renew their authentication session without re-entering login details.
RevokeRefreshTokenAsync: Enhances security by allowing administrators to invalidate refresh tokens, forcing users to re-authenticate if needed.
By using interfaces, we ensure that multiple implementations (e.g., JWT authentication, OAuth, or custom authentication mechanisms) can be used interchangeably without modifying the core business logic. This promotes scalability, security, and maintainability in the authentication system. */