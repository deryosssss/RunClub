using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // Represents a user's running progress record within the RunClub system
    public class ProgressRecord
    {
        public int ProgressRecordId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateOnly ProgressDate { get; set; }

        public TimeOnly ProgressTime { get; set; }

        public double DistanceCovered { get; set; }

        public TimeSpan TimeTaken { get; set; }

        public User? User { get; set; }
    }


}

/* The ProgressRecord.cs class is designed to store and manage user running progress records within the RunClub API. Each progress record has a unique ProgressRecordId serving as its primary key. The UserId is a foreign key, linking each record to a specific user. The ProgressDetails property stores a textual summary of the session, such as the running route or personal notes. The ProgressDate and ProgressTime properties ensure separate storage of date and time, improving data accuracy. The DistanceCovered and TimeTaken properties store performance-related metrics, helping users track their progress over time. The nullable User? navigation property allows flexibility in associating records with users. This class is crucial for implementing progress tracking features, enabling users to monitor their running activities, analyze their performance, and improve over time. */
