using app;
using app.Processors;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

builder.Services
    .AddAuthentication(l => l.AddScheme("Basic", f => f.HandlerType = typeof(HeaderAuth)));

builder.Services.AddLogging();
builder.Services.AddAuthorization();
builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    var authScheme = "BasicAuth";
    
    opt.AddSecurityDefinition(authScheme, new()
    {
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Basic"
    });
    
    opt.AddSecurityRequirement( new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = authScheme}
            },
            new string[] { }
        }
    });
});

var app = builder.Build();
app.UseRequestDecompression();
app.UseResponseCompression();
app.Use((context, next) =>
{
    if (!context.Request.HasJsonContentType())
    {
        context.Request.Headers.ContentType = "application/json";
    }
    return next(context);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello!");

app.MapPost("/inbound", new RecipeImporter(app.Logger).ImportRecipe)
    .WithName("Default")
    .RequireAuthorization();

app.Run();