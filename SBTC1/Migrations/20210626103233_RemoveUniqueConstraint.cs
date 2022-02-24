using Microsoft.EntityFrameworkCore.Migrations;

namespace SBTC1.Migrations
{
    public partial class RemoveUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seat_TrainRouteId",
                table: "Seat");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_TrainRouteId",
                table: "Seat",
                column: "TrainRouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seat_TrainRouteId",
                table: "Seat");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_TrainRouteId",
                table: "Seat",
                column: "TrainRouteId",
                unique: true);
        }
    }
}
