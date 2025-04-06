namespace RunClubAPI.DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}


/* The TokenResponseDTO class is a Data Transfer Object (DTO) used in authentication. After a user successfully logs in, the system generates an Access Token and a Refresh Token, which are sent back to the client in this DTO.

The AccessToken is a JWT (JSON Web Token), which is used for authenticating API requests.
The RefreshToken allows users to obtain a new access token without having to log in again.
The Expiration property specifies when the access token expires, ensuring that clients can handle token renewal efficiently.
By using this DTO, the system ensures secure and structured authentication, improving security and performance in token-based authentication workflows. */