using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // User model that extends IdentityUser for authentication & authorization
    public class User : IdentityUser
    {
        // Unique identifier for the user (Primary Key)
        public int UserId { get; set; }

        // User's full name (default is empty to prevent null issues)
        public string Name { get; set; } = string.Empty;

        // Explicitly hiding the Email property from IdentityUser
        public new string Email { get; set; } = string.Empty;

        // Foreign Key to associate user with a specific role
        public string RoleId { get; set; }  

        // Navigation property to link User with Role entity
        public Role? Role { get; set; }

        // List of enrollments associated with the user (1-to-many relationship)
        public List<Enrollment> Enrollments { get; set; } = new();

        // List of progress records associated with the user (1-to-many relationship)
        public List<ProgressRecord> ProgressRecords { get; set; } = new();

        // Refresh token for authentication (used for issuing new access tokens)
        public string RefreshToken { get; set; } = string.Empty;

        // Expiry date of the refresh token (prevents unauthorized reuse)
        public DateTime RefreshTokenExpiry { get; set; }
    }
}

/* The User.cs class is an extension of ASP.NET Identity's IdentityUser class, customized for the RunClub API. It introduces additional properties beyond the default IdentityUser, such as UserId, Name, and RoleId, which help in managing user data within the application. The RoleId property establishes a foreign key relationship with the Role entity, ensuring that each user is assigned a role. Furthermore, the class maintains one-to-many relationships with both Enrollment and ProgressRecord, allowing users to participate in events and track their running progress. It also includes JWT refresh token properties, enabling secure token-based authentication by allowing users to request new tokens without frequent logins. This structure ensures scalability, security, and efficient user management within the RunClub system*/
