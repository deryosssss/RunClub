using RunClubAPI.Models;

namespace RunClubAPI.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; }
        public List<ProgressRecordDTO> ProgressRecords { get; set; }
    }

}
