using RunClubAPI.Models;

// Data Transfer Object (DTO) for event-related data
// DTOs are used to transfer data between the API and external clients
namespace RunClubAPI.DTOs
{
    public class EventDTO
    {
        public int EventId { get; set; } // Unique identifier for the event

        public string EventName { get; set; } // Name of the event (e.g., "Annual Marathon")

        public string Description { get; set; } // A short description of the event

        public DateOnly EventDate { get; set; } // Stores only the date component, preventing time zone issues

        public string EventTime { get; set; } // Stores the event time as a string to avoid TimeOnly serialization issues

        public string Location { get; set; } // The physical or virtual location of the event

        public int EnrollmentCount { get; set; } // The number of users enrolled in the event

        // Default constructor (needed for JSON serialization/deserialization)
        public EventDTO() { }

        // Constructor that maps an `Event` entity to `EventDTO`
        public EventDTO(Event eventEntity)
        {
            EventId = eventEntity.EventId; // Copy Event ID from the entity
            EventName = eventEntity.EventName; // Copy Event Name
            Description = eventEntity.Description; // Copy Description
            EventDate = eventEntity.EventDate; // Copy Event Date (DateOnly ensures precision)

            // Convert TimeOnly to string to avoid serialization issues
            EventTime = eventEntity.EventTime.ToString();

            Location = eventEntity.Location; // Copy Location

            // Get the number of enrollments while handling potential null values
            EnrollmentCount = eventEntity.Enrollments?.Count ?? 0;
        }
    }
}

/* 
The EventDTO class is a Data Transfer Object (DTO) that acts as a bridge between the API and external clients. It ensures that only relevant and structured data about events is exposed while keeping database models separate.

Each event is uniquely identified by the EventId, and key attributes such as EventName, Description, EventDate, and EventTime define the event’s details. A notable design choice is storing EventDate as DateOnly, which prevents time zone inconsistencies often caused by DateTime. Additionally, EventTime is stored as a string instead of TimeOnly to avoid serialization issues when sending data via JSON.

The constructor method allows for easy conversion from an Event entity to EventDTO, ensuring that all required fields are correctly mapped. The EnrollmentCount property dynamically calculates the number of users enrolled in the event while preventing null reference exceptions using the null-coalescing operator (?.Count ?? 0).

By using EventDTO, we ensure data encapsulation, API security, and optimal performance, as unnecessary database fields are not exposed to the client.
*/