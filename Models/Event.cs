namespace RunClubAPI.Models
{
    // Represents a running event within the RunClub system
    public class Event
    {
        // Primary key: Unique identifier for each event
        public int EventId { get; set; }

        // Stores the event's name (e.g., "Marathon 2024")
        public string EventName { get; set; }

        // Provides additional details about the event (e.g., rules, distance)
        public string Description { get; set; }

        // Stores the event date (DateOnly ensures only the date is stored)
        public DateOnly EventDate { get; set; }

        // Stores the event time (TimeOnly ensures only the time is stored)
        public TimeOnly EventTime { get; set; }

        // Stores the location where the event will take place
        public string Location { get; set; }

        // Represents the list of users enrolled in the event
        // Uses ICollection for scalability, allowing multiple users to register
        // Nullable (?) ensures events can exist without enrollments initially
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}

/* The Event.cs class defines the structure and attributes of a running event in the RunClub API. Each event has an EventId, which serves as the primary key, and properties such as EventName, Description, EventDate, and EventTime, which store details about the event. The Location property specifies where the event will be held. The class also includes a navigation property, Enrollments, which establishes a one-to-many relationship with the Enrollment class, meaning multiple users can enroll in a single event. The nullable (?) collection ensures that an event can exist even if no users have enrolled yet. The use of DateOnly and TimeOnly types improves data integrity, ensuring that dates and times are stored separately without unnecessary components. This class is crucial for managing event details and participant registrations, enabling seamless integration with Entity Framework Core for database operations.*/
