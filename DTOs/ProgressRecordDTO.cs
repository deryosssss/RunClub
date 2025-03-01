using RunClubAPI.Models; // Ensure necessary model imports

namespace RunClubAPI.DTOs
{
    // Data Transfer Object (DTO) for user progress tracking
    // DTOs help in structuring and transmitting data between API and clients
    public class ProgressRecordDTO
    {
        public int ProgressRecordId { get; set; } // Unique identifier for the progress record

        public int UserId { get; set; } // ID of the user associated with this progress record

        public string ProgressDetails { get; set; } // Stores additional details about the progress (e.g., training notes)

        public string ProgressDate { get; set; } // Separate date field (avoids time zone issues with DateTime)

        public string ProgressTime { get; set; } // Separate time field (avoids serialization issues with TimeOnly)

        public double DistanceCovered { get; set; } // Distance the user covered during the session (in km/miles)

        public double TimeTaken { get; set; } // Total time taken to complete the activity (in minutes/hours)
    }
}

/*
The ProgressRecordDTO class is a Data Transfer Object (DTO) designed to facilitate the structured exchange of progress data between the API and external clients. Each progress record is uniquely identified using ProgressRecordId, and it is associated with a specific user via UserId.

A key design choice in this DTO is separating the date and time components using ProgressDate and ProgressTime, both stored as strings. This approach helps avoid time zone inconsistencies that often arise with DateTime and serialization issues with TimeOnly.

Other properties, such as DistanceCovered and TimeTaken, provide useful metrics for tracking a user's progress over time. The ProgressDetails property allows users to store additional notes, such as workout intensity or personal observations.

By implementing this DTO, we ensure that only relevant progress data is transferred efficiently, enhancing API security, readability, and maintainability while keeping database entities separate from client-side representations.
*/