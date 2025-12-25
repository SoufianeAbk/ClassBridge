using System.ComponentModel.DataAnnotations;

namespace ClassBridge.Core.DTOs
{
    public class CreateParentDTO
    {
        [Required(ErrorMessage = "Adres is verplicht")]
        [StringLength(200, ErrorMessage = "Adres mag maximaal 200 tekens bevatten")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stad is verplicht")]
        [StringLength(100, ErrorMessage = "Stad mag maximaal 100 tekens bevatten")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^\d{4}\s?[A-Z]{2}$", ErrorMessage = "Ongeldige postcode (gebruik formaat: 1000 AB)")]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [Phone(ErrorMessage = "Ongeldig telefoonnummer")]
        public string? EmergencyContactPhone { get; set; }
    }

    public class CreateChildDTO
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 tekens bevatten")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        [StringLength(50, ErrorMessage = "Achternaam mag maximaal 50 tekens bevatten")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Klas is verplicht")]
        [StringLength(50, ErrorMessage = "Klas mag maximaal 50 tekens bevatten")]
        public string Grade { get; set; } = string.Empty;

        [StringLength(200)]
        public string? SchoolName { get; set; }

        [StringLength(500)]
        public string? SpecialNeeds { get; set; }
    }
}