using RunClubAPI.Models; 

namespace RunClubAPI.DTOs
{
    // DTO for transferring user data between client and server
    public class UserDTO
    {
        public int UserId { get; set; } // Unique identifier for the user

        // [Required(ErrorMessage = "Name is required")]
        // [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; } // User's full name

        public string Email { get; set; } // User's email address (used for authentication)

        public string RoleId { get; set; } // Required - The ID of the role assigned to the user

        public RoleDTO? Role { get; set; } // Optional - Contains role details if included in response
    }
}

/* The UserDTO class is a Data Transfer Object (DTO) used to send user-related data between the API and the client. It ensures that only necessary and structured information is exchanged, improving performance and security.

UserId uniquely identifies each user.
Name represents the full name of the user.
Email is used for authentication and communication.
RoleId ensures that every user has a specific role in the system.
Role is an optional property that contains detailed role information if needed, helping in cases where additional role metadata is required.
Using UserDTO instead of exposing the entire User entity prevents sensitive data (like passwords) from being leaked and ensures better API design and maintainability. */
