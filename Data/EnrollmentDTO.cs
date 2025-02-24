using RunClubAPI.Models;

namespace RunClub.DTOs
{
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
