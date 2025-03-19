using DeliveryFeeAPI.Data;
using DeliveryFeeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeAPI.Controllers;

[Route("api/delivery-fee")]
[ApiController] 
public class DeliveryFeeController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public DeliveryFeeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    /// <summary>
    /// Calculates the delivery fee based on city, vehicle type, and latest weather conditions.
    /// </summary>
    /// <param name="city">City name: Tallinn, Tartu, or Pärnu</param>
    /// <param name="vehicleType">Vehicle type: Car, Scooter, or Bike</param>
    /// <returns>The total delivery fee or an error message</returns>
    public async Task<IActionResult> GetDeliveryFee([FromQuery] string city, [FromQuery] string vehicleType)

    {
        var latestWeather = await _dbContext.WeatherObservations
            .Where(w => w.Station.Contains(city))
            .OrderByDescending(w => w.Timestamp)
            .FirstOrDefaultAsync();

        var baseFee = await _dbContext.DeliveryFeeRules
            .FirstOrDefaultAsync(r => r.City == city && r.VehicleType == vehicleType);

        if (baseFee == null || latestWeather == null)
            return BadRequest("Invalid city or vehicle type");

        double totalFee = baseFee.BaseFee;

        if ((vehicleType == "Scooter" || vehicleType == "Bike") && latestWeather.AirTemperature < 0)
            totalFee += 0.5;

        if (vehicleType == "Bike" && latestWeather.WindSpeed > 20)
            return BadRequest("Usage of selected vehicle type is forbidden");

        return Ok(new { City = city, VehicleType = vehicleType, TotalFee = totalFee });
    }
}
