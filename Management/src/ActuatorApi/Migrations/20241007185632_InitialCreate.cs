using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steeltoe.Samples.ActuatorApi.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Forecasts",
            columns: table => new
            {
                Date = table.Column<DateOnly>(type: "date", nullable: false),
                TemperatureC = table.Column<int>(type: "int", nullable: false),
                Summary = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Forecasts", x => x.Date);
            })
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Forecasts");
    }
}
