using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}