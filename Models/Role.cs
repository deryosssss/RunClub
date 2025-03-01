using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // Represents a role in the RunClub system (e.g., Admin, User, Coach)
    public class Role
    {
        // Unique identifier for the role (Primary Key)
        public string RoleId { get; set; }

        // Name of the role (e.g., "Admin", "User")
        public string RoleName { get; set; }

        // Normalized version of RoleName for case-insensitive comparisons (e.g., "ADMIN", "USER")
        public string RoleNormalizedName { get; set; }
    }
}

/* The Role.cs class defines user roles within the RunClub API, ensuring proper role-based access control (RBAC). Each role has a unique identifier, RoleId, which acts as the primary key. The RoleName property stores the actual role name (e.g., "Admin", "User", "Coach"), while RoleNormalizedName provides a standardized uppercase version for case-insensitive comparisons. This class is crucial for defining permissions within the system, ensuring that different user types have appropriate access to resources and functionalities.*/