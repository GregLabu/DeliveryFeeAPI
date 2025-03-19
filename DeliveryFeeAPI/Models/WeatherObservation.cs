public class WeatherObservation
{
    public int Id { get; set; } // Auto-incrementing primary key
    public string? Station { get; set; }
    public string? WMOCode { get; set; }
    public double AirTemperature { get; set; }
    public double WindSpeed { get; set; }
    public string? WeatherPhenomenon { get; set; }
    public DateTime Timestamp { get; set; }
}
