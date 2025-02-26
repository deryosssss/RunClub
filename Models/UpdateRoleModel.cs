using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class UpdateRoleModel
    {
        public string RoleId { get; set; }
        public string NewRoleName { get; set; }
    }
}
