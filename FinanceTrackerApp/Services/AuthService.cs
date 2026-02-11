using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services
{
    public class AuthService
    {
        private AppUser? _currentUser;
        public AppUser? CurrentUser => _currentUser;
        public bool IsLoggedIn => _currentUser != null;
        public string CurrentUserId => _currentUser?.Email ?? "demo-user";
        // fake database (for demo)
        private readonly List<AppUser> _users = new()
        {
            new AppUser
            {
                FullName = "Demo User",
                Email = "demo@finance.com",
                Password = "1234"
            }
        };
        public bool Register(AppUser user)
        {
            if (_users.Any(u => u.Email.ToLower() == user.Email.ToLower()))
                return false;
            _users.Add(user);
            _currentUser = user;
            return true;
        }
        public bool Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.Password == password);
            if (user is null) return false;
            _currentUser = user;
            return true;
        }
        public void Logout()
        {
            _currentUser = null;
        }
    }
}
