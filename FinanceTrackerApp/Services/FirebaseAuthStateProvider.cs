using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FinanceTrackerApp.Services
{
    /// <summary>
    /// Represents a Firebase authentication session.
    /// </summary>
    public class FirebaseAuthSession
    {
        public string LocalId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string IdToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(1);
    }

    /// <summary>
    /// Service to manage Firebase authentication session state.
    /// </summary>
    public class AuthService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private FirebaseAuthSession? _session;

        public event Action? SessionChanged;

        public AuthService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        /// <summary>
        /// Sets and persists the current session.
        /// </summary>
        public async Task SetSessionAsync(FirebaseAuthSession session)
        {
            _session = session;
            await _localStorage.SetAsync("firebaseSession", session);
            SessionChanged?.Invoke();
        }

        /// <summary>
        /// Clears the current session.
        /// </summary>
        public async Task ClearSessionAsync()
        {
            _session = null;
            await _localStorage.DeleteAsync("firebaseSession");
            SessionChanged?.Invoke();
        }

        /// <summary>
        /// Retrieves the current session, restoring from storage if needed.
        /// </summary>
        public async Task<FirebaseAuthSession?> GetSessionAsync()
        {
            if (_session != null)
                return _session;

            var stored = await _localStorage.GetAsync<FirebaseAuthSession>("firebaseSession");
            _session = stored.Value;
            return _session;
        }
    }

    /// <summary>
    /// Converts the local Firebase session into an ASP.NET authentication state.
    /// </summary>
    public class FirebaseAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public FirebaseAuthStateProvider(AuthService authService)
        {
            _authService = authService;
            _authService.SessionChanged += HandleSessionChanged;
        }

        /// <summary>
        /// Builds the current ClaimsPrincipal from stored session information.
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            FirebaseAuthSession? session;
            try
            {
                session = await _authService.GetSessionAsync();
            }
            catch
            {
                session = null;
            }

            if (session is null || string.IsNullOrWhiteSpace(session.LocalId))
            {
                // Unauthenticated
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Create claims based on Firebase session
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, session.LocalId),
                new Claim(ClaimTypes.Email, session.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, session.Email ?? session.LocalId)
            };

            var identity = new ClaimsIdentity(claims, "Firebase");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Pushes auth state refresh to Blazor when session changes.
        /// </summary>
        private void HandleSessionChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
