namespace FinanceTrackerApp.Services;

public class FirebaseOptions
{
    public string ApiKey { get; set; } = "";
    public string AuthDomain { get; set; } = "";
    public string ProjectId { get; set; } = "";
    public string StorageBucket { get; set; } = "";
    public string MessagingSenderId { get; set; } = "";
    public string AppId { get; set; } = "";
    public string DatabaseUrl { get; set; } = "";
    public string AuthToken { get; set; } = "";
    public bool AllowDemoLogin { get; set; } = false;
}
