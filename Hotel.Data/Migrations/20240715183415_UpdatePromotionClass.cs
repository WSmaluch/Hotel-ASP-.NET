using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdatePromotionClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Promotion_PromotionId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_PromotionId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Dish");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Promotion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Promotion");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "Dish",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_PromotionId",
                table: "Dish",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Promotion_PromotionId",
                table: "Dish",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id");
        }
    }
}
