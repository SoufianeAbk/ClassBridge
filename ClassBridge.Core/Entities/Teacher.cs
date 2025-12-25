using ClassBridge.Core.Enums;

namespace ClassBridge.Core.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public decimal HourlyRate { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsAvailableForMeetings { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<TeacherStrength> Strengths { get; set; } = new List<TeacherStrength>();
        public ICollection<TeacherAvailability> Availabilities { get; set; } = new List<TeacherAvailability>();
    }
}