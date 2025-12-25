using ClassBridge.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassBridge.Core.DTOs
{
    public class CreateTeacherDTO
    {
        [Required(ErrorMessage = "Bio is verplicht")]
        [StringLength(1000, ErrorMessage = "Bio mag maximaal 1000 tekens bevatten")]
        public string Bio { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kwalificatie is verplicht")]
        [StringLength(200, ErrorMessage = "Kwalificatie mag maximaal 200 tekens bevatten")]
        public string Qualification { get; set; } = string.Empty;

        [Range(0, 50, ErrorMessage = "Jaren ervaring moet tussen 0 en 50 liggen")]
        public int YearsOfExperience { get; set; }

        [Range(0, 1000, ErrorMessage = "Uurtarief moet tussen €0 en €50 liggen")]
        public decimal HourlyRate { get; set; }

        public string? ProfileImageUrl { get; set; }

        public List<TeacherStrengthDTO> Strengths { get; set; } = new List<TeacherStrengthDTO>();
    }

    public class TeacherStrengthDTO
    {
        [Required]
        public Subject Subject { get; set; }

        [Range(1, 5, ErrorMessage = "Vaardigheid moet tussen 1 en 5 liggen")]
        public int ProficiencyLevel { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}