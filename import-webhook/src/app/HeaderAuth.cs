using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace app;

public class HeaderAuth : IAuthenticationHandler
{
    private static readonly string AuthToken = Environment.GetEnvironmentVariable("APP_API_TOKEN");

    static HeaderAuth()
    {
        if (AuthToken == null)
        {
            Environment.FailFast("APP_API_TOKEN is not set!");
        }
    }
    
    private static readonly ClaimsPrincipal User = 
        new (new GenericPrincipal(new GenericIdentity("api"), new[] {"api"}));
    
    private HttpContext _context;

    public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        _context = context;
        await Task.CompletedTask;
    }

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        await Task.CompletedTask;
        var key = GetKeyFromHeader(_context.Request.Headers["Authorization"].SingleOrDefault());
        if (key == AuthToken)
        {
            return AuthenticateResult.Success(new AuthenticationTicket(User, "http_auth"));
        }
        else
        {
            return AuthenticateResult.Fail("The provided token is not valid.");
        }
    }


    public async Task ChallengeAsync(AuthenticationProperties? properties)
    {
        await Task.CompletedTask;
        _context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        await JsonSerializer.SerializeAsync(_context.Response.BodyWriter.AsStream(),
            new {Message = "Please provide an authorization"});
    }

    public async Task ForbidAsync(AuthenticationProperties? properties)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Reads the password out of a basic auth header.
    /// </summary>
    /// <param name="header">The header from which to read the basic auth key.</param>
    /// <returns></returns>
    private string? GetKeyFromHeader(string? header)
    {
        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(header.Trim()[6..]))
                .Split(':').LastOrDefault();
        }
        catch
        {
            return null;
        }
    }
} 