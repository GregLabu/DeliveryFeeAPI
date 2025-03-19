using DeliveryFeeAPI.Data;
using DeliveryFeeAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add database and services
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=weather.db"));
builder.Services.AddControllers();
builder.Services.AddHostedService<WeatherDataFetcher>(); // Background service for weather updates

// Adding Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery Fee API",
        Version = "v1",
        Description = "API for calculating delivery fees based on city, vehicle type, and weather conditions."
    });
});

var app = builder.Build();

// Enables Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery Fee API v1");
        c.RoutePrefix = ""; // Makes Swagger the default page so it is easy to test 
    });
}

app.MapControllers();
app.Run();
