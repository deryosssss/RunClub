using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class Event
    {
        public int EventId { get; set; }
        
        public required string EventName { get; set; }
        public required string Description { get; set; }
        public DateTime EventDate { get; set; }
        public required string Location { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
