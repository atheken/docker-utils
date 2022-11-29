using System.Net;
using app;
using app.Processors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        var key = "asdf";
        var url = "https://asdf.example.com";

        var importer = new RecipeImporter(new Logger<RecipeImporter>(new NullLoggerFactory()), 
            new HttpClient(this), "https://example.com", key);
        
        var payload = GetRequestPayload(content);
        await importer.ImportRecipe(payload);
        
        Assert.NotNull(_sentRequest);
        
        var authHeader = _sentRequest.Headers.Authorization;
        Assert.Equal(key, authHeader.Parameter);
        Assert.Equal("Bearer", authHeader.Scheme);
        Assert.Equal("example.com", _sentRequest.RequestUri.Host);
        
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