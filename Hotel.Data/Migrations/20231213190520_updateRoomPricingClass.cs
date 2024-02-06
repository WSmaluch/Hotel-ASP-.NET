using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class updateRoomPricingClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BasePriceAdult",
                table: "RoomPricing",
                newName: "BasePriceChildren");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePriceAdult",
                table: "RoomPricing",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePriceAdult",
                table: "RoomPricing");

            migrationBuilder.RenameColumn(
                name: "BasePriceChildren",
                table: "RoomPricing",
                newName: "BasePriceAdult");
        }
    }
}
