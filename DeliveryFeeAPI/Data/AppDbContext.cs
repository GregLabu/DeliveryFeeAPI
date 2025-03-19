using DeliveryFeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<WeatherObservation> WeatherObservations { get; set; }
    public DbSet<DeliveryFeeRule> DeliveryFeeRules { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherObservation>()
        .Property(w => w.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<DeliveryFeeRule>().HasData(
            new DeliveryFeeRule { Id = 1, City = "Tallinn", VehicleType = "Car", BaseFee = 4.0 },
            new DeliveryFeeRule { Id = 2, City = "Tallinn", VehicleType = "Scooter", BaseFee = 3.5 },
            new DeliveryFeeRule { Id = 3, City = "Tallinn", VehicleType = "Bike", BaseFee = 3.0 },
            new DeliveryFeeRule { Id = 4, City = "Tartu", VehicleType = "Car", BaseFee = 3.5 },
            new DeliveryFeeRule { Id = 5, City = "Tartu", VehicleType = "Scooter", BaseFee = 3.0 },
            new DeliveryFeeRule { Id = 6, City = "Tartu", VehicleType = "Bike", BaseFee = 2.5 },
            new DeliveryFeeRule { Id = 7, City = "Pärnu", VehicleType = "Car", BaseFee = 3.0 },
            new DeliveryFeeRule { Id = 8, City = "Pärnu", VehicleType = "Scooter", BaseFee = 2.5 },
            new DeliveryFeeRule { Id = 9, City = "Pärnu", VehicleType = "Bike", BaseFee = 2.0 }
        );

    }
}
