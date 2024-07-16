using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateRestaurantScheduleClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantSchedule_RestaurantCategory_CategoryId",
                table: "RestaurantSchedule");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantSchedule_CategoryId",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "RestaurantSchedule");

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "RestaurantSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "RestaurantSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RestaurantSchedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "RestaurantSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RestaurantSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemovedBy",
                table: "RestaurantSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedDate",
                table: "RestaurantSchedule",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "RemovedBy",
                table: "RestaurantSchedule");

            migrationBuilder.DropColumn(
                name: "RemovedDate",
                table: "RestaurantSchedule");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "RestaurantSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSchedule_CategoryId",
                table: "RestaurantSchedule",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantSchedule_RestaurantCategory_CategoryId",
                table: "RestaurantSchedule",
                column: "CategoryId",
                principalTable: "RestaurantCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
