using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddAttractionPriceClass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttractionPriceId",
                table: "Attraction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttractionPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionPrice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AttractionPriceId",
                table: "Attraction",
                column: "AttractionPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attraction_AttractionPrice_AttractionPriceId",
                table: "Attraction",
                column: "AttractionPriceId",
                principalTable: "AttractionPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attraction_AttractionPrice_AttractionPriceId",
                table: "Attraction");

            migrationBuilder.DropTable(
                name: "AttractionPrice");

            migrationBuilder.DropIndex(
                name: "IX_Attraction_AttractionPriceId",
                table: "Attraction");

            migrationBuilder.DropColumn(
                name: "AttractionPriceId",
                table: "Attraction");
        }
    }
}
