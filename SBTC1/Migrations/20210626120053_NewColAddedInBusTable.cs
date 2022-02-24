using Microsoft.EntityFrameworkCore.Migrations;

namespace SBTC1.Migrations
{
    public partial class NewColAddedInBusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalRateCounts",
                table: "Train",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRateCounts",
                table: "Train");
        }
    }
}
