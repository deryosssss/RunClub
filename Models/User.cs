using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class User : IdentityUser
    {
        // Custom fields
        public int UserId { get; set; } // Ensure this exists
        public string Name { get; set; } = string.Empty; // Fix nullability issue
        public override string Email { get; set; } = string.Empty; // Fix hiding inherited Email
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        // Related entities
        public List<Enrollment> Enrollments { get; set; } = new();
        public List<ProgressRecord> ProgressRecords { get; set; } = new();

        // Token Refresh fields
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}

