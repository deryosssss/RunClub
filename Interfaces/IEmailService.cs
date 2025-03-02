using System.Threading.Tasks;

namespace RunClubAPI.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}
