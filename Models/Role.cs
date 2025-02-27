using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; set; }
    }
}