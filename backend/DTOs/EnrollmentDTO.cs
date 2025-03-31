using System; 

namespace RunClubAPI.DTOs
{
    // Data Transfer Object (DTO) for user enrollments in events
    // This class is used to transfer enrollment-related data between the API and clients
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; } // Unique identifier for the enrollment record

        public DateOnly EnrollmentDate { get; set; }  // Stores only the date without time component, ensuring precision
        // This helps avoid inconsistencies caused by time zone differences.

        public int UserId { get; set; } // ID of the user who is enrolling in the event

        public int EventId { get; set; } // ID of the event that the user is enrolling in
    }
}

/* In my ASP.NET Core Web API, the EnrollmentDTO class is a Data Transfer Object (DTO) designed to streamline data exchange related to user enrollments in events.

Each enrollment is uniquely identified by an EnrollmentId. The EnrollmentDate field uses DateOnly instead of DateTime, which ensures that we only store the date without any time component. This is particularly useful because events are often scheduled based on calendar dates rather than specific times, and it helps prevent time zone inconsistencies.

The UserId property links the enrollment to a specific user, while the EventId associates it with a specific event. This structure allows for efficient querying, ensuring that the API can retrieve which users are enrolled in a given event or which events a user has signed up for.

Using DTOs like EnrollmentDTO helps maintain data integrity, API security, and separation of concerns, as it prevents direct manipulation of database models and ensures that only relevant data is exposed.*/