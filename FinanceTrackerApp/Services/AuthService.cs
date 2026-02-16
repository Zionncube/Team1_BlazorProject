using System.Security.Cryptography;
using FinanceTrackerApp.Models;
using FinanceTrackerApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;

namespace FinanceTrackerApp.Services
{
    public class AuthService
    {
        private const string SessionKey = "firebase_auth_session";
        private const string DemoEmail = "demo@finance.com";
        private const string DemoPassword = "1234";

        private readonly FinanceDbContext _db;
        private readonly ProtectedLocalStorage _storage;
        private readonly FirebaseOptions _options;

        private FirebaseAuthSession? _cachedSession;
        public string? LastError { get; private set; }

        public event Action? SessionChanged;

        public AuthService(
            FinanceDbContext db,
            ProtectedLocalStorage storage,
            IOptions<FirebaseOptions> options)
        {
            _db = db;
            _storage = storage;
            _options = options.Value;
        }

        public async Task<bool> RegisterAsync(AppUser user)
        {
            LastError = null;
            var email = NormalizeEmail(user.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                LastError = "Please enter a valid email.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 6)
            {
                LastError = "Password must be at least 6 characters.";
                return false;
            }

            var exists = await _db.Users.AnyAsync(u => u.Email == email);
            if (exists)
            {
                LastError = "This email is already registered.";
                return false;
            }

            var newUser = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = HashPassword(user.Password),
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            var session = new FirebaseAuthSession
            {
                IdToken = "local-session",
                RefreshToken = "local-refresh",
                LocalId = newUser.UserId,
                Email = newUser.Email,
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(30)
            };
            await SaveSessionAsync(session);
            NotifySessionChanged();
            return true;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            LastError = null;
            if (_options.AllowDemoLogin && IsDemoCredentials(email, password))
            {
                await SaveSessionAsync(CreateDemoSession());
                NotifySessionChanged();
                return true;
            }

            var normalizedEmail = NormalizeEmail(email);
            if (string.IsNullOrWhiteSpace(normalizedEmail))
            {
                LastError = "The email format is invalid.";
                return false;
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
            if (user is null || !VerifyPassword(password, user.PasswordHash))
            {
                LastError = "Invalid email or password.";
                return false;
            }

            var session = new FirebaseAuthSession
            {
                IdToken = "local-session",
                RefreshToken = "local-refresh",
                LocalId = user.UserId,
                Email = user.Email,
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(30)
            };
            await SaveSessionAsync(session);
            NotifySessionChanged();
            return true;
        }

        public async Task LogoutAsync()
        {
            _cachedSession = null;
            try
            {
                await _storage.DeleteAsync(SessionKey);
            }
            catch
            {
                // Ignore storage errors so logout does not crash the UI circuit.
            }
            NotifySessionChanged();
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var session = await GetSessionAsync();
            return session?.LocalId ?? "";
        }

        public async Task<FirebaseAuthSession?> GetSessionAsync()
        {
            if (_cachedSession != null &&
                _cachedSession.ExpiresAtUtc > DateTimeOffset.UtcNow.AddMinutes(1))
            {
                return _cachedSession;
            }

            ProtectedBrowserStorageResult<FirebaseAuthSession> stored;
            try
            {
                stored = await _storage.GetAsync<FirebaseAuthSession>(SessionKey);
            }
            catch
            {
                // If browser storage is unavailable, stay anonymous instead of failing the circuit.
                return null;
            }
            if (!stored.Success || stored.Value is null)
            {
                _cachedSession = null;
                return null;
            }

            var session = stored.Value;
            if (session.ExpiresAtUtc <= DateTimeOffset.UtcNow.AddMinutes(1))
            {
                await LogoutAsync();
                return null;
            }

            _cachedSession = session;
            return session;
        }

        private async Task SaveSessionAsync(FirebaseAuthSession session)
        {
            _cachedSession = session;
            try
            {
                await _storage.SetAsync(SessionKey, session);
            }
            catch
            {
                // Ignore storage errors; session stays cached for current circuit.
            }
        }

        private void NotifySessionChanged() => SessionChanged?.Invoke();

        private static bool IsDemoCredentials(string email, string password)
        {
            return string.Equals(email?.Trim(), DemoEmail, StringComparison.OrdinalIgnoreCase) &&
                   password == DemoPassword;
        }

        private static string NormalizeEmail(string? email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;
            var salt = Convert.FromBase64String(parts[0]);
            var expected = Convert.FromBase64String(parts[1]);
            var actual = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }

        private static FirebaseAuthSession CreateDemoSession()
        {
            return new FirebaseAuthSession
            {
                IdToken = "demo-token",
                RefreshToken = "demo-refresh",
                LocalId = "demo-user",
                Email = DemoEmail,
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(30)
            };
        }
    }

    public class FirebaseAuthSession
    {
        public string IdToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string LocalId { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTimeOffset ExpiresAtUtc { get; set; } = DateTimeOffset.UtcNow;
    }

}
