using ClassBridge.Core.Enums;

namespace ClassBridge.Core.Entities
{
    public class TeacherStrength
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Subject Subject { get; set; }
        public int ProficiencyLevel { get; set; } // 1-5 schaal
        public string? Description { get; set; } // Optionele beschrijving van expertise
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Teacher Teacher { get; set; } = null!;
    }
}