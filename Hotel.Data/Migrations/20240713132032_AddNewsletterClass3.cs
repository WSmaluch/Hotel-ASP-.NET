using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddNewsletterClass3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Newsletter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemovedBy",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "RemovedBy",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "RemovedDate",
                table: "Newsletter");
        }
    }
}
