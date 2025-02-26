using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class ProgressRecordDTO
    {
        public int ProgressRecordId { get; set; }
        public int UserId { get; set; }
        public string ProgressDetails { get; set; }
        public DateTime  ProgressDateTime { get; set; }
        public double DistanceCovered  { get; set; }
        public double TimeTaken { get; set; }
    }
}
