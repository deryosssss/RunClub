namespace RunClubAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public DateOnly EnrollmentDate { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int EventId { get; set; }

        public User? User { get; set; }

        public Event? Event { get; set; }

        public bool IsCompleted { get; set; }
    }
}



/* The Enrollment.cs class represents a many-to-many relationship between users and events in the RunClub API. It acts as a join table that tracks which users have enrolled in which running events. The class contains an EnrollmentId, which serves as the primary key, and an EnrollmentDate, which records the date when a user signed up for an event. The UserId and EventId act as foreign keys, linking each enrollment record to a specific user and a specific event. The User and Event properties are navigation properties, allowing Entity Framework to automatically retrieve the associated user and event details. The required keyword ensures that these properties are mandatory, preventing enrollments from being created without a valid user and event. This class plays a critical role in managing event participation, ensuring that users can register for events while maintaining data integrity within the system. It is used in conjunction with Entity Framework Core to enforce relationships, optimize queries, and provide efficient database interactions. */