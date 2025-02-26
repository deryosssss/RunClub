using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }

        public DateOnly EnrollmentDate { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
