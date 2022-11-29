using app;
using app.Processors;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(l => 
    l.AddScheme("Basic", f => f.HandlerType = typeof(HeaderAuth)));

builder.Services.AddLogging();
builder.Services.AddAuthorization();
builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();
builder.Services.AddSingleton<RecipeImporter>();
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
builder.Configuration.AddEnvironmentVariables("APP_");

// bind top-level options:
builder.Services.Configure<ImporterSettings>(builder.Configuration);

// builder.Services.AddExceptionHandler(k =>
// {
//     k.AllowStatusCode404Response = true;
// });

#region Swagger
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

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestDecompression();
app.UseResponseCompression();

app.MapGet("/", () => "Hello!");

app.MapPost("/inbound",app.Services.GetService<RecipeImporter>().ImportRecipe)
    .WithName("Default")
    .RequireAuthorization();

// app.UseExceptionHandler(c => c.Run(async context =>
// {
//     var ex = context.Features.Get<IExceptionHandlerPathFeature>().Error;
//     var response = new Status(ex.Message, 500);
//     context.Response.StatusCode = 500;
//     await context.Response.WriteAsJsonAsync(response);
// }));

app.Run();