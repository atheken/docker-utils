namespace app.Processors;

public class ImporterSettings
{
    public MealieConfig Mealie { get; set; }
    public string ApiToken { get; set; }
    public string DataDir { get; set; } = Path.GetTempPath();
}

public record MealieConfig(string BaseUrl, string ApiKey);