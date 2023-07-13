using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class RoomsOthers4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Room_TypeId",
                table: "Room",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Types_TypeId",
                table: "Room",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "IdType",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Types_TypeId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_TypeId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Room");
        }
    }
}
