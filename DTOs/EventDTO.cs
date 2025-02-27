using RunClubAPI.Models; // ✅ Ensure this is correct

public class EventDTO
{
    public int EventId { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateOnly EventDate { get; set; }
    public string EventTime { get; set; } // ✅ Ensure it's a string
    public string Location { get; set; }
    public int EnrollmentCount { get; set; }

    // ✅ Default constructor (needed for serialization)
    public EventDTO() { }

    // ✅ Constructor to map from Event entity
    public EventDTO(Event eventEntity)
    {
        EventId = eventEntity.EventId;
        EventName = eventEntity.EventName;
        Description = eventEntity.Description;
        EventDate = eventEntity.EventDate;
        EventTime = eventEntity.EventTime.ToString(); // ✅ Fix TimeOnly issue
        Location = eventEntity.Location;
        EnrollmentCount = eventEntity.Enrollments?.Count ?? 0; // Avoid null errors
    }
}
