namespace RunClubAPI.DTOs
{
    public class ProgressRecordDTO
    {
        public int ProgressRecordId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string? CoachName { get; set; }       // ✅ Optional for frontend display

        public string ProgressDate { get; set; } = string.Empty;
        public string ProgressTime { get; set; } = string.Empty;
        public double DistanceCovered { get; set; }
        public string TimeTaken { get; set; } = string.Empty;
    }

}


/*
The ProgressRecordDTO class is a Data Transfer Object (DTO) designed to facilitate the structured exchange of progress data between the API and external clients. Each progress record is uniquely identified using ProgressRecordId, and it is associated with a specific user via UserId.

A key design choice in this DTO is separating the date and time components using ProgressDate and ProgressTime, both stored as strings. This approach helps avoid time zone inconsistencies that often arise with DateTime and serialization issues with TimeOnly.

Other properties, such as DistanceCovered and TimeTaken, provide useful metrics for tracking a user's progress over time. The ProgressDetails property allows users to store additional notes, such as workout intensity or personal observations.

By implementing this DTO, we ensure that only relevant progress data is transferred efficiently, enhancing API security, readability, and maintainability while keeping database entities separate from client-side representations.
*/