using System.ComponentModel.DataAnnotations;

namespace RunClubAPI.Models
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
