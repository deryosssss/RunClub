using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public new string Email { get; set; } = string.Empty;  // Explicitly hiding base class property
        public string RoleId { get; set; }  // Ensure RoleId is int
        public Role? Role { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();
        public List<ProgressRecord> ProgressRecords { get; set; } = new();

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
