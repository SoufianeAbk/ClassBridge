using ClassBridge.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ClassBridge.Web.ViewModels
{
    public class RegisterParentViewModel
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
}