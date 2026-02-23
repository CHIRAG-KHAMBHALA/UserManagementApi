namespace UserManagementAPI.Models
{
    // Main entity that maps to the Users table in SQLite
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        // Email is unique - enforced at DB level via HasIndex in AppDbContext
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        // Auto-set when user is first created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
