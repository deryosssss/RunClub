using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // Represents an enrollment record for users participating in events
    public class Enrollment
    {
        // Unique identifier for the enrollment record
        public int EnrollmentId { get; set; }

        // Stores the date when the user enrolled in an event
        // DateOnly ensures only the date is stored, without any time component
        public DateOnly EnrollmentDate { get; set; }  

        // Foreign key referencing the user who enrolled
        public int UserId { get; set; }

        // Foreign key referencing the event the user enrolled in
        public int EventId { get; set; }

        // Navigation property linking the enrollment to a specific user
        // The 'required' keyword ensures the User property is always set (C# 11 feature)
        public required User User { get; set; }

        // Navigation property linking the enrollment to a specific event
        // Ensures that every enrollment record is associated with a valid event
        public required Event Event { get; set; }
    }
}

/* The Enrollment.cs class represents a many-to-many relationship between users and events in the RunClub API. It acts as a join table that tracks which users have enrolled in which running events. The class contains an EnrollmentId, which serves as the primary key, and an EnrollmentDate, which records the date when a user signed up for an event. The UserId and EventId act as foreign keys, linking each enrollment record to a specific user and a specific event. The User and Event properties are navigation properties, allowing Entity Framework to automatically retrieve the associated user and event details. The required keyword ensures that these properties are mandatory, preventing enrollments from being created without a valid user and event. This class plays a critical role in managing event participation, ensuring that users can register for events while maintaining data integrity within the system. It is used in conjunction with Entity Framework Core to enforce relationships, optimize queries, and provide efficient database interactions. */