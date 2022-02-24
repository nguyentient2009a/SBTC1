using Microsoft.EntityFrameworkCore.Migrations;

namespace SBTC1.Migrations
{
    public partial class AddedTrainVehicleNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainVehicleNumber",
                table: "Ticket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainVehicleNumber",
                table: "Ticket");
        }
    }
}
