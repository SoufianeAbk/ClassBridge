namespace ClassBridge.Web.ViewModels
{
    public class ParentProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public List<ChildViewModel> Children { get; set; } = new List<ChildViewModel>();

        public string FullName => $"{FirstName} {LastName}";
    }

    public class ChildViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string? SchoolName { get; set; }
        public string? SpecialNeeds { get; set; }
        public int Age { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}