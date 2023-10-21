using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class updateOptionsContentItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentItem_Options_OptionsIdOption",
                table: "ContentItem");

            migrationBuilder.DropIndex(
                name: "IX_ContentItem_OptionsIdOption",
                table: "ContentItem");

            migrationBuilder.DropColumn(
                name: "OptionsIdOption",
                table: "ContentItem");

            migrationBuilder.CreateTable(
                name: "ContentItemOptions",
                columns: table => new
                {
                    ContentItemsIdContentItem = table.Column<int>(type: "int", nullable: false),
                    OptionsIdOption = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentItemOptions", x => new { x.ContentItemsIdContentItem, x.OptionsIdOption });
                    table.ForeignKey(
                        name: "FK_ContentItemOptions_ContentItem_ContentItemsIdContentItem",
                        column: x => x.ContentItemsIdContentItem,
                        principalTable: "ContentItem",
                        principalColumn: "IdContentItem",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentItemOptions_Options_OptionsIdOption",
                        column: x => x.OptionsIdOption,
                        principalTable: "Options",
                        principalColumn: "IdOption",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentItemOptions_OptionsIdOption",
                table: "ContentItemOptions",
                column: "OptionsIdOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentItemOptions");

            migrationBuilder.AddColumn<int>(
                name: "OptionsIdOption",
                table: "ContentItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentItem_OptionsIdOption",
                table: "ContentItem",
                column: "OptionsIdOption");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentItem_Options_OptionsIdOption",
                table: "ContentItem",
                column: "OptionsIdOption",
                principalTable: "Options",
                principalColumn: "IdOption");
        }
    }
}
