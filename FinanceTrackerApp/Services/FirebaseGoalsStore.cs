using System.Net.Http.Json;
using System.Text.Json;
using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

public class FirebaseGoalsStore : IGoalsStore
{
    private readonly HttpClient _http;
    private readonly FirebaseOptions _options;

    public FirebaseGoalsStore(HttpClient http, FirebaseOptions options)
    {
        _http = http;
        _options = options;
    }

    public async Task<List<SavingsGoal>> GetGoalsAsync(string userId, CancellationToken ct = default)
    {
        var url = BuildUrl($"goals/{userId}");
        var response = await _http.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return new List<SavingsGoal>();

        var json = await response.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrWhiteSpace(json) || json == "null")
            return new List<SavingsGoal>();

        var map = JsonSerializer.Deserialize<Dictionary<string, SavingsGoal>>(json) ?? new();
        foreach (var kvp in map)
        {
            if (Guid.TryParse(kvp.Key, out var id))
                kvp.Value.GoalId = id;
        }
        return map.Values.ToList();
    }

    public async Task<List<GoalContribution>> GetContributionsAsync(string userId, CancellationToken ct = default)
    {
        var url = BuildUrl($"contributions/{userId}");
        var response = await _http.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return new List<GoalContribution>();

        var json = await response.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrWhiteSpace(json) || json == "null")
            return new List<GoalContribution>();

        var map = JsonSerializer.Deserialize<Dictionary<string, GoalContribution>>(json) ?? new();
        foreach (var kvp in map)
        {
            if (Guid.TryParse(kvp.Key, out var id))
                kvp.Value.ContributionId = id;
        }
        return map.Values.ToList();
    }

    public async Task SaveGoalAsync(string userId, SavingsGoal goal, CancellationToken ct = default)
    {
        if (goal.GoalId == Guid.Empty)
            goal.GoalId = Guid.NewGuid();

        var url = BuildUrl($"goals/{userId}/{goal.GoalId}");
        await _http.PutAsJsonAsync(url, goal, ct);
    }

    public async Task DeleteGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        var url = BuildUrl($"goals/{userId}/{goalId}");
        await _http.DeleteAsync(url, ct);
    }

    public async Task AddContributionAsync(string userId, GoalContribution contribution, CancellationToken ct = default)
    {
        if (contribution.ContributionId == Guid.Empty)
            contribution.ContributionId = Guid.NewGuid();

        var url = BuildUrl($"contributions/{userId}/{contribution.ContributionId}");
        await _http.PutAsJsonAsync(url, contribution, ct);
    }

    public async Task DeleteContributionsForGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        var contributions = await GetContributionsAsync(userId, ct);
        var toDelete = contributions.Where(c => c.GoalId == goalId).ToList();
        foreach (var item in toDelete)
        {
            var url = BuildUrl($"contributions/{userId}/{item.ContributionId}");
            await _http.DeleteAsync(url, ct);
        }
    }

    private string BuildUrl(string path)
    {
        var baseUrl = _options.DatabaseUrl.TrimEnd('/');
        var url = $"{baseUrl}/{path}.json";
        if (!string.IsNullOrWhiteSpace(_options.AuthToken))
        {
            url += $"?auth={_options.AuthToken}";
        }
        return url;
    }
}
