using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // Represents a user's running progress record within the RunClub system
    public class ProgressRecord
    {
        // Primary key: Unique identifier for each progress record entry
        public int ProgressRecordId { get; set; }

        // Foreign key: Links this record to a specific user
        public int UserId { get; set; }

        // Stores details of the user's progress (e.g., "Ran 5km at city park")
        public string ProgressDetails { get; set; }

        // Stores the date of the progress record (DateOnly ensures only date is stored)
        public DateOnly ProgressDate { get; set; }

        // Stores the time of the progress record (TimeOnly ensures only time is stored)
        public TimeOnly ProgressTime { get; set; }

        // Stores the distance covered during the session (measured in kilometers)
        public double DistanceCovered { get; set; }

        // Stores the time taken to complete the session (measured in minutes/hours)
        public double TimeTaken { get; set; }

        // Navigation property to associate the progress record with a user
        // Nullable (?) allows a record to exist without a user being explicitly linked
        public User? User { get; set; }
    }
}

/* The ProgressRecord.cs class is designed to store and manage user running progress records within the RunClub API. Each progress record has a unique ProgressRecordId serving as its primary key. The UserId is a foreign key, linking each record to a specific user. The ProgressDetails property stores a textual summary of the session, such as the running route or personal notes. The ProgressDate and ProgressTime properties ensure separate storage of date and time, improving data accuracy. The DistanceCovered and TimeTaken properties store performance-related metrics, helping users track their progress over time. The nullable User? navigation property allows flexibility in associating records with users. This class is crucial for implementing progress tracking features, enabling users to monitor their running activities, analyze their performance, and improve over time. */
