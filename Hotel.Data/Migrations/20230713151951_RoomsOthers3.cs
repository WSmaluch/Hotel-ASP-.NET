using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class RoomsOthers3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameType",
                table: "Types",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Types",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotosURL",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "PhotosURL",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Types",
                newName: "NameType");
        }
    }
}
