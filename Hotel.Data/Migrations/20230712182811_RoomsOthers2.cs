using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class RoomsOthers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Room_RoomId",
                table: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_RoomId",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Facilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_RoomId",
                table: "Facilities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Room_RoomId",
                table: "Facilities",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "IdRoom",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
