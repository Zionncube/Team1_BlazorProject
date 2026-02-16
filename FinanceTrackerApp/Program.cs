using FinanceTrackerApp.Components;
using ApexCharts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;
using FinanceTrackerApp.Data;
using FinanceTrackerApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
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

var dbFolder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "FinanceTrackerApp");
Directory.CreateDirectory(dbFolder);
var dbPath = Path.Combine(dbFolder, "FinanceTracker.db");
var legacyDbPath = Path.Combine(builder.Environment.ContentRootPath, "FinanceTracker.db");
if (!File.Exists(dbPath) && File.Exists(legacyDbPath))
{
    File.Copy(legacyDbPath, dbPath);
}

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.Configure<FirebaseOptions>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddHttpClient("firebase");
builder.Services.AddScoped<IGoalsStore, LocalGoalsStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS Users (
            UserId TEXT NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
            Email TEXT NOT NULL,
            PasswordHash TEXT NOT NULL,
            CreatedAt TEXT NOT NULL
        );
        """);
    db.Database.ExecuteSqlRaw("""
        CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Email ON Users (Email);
        """);
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS Categories (
            CategoryId TEXT NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
            UserId TEXT NOT NULL,
            Name TEXT NOT NULL,
            Color TEXT NOT NULL
        );
        """);
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS SavingsGoals (
            GoalId TEXT NOT NULL CONSTRAINT PK_SavingsGoals PRIMARY KEY,
            UserId TEXT NOT NULL,
            Title TEXT NOT NULL,
            Description TEXT NULL,
            TargetAmount TEXT NOT NULL,
            CurrentAmount TEXT NOT NULL,
            TargetDate TEXT NULL,
            IsCompleted INTEGER NOT NULL,
            Color TEXT NOT NULL,
            CreatedAt TEXT NOT NULL
        );
        """);
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS GoalContributions (
            ContributionId TEXT NOT NULL CONSTRAINT PK_GoalContributions PRIMARY KEY,
            GoalId TEXT NOT NULL,
            UserId TEXT NOT NULL,
            Amount TEXT NOT NULL,
            Date TEXT NOT NULL,
            Note TEXT NULL
        );
        """);
}

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
