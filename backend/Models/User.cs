using Microsoft.AspNetCore.Identity;

namespace RunClubAPI.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
    public ICollection<ProgressRecord> ProgressRecords { get; set; } = new HashSet<ProgressRecord>();

    // Token-based auth
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; } = DateTime.UtcNow;
}



/* The User.cs class is an extension of ASP.NET Identity's IdentityUser class, customized for the RunClub API. It introduces additional properties beyond the default IdentityUser, such as UserId, Name, and RoleId, which help in managing user data within the application. The RoleId property establishes a foreign key relationship with the Role entity, ensuring that each user is assigned a role. Furthermore, the class maintains one-to-many relationships with both Enrollment and ProgressRecord, allowing users to participate in events and track their running progress. It also includes JWT refresh token properties, enabling secure token-based authentication by allowing users to request new tokens without frequent logins. This structure ensures scalability, security, and efficient user management within the RunClub system*/
