using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    // Simple in-memory store - acts as our "database" while the app is running.
    // Registered as a Singleton in Program.cs so the same list is shared across all requests.
    public class UserStore
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        // Return a copy so callers can't modify the internal list directly
        public List<User> GetAll() => new List<User>(_users);

        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        // Check if any OTHER user already has this email
        public bool EmailExists(string email, int excludeId = 0)
            => _users.Any(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeId);

        public User Add(User user)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return user;
        }

        public void Remove(User user) => _users.Remove(user);
    }
}
