using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateDishClass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionInfo_Dish_DishId",
                table: "NutritionInfo");

            migrationBuilder.DropIndex(
                name: "IX_NutritionInfo_DishId",
                table: "NutritionInfo");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "NutritionInfo");

            migrationBuilder.AddColumn<int>(
                name: "NutritionInfoId",
                table: "Dish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_NutritionInfoId",
                table: "Dish",
                column: "NutritionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_NutritionInfo_NutritionInfoId",
                table: "Dish",
                column: "NutritionInfoId",
                principalTable: "NutritionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_NutritionInfo_NutritionInfoId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_NutritionInfoId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "NutritionInfoId",
                table: "Dish");

            migrationBuilder.AddColumn<int>(
                name: "DishId",
                table: "NutritionInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NutritionInfo_DishId",
                table: "NutritionInfo",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionInfo_Dish_DishId",
                table: "NutritionInfo",
                column: "DishId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
