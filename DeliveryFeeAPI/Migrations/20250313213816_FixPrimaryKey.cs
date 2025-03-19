using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryFeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryFeeRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleType = table.Column<string>(type: "TEXT", nullable: false),
                    BaseFee = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryFeeRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherObservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Station = table.Column<string>(type: "TEXT", nullable: true),
                    WMOCode = table.Column<string>(type: "TEXT", nullable: true),
                    AirTemperature = table.Column<double>(type: "REAL", nullable: false),
                    WindSpeed = table.Column<double>(type: "REAL", nullable: false),
                    WeatherPhenomenon = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherObservations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliveryFeeRules",
                columns: new[] { "Id", "BaseFee", "City", "VehicleType" },
                values: new object[,]
                {
                    { 1, 4.0, "Tallinn", "Car" },
                    { 2, 3.5, "Tallinn", "Scooter" },
                    { 3, 3.0, "Tallinn", "Bike" },
                    { 4, 3.5, "Tartu", "Car" },
                    { 5, 3.0, "Tartu", "Scooter" },
                    { 6, 2.5, "Tartu", "Bike" },
                    { 7, 3.0, "Pärnu", "Car" },
                    { 8, 2.5, "Pärnu", "Scooter" },
                    { 9, 2.0, "Pärnu", "Bike" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryFeeRules");

            migrationBuilder.DropTable(
                name: "WeatherObservations");
        }
    }
}
