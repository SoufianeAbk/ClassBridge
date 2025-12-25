using ClassBridge.Core.Enums;

namespace ClassBridge.Web.ViewModels
{
    public class TeacherProfileViewModel
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
        public List<TeacherStrengthViewModel> Strengths { get; set; } = new List<TeacherStrengthViewModel>();
        public List<AvailabilityViewModel> Availabilities { get; set; } = new List<AvailabilityViewModel>();

        public string FullName => $"{FirstName} {LastName}";
    }

    public class AvailabilityViewModel
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
        public string DayName => DayOfWeek.ToString();
        public string TimeRange => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
}