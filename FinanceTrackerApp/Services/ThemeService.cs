using Microsoft.JSInterop;

namespace FinanceTrackerApp.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode = true;

    public event Action? OnThemeChanged;

    public bool IsDarkMode => _isDarkMode;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        var storedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
        if (!string.IsNullOrEmpty(storedTheme))
        {
            _isDarkMode = storedTheme == "dark";
        }
        else
        {
            // Default to dark
            _isDarkMode = true;
        }
        await ApplyThemeAsync();
    }

    public async Task ToggleThemeAsync()
    {
        _isDarkMode = !_isDarkMode;
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _isDarkMode ? "dark" : "light");
        await ApplyThemeAsync();
        OnThemeChanged?.Invoke();
    }

    private async Task ApplyThemeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", _isDarkMode ? "dark" : "light");
    }
}
