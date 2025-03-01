namespace RunClubAPI.Models
{
    // ✅ Represents a model for handling user authentication requests
    public class AuthModel
    {
        // ✅ Stores the user's email address 
        // This is used as a unique identifier for logging in.
        public string Email { get; set; }

        // ✅ Stores the user's password 
        // This will be validated against the stored (hashed) password in the database.
        public string Password { get; set; }
    }
}

/* The AuthModel.cs class is a data transfer object (DTO) used for handling user authentication within the RunClub API. It contains two properties: Email, which acts as the unique identifier for user login, and Password, which is entered by the user and validated against the stored (hashed) password in the database. This model is typically used in login API requests, where the user submits their credentials, and the system verifies them before issuing a JWT token for authentication. The AuthModel does not store any additional information like user roles or profile details, ensuring it remains lightweight and focused on authentication. It is commonly passed to an authentication service or controller that processes the login request, checks the credentials against the database, and, upon success, generates a secure token to maintain the user’s session. This model plays a key role in ensuring that only authorized users can access the system, enforcing security and access control. */