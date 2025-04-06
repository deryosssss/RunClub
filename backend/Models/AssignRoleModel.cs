namespace RunClubAPI.Models
{
    public class AssignRoleModel
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}


/* The AssignRoleModel.cs class is a simple data transfer object (DTO) used within the RunClub API to handle user role assignments. It contains two properties: UserId, which stores the unique identifier of a user (typically a GUID used in ASP.NET Identity), and RoleName, which represents the role being assigned (e.g., "Admin", "User"). This class is primarily used in API requests where administrators assign or modify user roles. The class does not interact directly with the database but is passed between the controller and service layers to facilitate structured data exchange. The API controller likely processes an HTTP POST request with an AssignRoleModel object in the request body, validating it before assigning the role using the ASP.NET Identity system. This approach ensures a clean and maintainable architecture by separating concerns—allowing the API to enforce role-based access control (RBAC) while keeping the model lightweight and reusable. */