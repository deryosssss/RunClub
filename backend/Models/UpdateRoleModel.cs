namespace RunClubAPI.Models
{
    public class UpdateRoleModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string NewRoleName { get; set; } = string.Empty;
    }
}


/* The UpdateRoleModel.cs class serves as a Data Transfer Object (DTO) for updating user roles in the RunClub API. It contains two properties: RoleId, which uniquely identifies the role to be modified, and NewRoleName, which stores the updated role name. This model is primarily used in role management features, allowing administrators to rename roles efficiently while maintaining their unique identifiers. This ensures structured role updates without affecting role assignments across users.*/