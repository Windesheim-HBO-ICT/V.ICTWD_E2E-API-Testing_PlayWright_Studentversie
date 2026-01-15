using AspNetCore.Authentication.ApiKey;
using Microsoft.OpenApi;
using System.Reflection;
using WebApplication1.Authenticatie;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddSwaggerGen(config =>
{
    // Zorgen dat er in swagger een extra AI-key veld komt voor authenticated operations
    config.OperationFilter<CustomHeaderSwaggerAttribute>();
    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme) // Voor API key authenticatie
    .AddApiKeyInHeader<DatabaseApiKeyProvider>(
        options =>
        {
            options.Realm = "Windesheim Special Technologies";
            options.KeyName = "X-API-Key"; // Header name
        });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.MapGet("/", static async context =>
{
    context.Response.Redirect("./start.html");
});


app.Run();
