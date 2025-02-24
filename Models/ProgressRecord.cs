using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class ProgressRecord
    {
        public int ProgressRecordId { get; set; }
        public int UserId { get; set; }  // Foreign Key

        public required string ProgressDetails { get; set; }
        public DateTime DateUpdated { get; set; }

        public required User User { get; set; } // Ensuring non-nullable navigation property
    }
}
