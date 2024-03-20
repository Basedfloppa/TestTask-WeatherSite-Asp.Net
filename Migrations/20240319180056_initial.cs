using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask_DS.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weather",
                columns: table => new
                {
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<DateTimeOffset>(type: "time with time zone", nullable: false),
                    temperature = table.Column<sbyte>(type: "smallint", nullable: true),
                    air_moisture = table.Column<byte>(type: "smallint", nullable: true),
                    dew_point = table.Column<sbyte>(type: "smallint", nullable: true),
                    pressure = table.Column<ushort>(type: "integer", nullable: true),
                    air_direction = table.Column<List<string>>(type: "varchar(10)", nullable: true),
                    air_speed = table.Column<byte>(type: "smallint", nullable: true),
                    cloudiness = table.Column<byte>(type: "smallint", nullable: true),
                    lower_cloudiness_treshold = table.Column<ushort>(type: "integer", nullable: true),
                    horisontal_visibility = table.Column<byte>(type: "smallint", nullable: true),
                    weather_condition = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Weather_Pk", x => new { x.date, x.time });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather");
        }
    }
}
