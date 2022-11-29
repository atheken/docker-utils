using System.Net;
using app;
using app.Processors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using static System.Text.Json.JsonSerializer;

namespace app_tests;

public class RecipeImporterTests : HttpMessageHandler
{
    private HttpRequestMessage? _sentRequest;

    [Theory]
    [InlineData("asdkdlksdkls<https://asdf.example.com>\n'dsdsadasasd")]
    [InlineData("asdkdlksdkls<\nhttps://asdf.example.com>\n'dsdsadasasd")]
    [InlineData("asdkdlksdkls<\nhttps://asdf.example.com >\n'dsdsadasasd")]
    [InlineData("asdkdlksdkls\"https://asdf.example.com >\n'dsdsadasasd")]
    [InlineData("asdkdlksdkls'https://asdf.example.com >\n'dsdsadasasd")]
    [InlineData("asdkdlksdkls https://asdf.example.com >\n'dsdsadasasd")]
    [InlineData("https://asdf.example.com")]
    public async Task ImporterParsesRecipeUrl(string content)
    {
        var url = "https://asdf.example.com";

        var settings = new ImporterSettings()
        {
            ApiToken = "TEST_TOKEN",
            Mealie = new MealieConfig("https://mealie.example.com", "ASDD:::SA:SA")
        };
        
        var importer = new RecipeImporter(new OptionsWrapper<ImporterSettings>(settings), 
            new Logger<RecipeImporter>(new NullLoggerFactory()), 
            new HttpClient(this));
        
        var payload = GetRequestPayload(content);
        await importer.ImportRecipe(payload);
        
        Assert.NotNull(_sentRequest);
        
        var authHeader = _sentRequest.Headers.Authorization;
        Assert.Equal(settings.Mealie.ApiKey, authHeader.Parameter);
        Assert.Equal("Bearer", authHeader.Scheme);
        Assert.Equal($"{settings.Mealie.BaseUrl}/api/recipes/create-url", _sentRequest.RequestUri.ToString());
        
        Assert.Equal("/api/recipes/create-url", _sentRequest.RequestUri.PathAndQuery);
        Assert.Equal(HttpMethod.Post, _sentRequest.Method);
        Assert.Equal(new CreateRecipeRequest(url), Deserialize<CreateRecipeRequest>(await _sentRequest.Content.ReadAsStringAsync()));

    }

    private InboundPayload GetRequestPayload(string content)
    {
        return new InboundPayload(new EmailAddress("atheken@example.com"), 
            new EmailAddress[]{new ("recipes@example.com") }, "Recipe Import", HtmlBody: content);
    }

    /// <summary>
    /// Handle record the outbound request, set the response code to OK.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage? request, CancellationToken cancellationToken)
    {
        _sentRequest = request;
        return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }
}