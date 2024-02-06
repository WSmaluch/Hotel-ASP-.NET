using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddStatusReservationRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Room",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_StatusId",
                table: "Room",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_StatusId",
                table: "Reservations",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Status_StatusId",
                table: "Reservations",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Status_StatusId",
                table: "Room",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Status_StatusId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Status_StatusId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_StatusId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_StatusId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Reservations");
        }
    }
}
