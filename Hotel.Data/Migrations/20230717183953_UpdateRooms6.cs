using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateRooms6 : Migration
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

            migrationBuilder.CreateTable(
                name: "FacilitiesRoom",
                columns: table => new
                {
                    FacilitiesIdFacility = table.Column<int>(type: "int", nullable: false),
                    RoomsIdRoom = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitiesRoom", x => new { x.FacilitiesIdFacility, x.RoomsIdRoom });
                    table.ForeignKey(
                        name: "FK_FacilitiesRoom_Facilities_FacilitiesIdFacility",
                        column: x => x.FacilitiesIdFacility,
                        principalTable: "Facilities",
                        principalColumn: "IdFacility",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilitiesRoom_Room_RoomsIdRoom",
                        column: x => x.RoomsIdRoom,
                        principalTable: "Room",
                        principalColumn: "IdRoom",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilitiesRoom_RoomsIdRoom",
                table: "FacilitiesRoom",
                column: "RoomsIdRoom");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitiesRoom");

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
