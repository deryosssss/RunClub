using RunClubAPI.Models; // Ensure correct model imports

namespace RunClubAPI.DTOs
{
    // Data Transfer Object (DTO) for roles
    // This class is used to transfer role-related data between API and client
    public class RoleDTO
    {
        public string RoleId { get; set; } // Unique identifier for the role

        public string RoleName { get; set; } // Human-readable role name (e.g., "Admin", "Coach", "Runner")

        public string RoleNormalizedName { get; set; } // Normalized name for case-insensitive role management
    }
}

/* The RoleDTO class is a Data Transfer Object (DTO) used to handle role-related data within the API. Instead of exposing database models directly, this DTO ensures structured and secure data exchange between the client and the API.

The RoleId property represents a unique identifier for each role, ensuring distinct roles within the system.
The RoleName provides a human-readable name, such as 'Admin', 'Coach', or 'Runner'.
The RoleNormalizedName helps in case-insensitive role management, ensuring that role lookups remain consistent across the database.
By using a DTO approach, we maintain a clean separation of concerns between database models and API responses. This ensures security, maintainability, and flexibility in how roles are assigned and managed. */
