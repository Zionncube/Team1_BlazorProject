namespace FinanceTrackerApp.Models
{
    public class AppUser
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; // for demo only (not secure)
    }
}
