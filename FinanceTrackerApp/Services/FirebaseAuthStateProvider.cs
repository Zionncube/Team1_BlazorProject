using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FinanceTrackerApp.Services
{
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
