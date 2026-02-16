using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FinanceTrackerApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;

namespace FinanceTrackerApp.Services
{
    public class AuthService
    {
        private const string SessionKey = "firebase_auth_session";
        private const string DemoEmail = "demo@finance.com";
        private const string DemoPassword = "1234";

        private readonly IHttpClientFactory _httpFactory;
        private readonly ProtectedLocalStorage _storage;
        private readonly FirebaseOptions _options;

        private FirebaseAuthSession? _cachedSession;

        public event Action? SessionChanged;

        public AuthService(
            IHttpClientFactory httpFactory,
            ProtectedLocalStorage storage,
            IOptions<FirebaseOptions> options)
        {
            _httpFactory = httpFactory;
            _storage = storage;
            _options = options.Value;
        }

        public async Task<bool> RegisterAsync(AppUser user)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                return false;

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_options.ApiKey}";
            var payload = new
            {
                email = user.Email,
                password = user.Password,
                returnSecureToken = true
            };

            var http = _httpFactory.CreateClient();
            var response = await http.PostAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
                return false;

            var auth = await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>();
            if (auth is null)
                return false;

            var session = FirebaseAuthSession.FromAuthResponse(auth);
            await SaveSessionAsync(session);
            NotifySessionChanged();
            return true;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (_options.AllowDemoLogin && IsDemoCredentials(email, password))
            {
                await SaveSessionAsync(CreateDemoSession());
                NotifySessionChanged();
                return true;
            }

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                return false;

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_options.ApiKey}";
            var payload = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var http = _httpFactory.CreateClient();
            var response = await http.PostAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
                return false;

            var auth = await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>();
            if (auth is null)
                return false;

            var session = FirebaseAuthSession.FromAuthResponse(auth);
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
            return session?.LocalId ?? "demo-user";
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
                var refreshed = await RefreshAsync(session.RefreshToken);
                if (refreshed is null)
                {
                    await LogoutAsync();
                    return null;
                }
                session = refreshed;
                await SaveSessionAsync(session);
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

        private async Task<FirebaseAuthSession?> RefreshAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                return null;

            var url = $"https://securetoken.googleapis.com/v1/token?key={_options.ApiKey}";
            var body = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var http = _httpFactory.CreateClient();
            var response = await http.PostAsync(url, new FormUrlEncodedContent(body));
            if (!response.IsSuccessStatusCode)
                return null;

            var refreshed = await response.Content.ReadFromJsonAsync<FirebaseRefreshResponse>();
            if (refreshed is null)
                return null;

            return FirebaseAuthSession.FromRefreshResponse(refreshed);
        }

        private void NotifySessionChanged() => SessionChanged?.Invoke();

        private static bool IsDemoCredentials(string email, string password)
        {
            return string.Equals(email?.Trim(), DemoEmail, StringComparison.OrdinalIgnoreCase) &&
                   password == DemoPassword;
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

        public static FirebaseAuthSession FromAuthResponse(FirebaseAuthResponse response)
        {
            var expiresInSeconds = int.TryParse(response.ExpiresIn, out var s) ? s : 3600;
            return new FirebaseAuthSession
            {
                IdToken = response.IdToken ?? "",
                RefreshToken = response.RefreshToken ?? "",
                LocalId = response.LocalId ?? "",
                Email = response.Email ?? "",
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds)
            };
        }

        public static FirebaseAuthSession FromRefreshResponse(FirebaseRefreshResponse response)
        {
            var expiresInSeconds = int.TryParse(response.ExpiresIn, out var s) ? s : 3600;
            return new FirebaseAuthSession
            {
                IdToken = response.IdToken ?? "",
                RefreshToken = response.RefreshToken ?? "",
                LocalId = response.UserId ?? "",
                Email = "",
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds)
            };
        }
    }

    public class FirebaseAuthResponse
    {
        [JsonPropertyName("idToken")]
        public string? IdToken { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public string? ExpiresIn { get; set; }

        [JsonPropertyName("localId")]
        public string? LocalId { get; set; }
    }

    public class FirebaseRefreshResponse
    {
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public string? ExpiresIn { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
}
