using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using DeliveryFeeAPI.Data;
using DeliveryFeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeAPI.Services;

public class WeatherDataFetcher : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly string _weatherApiUrl = "https://www.ilmateenistus.ee/ilma_andmed/xml/observations.php";

    public WeatherDataFetcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nextRun = now.Date.AddHours(now.Hour).AddMinutes(15); // Next HH:15

            if (now.Minute >= 15)
            {
                nextRun = nextRun.AddHours(1); // If it's past HH:15, schedule for next hour
            }

            TimeSpan delay = nextRun - now;
            Console.WriteLine($"Waiting until {nextRun} to fetch weather data...");

            await Task.Delay(delay, stoppingToken); //Wait until HH:15

            await FetchWeatherData();
        }
    }

    private async Task FetchWeatherData()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            Console.WriteLine("Fetching weather data...");
            var response = await _httpClient.GetStringAsync(_weatherApiUrl);
            var xmlDoc = XDocument.Parse(response);

            var stations = new[] { "Tallinn-Harku", "Tartu-Tõravere", "Pärnu" };
            var newEntries = new List<WeatherObservation>();

            foreach (var station in xmlDoc.Descendants("station"))
            {
                string name = station.Element("name")?.Value;
                if (!stations.Contains(name)) continue;

                var existingEntry = await dbContext.WeatherObservations
                    .FirstOrDefaultAsync(w => w.Station == name && w.Timestamp.Date == DateTime.UtcNow.Date);

                if (existingEntry != null)
                {
                    Console.WriteLine($"Skipping duplicate entry for {name}");
                    continue;
                }

                var observation = new WeatherObservation
                {
                    Station = name,
                    WMOCode = station.Element("wmocode")?.Value ?? "Unknown",
                    AirTemperature = double.TryParse(station.Element("airtemperature")?.Value, out var t) ? t : 0,
                    WindSpeed = double.TryParse(station.Element("windspeed")?.Value, out var w) ? w : 0,
                    WeatherPhenomenon = station.Element("phenomenon")?.Value ?? "Clear",
                    Timestamp = DateTime.UtcNow
                };

                newEntries.Add(observation);
            }

            if (newEntries.Count > 0)
            {
                await dbContext.WeatherObservations.AddRangeAsync(newEntries);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Weather data updated successfully!");
            }
            else
            {
                Console.WriteLine("No new weather data to insert.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching weather data: {ex.Message}");
        }
    }
}
