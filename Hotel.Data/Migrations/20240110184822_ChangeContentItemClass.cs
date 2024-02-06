using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class ChangeContentItemClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "ContentItem",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContentItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContentItem");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ContentItem",
                newName: "Text");
        }
    }
}
