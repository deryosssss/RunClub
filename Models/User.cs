using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RunClubAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }


        public List<Enrollment>? Enrollments { get; set; }
        public List<ProgressRecord>? ProgressRecords { get; set; }

            // ✅ New Fields for Token Refresh
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    }
}
