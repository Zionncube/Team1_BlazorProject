using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceTrackerApp.Services
{
    public class FirebaseAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public FirebaseAuthStateProvider(AuthService authService)
        {
            _authService = authService;
            _authService.SessionChanged += HandleSessionChanged;
        }

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
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, session.LocalId),
                new Claim(ClaimTypes.Email, session.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, session.Email ?? session.LocalId)
            }, "Firebase");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private void HandleSessionChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
