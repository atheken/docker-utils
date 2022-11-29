using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using static System.Text.Json.JsonSerializer;

namespace app.Processors;

public class RecipeImporter
{
    private static readonly Regex LinkDetector = new("(?<url>http(s)?://[^\"\\s'$>]+).*",
        RegexOptions.ExplicitCapture| RegexOptions.Singleline);
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private readonly ILogger _logger;
    private readonly string? _failedDir;

    public RecipeImporter(ILogger logger, HttpClient? client = null,
        string? baseUrl = null, string? apiKey = null)
    {
        
        _failedDir =  (Environment.GetEnvironmentVariable("APP_DATA_DIR") ?? Path.GetTempPath()) + "/failed";
        Directory.CreateDirectory(_failedDir);
        
        _baseUrl = baseUrl ?? Environment.GetEnvironmentVariable("MEALIE_BASE_URL");
        var mealieApiKey = apiKey ?? Environment.GetEnvironmentVariable("MEALIE_API_KEY");
        _client = client ?? new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mealieApiKey);
        _logger = logger;
    }
    
    public async Task<Status> ImportRecipe(InboundPayload payload)
    {
        try
        {
            // the email must be sent to recipes@..., and it must originate from a known address.
            if (payload.ToFull.Any(f=>f.Email?.ToLower().StartsWith("recipes@") ?? false) &&
                (payload.FromFull.Email?.Contains("theken") ?? false))
            {
                var detectedLink = LinkDetector.Matches(payload.TextBody ?? "").Select(k => k.Groups["url"].Value)
                    .FirstOrDefault();

                detectedLink ??= LinkDetector.Matches(payload.HtmlBody ?? "")
                    .Select(k => WebUtility.HtmlDecode(k.Groups["url"].Value)).FirstOrDefault();

                if (detectedLink != null)
                {
                    var result = await _client.PostAsJsonAsync($"{_baseUrl}/api/recipes/create-url",
                        new CreateRecipeRequest(detectedLink));
                    if (!result.IsSuccessStatusCode)
                    {
                        await using var file = File.OpenWrite($"{_failedDir}/{payload?.MessageId ?? Guid.NewGuid().ToString()}.json");
                        await SerializeAsync(file, payload);
                        result.EnsureSuccessStatusCode();
                    }
                    _logger.LogInformation($"Recipe imported from message: `{payload.MessageId}`!");
                }
            }
            else
            {
                await using var result = File.OpenWrite($"{_failedDir}/{payload?.MessageId ?? Guid.NewGuid().ToString()}.json");
                await SerializeAsync(result, payload);
                await result.FlushAsync();
                _logger.LogInformation(
                    "Dropping inbound message because it's not from an appropriate source, or to `recipes@`");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Unhandled error", ex);
            throw;
        }

        return new Status($"Processed message: `{payload.MessageId}`, thanks!");
    }
}