namespace RunClubAPI.Models
{public class Event
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly EventDate { get; set; }
    public TimeOnly EventTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public int? CoachId { get; set; } // FK to Coach
    public Coach? Coach { get; set; } // Navigation property

    public ICollection<Enrollment>? Enrollments { get; set; }
}

}



/* The Event.cs class defines the structure and attributes of a running event in the RunClub API. Each event has an EventId, which serves as the primary key, and properties such as EventName, Description, EventDate, and EventTime, which store details about the event. The Location property specifies where the event will be held. The class also includes a navigation property, Enrollments, which establishes a one-to-many relationship with the Enrollment class, meaning multiple users can enroll in a single event. The nullable (?) collection ensures that an event can exist even if no users have enrolled yet. The use of DateOnly and TimeOnly types improves data integrity, ensuring that dates and times are stored separately without unnecessary components. This class is crucial for managing event details and participant registrations, enabling seamless integration with Entity Framework Core for database operations.*/
