using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class ProgressRecord
    {
    public int ProgressRecordId { get; set; }
    public int UserId { get; set; }
    public string ProgressDetails { get; set; }
    public DateOnly ProgressDate { get; set; } // ✅ Store only date
    public TimeOnly ProgressTime { get; set; } // ✅ Store only time
    public double DistanceCovered { get; set; }
    public double TimeTaken { get; set; }

        // ✅ Allow User to be null by making it nullable
        public User? User { get; set; } 
    }

}
