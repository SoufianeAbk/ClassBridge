namespace ClassBridge.Core.Entities
{
    public class TeacherAvailability
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; } = true; // Wekelijks terugkerend
        public DateTime? SpecificDate { get; set; } // Voor eenmalige beschikbaarheid
        public bool IsBooked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Teacher Teacher { get; set; } = null!;
    }
}