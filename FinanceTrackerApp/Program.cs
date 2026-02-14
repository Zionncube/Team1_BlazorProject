using FinanceTrackerApp.Components;
using ApexCharts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Options;
using FinanceTrackerApp.Data;
using FinanceTrackerApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, FirebaseAuthStateProvider>();
builder.Services.AddApexCharts();

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlite("Data Source=FinanceTracker.db"));
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<BudgetService>();
builder.Services.AddScoped<BudgetCalculationService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<ThemeService>();

builder.Services.Configure<FirebaseOptions>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddHttpClient("firebase");
builder.Services.AddSingleton<IGoalsStore>(sp =>
{
    var options = sp.GetRequiredService<IOptions<FirebaseOptions>>().Value;
    if (string.IsNullOrWhiteSpace(options.DatabaseUrl))
    {
        return new InMemoryGoalsStore();
    }

    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("firebase");
    return new FirebaseGoalsStore(http, options);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
