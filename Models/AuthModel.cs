using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RunClubAPI.Models
{
    public class AuthModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}