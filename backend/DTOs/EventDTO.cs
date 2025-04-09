using RunClubAPI.Models;

public class EventDTO
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly EventDate { get; set; }
    public string EventTime { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int EnrollmentCount { get; set; }
    public string? ImageUrl { get; set; }

    // Coach fields (projected from navigation)
    public string? CoachName { get; set; }
    public string? CoachPhotoUrl { get; set; }
    public string? CoachBio { get; set; }
    public double? CoachRating { get; set; }

    public EventDTO() { }

    public EventDTO(Event e)
    {
        EventId = e.EventId;
        EventName = e.EventName;
        Description = e.Description;
        EventDate = e.EventDate;
        EventTime = e.EventTime.ToString("HH:mm:ss");
        Location = e.Location;
        EnrollmentCount = e.Enrollments?.Count ?? 0;
        ImageUrl = e.ImageUrl;

        // Safely extract coach info
        CoachName = e.Coach?.Name;
        CoachPhotoUrl = e.Coach?.PhotoUrl;
        CoachBio = e.Coach?.Bio;
        CoachRating = e.Coach?.Rating;
    }
}






/* 
The EventDTO class is a Data Transfer Object (DTO) that acts as a bridge between the API and external clients. It ensures that only relevant and structured data about events is exposed while keeping database models separate.

Each event is uniquely identified by the EventId, and key attributes such as EventName, Description, EventDate, and EventTime define the event’s details. A notable design choice is storing EventDate as DateOnly, which prevents time zone inconsistencies often caused by DateTime. Additionally, EventTime is stored as a string instead of TimeOnly to avoid serialization issues when sending data via JSON.

The constructor method allows for easy conversion from an Event entity to EventDTO, ensuring that all required fields are correctly mapped. The EnrollmentCount property dynamically calculates the number of users enrolled in the event while preventing null reference exceptions using the null-coalescing operator (?.Count ?? 0).

By using EventDTO, we ensure data encapsulation, API security, and optimal performance, as unnecessary database fields are not exposed to the client.
*/