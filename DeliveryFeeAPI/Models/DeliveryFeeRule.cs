namespace DeliveryFeeAPI.Models;

public class DeliveryFeeRule
{
    public int Id { get; set; }
    public string City { get; set; }
    public string VehicleType { get; set; }
    public double BaseFee { get; set; }
}
