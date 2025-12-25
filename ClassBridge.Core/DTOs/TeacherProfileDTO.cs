using ClassBridge.Core.Enums;

namespace ClassBridge.Core.DTOs
{
    public class TeacherProfileDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public decimal HourlyRate { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsAvailableForMeetings { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<AvailabilityDTO> Availabilities { get; set; } = new List<AvailabilityDTO>();

        public string FullName => $"{FirstName} {LastName}";
    }

    public class AvailabilityDTO
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
    }
}