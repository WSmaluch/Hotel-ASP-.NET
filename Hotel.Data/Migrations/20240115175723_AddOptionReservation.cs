using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddOptionReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OptionId",
                table: "Reservations",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Options_OptionId",
                table: "Reservations",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "IdOption",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Options_OptionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_OptionId",
                table: "Reservations");
        }
    }
}
