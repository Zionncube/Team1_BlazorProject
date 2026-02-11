using FinanceTrackerApp.Components;
using FinanceTrackerApp.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<AuthService>();
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
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
