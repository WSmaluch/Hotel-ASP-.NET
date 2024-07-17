using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateDishClass2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_SeasonalMenu_SeasonalMenuId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_SeasonalMenuId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "SeasonalMenuId",
                table: "Dish");

            migrationBuilder.CreateTable(
                name: "DishSeasonalMenu",
                columns: table => new
                {
                    DishesId = table.Column<int>(type: "int", nullable: false),
                    SeasonalMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishSeasonalMenu", x => new { x.DishesId, x.SeasonalMenuId });
                    table.ForeignKey(
                        name: "FK_DishSeasonalMenu_Dish_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishSeasonalMenu_SeasonalMenu_SeasonalMenuId",
                        column: x => x.SeasonalMenuId,
                        principalTable: "SeasonalMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishSeasonalMenu_SeasonalMenuId",
                table: "DishSeasonalMenu",
                column: "SeasonalMenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishSeasonalMenu");

            migrationBuilder.AddColumn<int>(
                name: "SeasonalMenuId",
                table: "Dish",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_SeasonalMenuId",
                table: "Dish",
                column: "SeasonalMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_SeasonalMenu_SeasonalMenuId",
                table: "Dish",
                column: "SeasonalMenuId",
                principalTable: "SeasonalMenu",
                principalColumn: "Id");
        }
    }
}
