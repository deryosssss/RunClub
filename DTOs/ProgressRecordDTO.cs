using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class ProgressRecordDTO
    {
    public int ProgressRecordId { get; set; }
    public int UserId { get; set; }
    public string ProgressDetails { get; set; }
    public string ProgressDate { get; set; }  // ✅ Separate date
    public string ProgressTime { get; set; }  // ✅ Separate time
    public double DistanceCovered { get; set; }
    public double TimeTaken { get; set; }
    }
}
