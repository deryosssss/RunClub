using System.ComponentModel.DataAnnotations;

namespace RunClubAPI.DTOs
{
    public class RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}




/*
The RegisterDTO class serves as a Data Transfer Object (DTO) designed to capture user registration details before they are processed by the API. This structure ensures data consistency and allows for input validation before storing user information in the database.

Each user provides their FirstName, LastName, and Email, where the email serves as a unique identifier for authentication. The Password and ConfirmPassword fields help enforce password validation, ensuring users do not mistakenly enter mismatched passwords.

Additionally, the Role property allows us to assign a specific user role (such as 'Admin', 'Coach', or 'Runner') at the time of registration, enabling role-based access control (RBAC).

By using a DTO instead of exposing direct database models, we enhance security, separation of concerns, and maintainability in the API. This also allows us to perform server-side validation before persisting data, reducing errors and improving user experience.
*/