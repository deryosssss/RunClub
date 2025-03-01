using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    // Model for updating an existing role in the system
    public class UpdateRoleModel
    {
        // The unique identifier of the role to be updated (Primary Key)
        public string RoleId { get; set; }

        // The new name for the role after the update
        public string NewRoleName { get; set; }
    }
}

/* The UpdateRoleModel.cs class serves as a Data Transfer Object (DTO) for updating user roles in the RunClub API. It contains two properties: RoleId, which uniquely identifies the role to be modified, and NewRoleName, which stores the updated role name. This model is primarily used in role management features, allowing administrators to rename roles efficiently while maintaining their unique identifiers. This ensures structured role updates without affecting role assignments across users.*/