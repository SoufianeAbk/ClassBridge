namespace ClassBridge.Core.Entities
{
    public class Child
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Grade { get; set; } = string.Empty; // bijv. "1e jaar", "2e jaar"
        public string? SchoolName { get; set; }
        public string? SpecialNeeds { get; set; } // Eventuele speciale behoeften
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Parent Parent { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.UtcNow.Year - DateOfBirth.Year -
                         (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }
}