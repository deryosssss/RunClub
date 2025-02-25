using Org.BouncyCastle.Asn1.Cms;
using RunClubAPI.Models;

namespace RunClub.DTOs
{
    public class ProgressRecordDTO
    {
        public int ProgressRecordId { get; set; }
        public int UserId { get; set; }
        public string ProgressDetails { get; set; }
        public DateTime  ProgressDateTime { get; set; }
        public int DistanceCovered  { get; set; }
        public int TimeTaken { get; set; }
    }
}
