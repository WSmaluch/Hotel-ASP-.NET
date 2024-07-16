using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateCulinrayEventClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CulinaryEvent_Menu_MenuId",
                table: "CulinaryEvent");

            migrationBuilder.DropIndex(
                name: "IX_CulinaryEvent_MenuId",
                table: "CulinaryEvent");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "CulinaryEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "CulinaryEvent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CulinaryEvent_MenuId",
                table: "CulinaryEvent",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CulinaryEvent_Menu_MenuId",
                table: "CulinaryEvent",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
