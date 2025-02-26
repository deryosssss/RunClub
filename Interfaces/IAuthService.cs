using System.Threading.Tasks;
using RunClubAPI.DTOs;

namespace RunClubAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
        Task<AuthResponseDTO> AuthenticateUserAsync(string username, string password);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request); // Add this
        Task RevokeRefreshTokenAsync(string userId); // Add this
    }
}