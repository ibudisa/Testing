using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class MigrationV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "FeelsLikeTemperature",
                table: "WeatherForecasts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<byte>(
                name: "ForecastType",
                table: "WeatherForecasts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "Humidity",
                table: "WeatherForecasts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pressure",
                table: "WeatherForecasts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "WindSpeed",
                table: "WeatherForecasts",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeelsLikeTemperature",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "ForecastType",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "Pressure",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "WindSpeed",
                table: "WeatherForecasts");
        }
    }
}
