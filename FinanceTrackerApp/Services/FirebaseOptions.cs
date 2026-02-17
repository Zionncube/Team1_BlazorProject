namespace FinanceTrackerApp.Services;

/// <summary>
/// Configuration values loaded from the Firebase section in appsettings.
/// </summary>
public class FirebaseOptions
{
    /// <summary>Firebase Web API key.</summary>
    public string ApiKey { get; set; } = "";
    /// <summary>Firebase auth domain.</summary>
    public string AuthDomain { get; set; } = "";
    /// <summary>Firebase project id.</summary>
    public string ProjectId { get; set; } = "";
    /// <summary>Firebase storage bucket.</summary>
    public string StorageBucket { get; set; } = "";
    /// <summary>Firebase messaging sender id.</summary>
    public string MessagingSenderId { get; set; } = "";
    /// <summary>Firebase app id.</summary>
    public string AppId { get; set; } = "";
    /// <summary>Realtime Database base URL.</summary>
    public string DatabaseUrl { get; set; } = "";
    /// <summary>Optional REST auth token for Firebase DB.</summary>
    public string AuthToken { get; set; } = "";
    /// <summary>Enables the demo credentials path in auth service.</summary>
    public bool AllowDemoLogin { get; set; } = false;
}
