using ClassBridge.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassBridge.Web.ViewModels
{
    public class RegisterTeacherViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 tekens bevatten")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        [StringLength(50, ErrorMessage = "Achternaam mag maximaal 50 tekens bevatten")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefoonnummer is verplicht")]
        [Phone(ErrorMessage = "Ongeldig telefoonnummer")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Wachtwoord moet minimaal 6 tekens bevatten")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoordbevestiging is verplicht")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bio is verplicht")]
        [StringLength(1000, ErrorMessage = "Bio mag maximaal 1000 tekens bevatten")]
        public string Bio { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kwalificatie is verplicht")]
        [StringLength(200, ErrorMessage = "Kwalificatie mag maximaal 200 tekens bevatten")]
        public string Qualification { get; set; } = string.Empty;

        [Range(0, 50, ErrorMessage = "Jaren ervaring moet tussen 0 en 50 liggen")]
        public int YearsOfExperience { get; set; }

        [Range(0, 1000, ErrorMessage = "Uurtarief moet tussen €0 en €1000 liggen")]
        public decimal HourlyRate { get; set; }

        public string? ProfileImageUrl { get; set; }

        public List<TeacherStrengthViewModel> Strengths { get; set; } = new List<TeacherStrengthViewModel>();
    }

    public class TeacherStrengthViewModel
    {
        [Required]
        public Subject Subject { get; set; }

        [Range(1, 5, ErrorMessage = "Vaardigheid moet tussen 1 en 5 liggen")]
        public int ProficiencyLevel { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}