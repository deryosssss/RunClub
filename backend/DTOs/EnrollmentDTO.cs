using System;

namespace RunClubAPI.DTOs
{
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public DateOnly EnrollmentDate { get; set; }

        public string UserId { get; set; } = string.Empty; // ✅ Fixed: should be string (not int)

        public int EventId { get; set; }
    }
}



/* In my ASP.NET Core Web API, the EnrollmentDTO class is a Data Transfer Object (DTO) designed to streamline data exchange related to user enrollments in events.

Each enrollment is uniquely identified by an EnrollmentId. The EnrollmentDate field uses DateOnly instead of DateTime, which ensures that we only store the date without any time component. This is particularly useful because events are often scheduled based on calendar dates rather than specific times, and it helps prevent time zone inconsistencies.

The UserId property links the enrollment to a specific user, while the EventId associates it with a specific event. This structure allows for efficient querying, ensuring that the API can retrieve which users are enrolled in a given event or which events a user has signed up for.

Using DTOs like EnrollmentDTO helps maintain data integrity, API security, and separation of concerns, as it prevents direct manipulation of database models and ensures that only relevant data is exposed.*/