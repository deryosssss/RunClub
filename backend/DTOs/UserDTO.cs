namespace RunClubAPI.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; } = string.Empty; // ✅ change from int to string

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public RoleDTO? Role { get; set; }
    }

}


/* The UserDTO class is a Data Transfer Object (DTO) used to send user-related data between the API and the client. It ensures that only necessary and structured information is exchanged, improving performance and security.

UserId uniquely identifies each user.
Name represents the full name of the user.
Email is used for authentication and communication.
RoleId ensures that every user has a specific role in the system.
Role is an optional property that contains detailed role information if needed, helping in cases where additional role metadata is required.
Using UserDTO instead of exposing the entire User entity prevents sensitive data (like passwords) from being leaked and ensures better API design and maintainability. */
