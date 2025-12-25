using System.ComponentModel.DataAnnotations;

namespace ClassBridge.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}