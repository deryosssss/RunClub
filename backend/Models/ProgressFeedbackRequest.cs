using System.ComponentModel.DataAnnotations;

namespace RunClubAPI.Models
{
    public class ProgressFeedbackRequest
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending"; // You could also use an enum

        public User? User { get; set; } // optional navigation
    }
}
