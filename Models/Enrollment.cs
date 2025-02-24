using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }  // Foreign Key
        public int EventId { get; set; } // Foreign Key
        
        public required User User { get; set; }  // Ensures it's always assigned
        public required Event Event { get; set; }
    }
}
