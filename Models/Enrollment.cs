using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public DateOnly EnrollmentDate { get; set; }  // ✅ DateOnly ensures no time component
        public int UserId { get; set; }
        public int EventId { get; set; }

        public required User User { get; set; }
        public required Event Event { get; set; }
    }
}
