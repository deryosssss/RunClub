namespace RunClubAPI.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly EventTime { get; set; } // ✅ Ensure this is correct
        public string Location { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; } // ✅ Ensure nullable
    }
}
