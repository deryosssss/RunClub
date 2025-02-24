using RunClubAPI.Models;

namespace RunClub.DTOs
{
    public class ProgressRecordDTO
    {
        public int ProgressRecordId { get; set; }
        public int UserId { get; set; }
        public string ProgressDetails { get; set; }
    }
}
