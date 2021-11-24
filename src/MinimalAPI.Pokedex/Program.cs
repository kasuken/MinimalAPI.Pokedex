using Microsoft.OpenApi.Models;
using MinimalAPI.Pokedex;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "Pokedex API with .NET 6 Minimal API",
    Title = "Pokedex API",
    Version = "v1",
    Contact = new OpenApiContact()
    {
        Name = "Emanuele Bartolesi",
        Url = new Uri("https://github.com/kasuken")
    }
}));
builder.Services.AddCors(options => options.AddPolicy("AnyOrigin", o => o.AllowAnyOrigin()));
builder.Services.AddPokedexServices();

var app = builder.Build();

app.UseCors();
app.UseStaticFiles();
app.UseSwagger();
app.RegisterRoutes();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();