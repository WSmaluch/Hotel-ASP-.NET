using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateEmployeeClass2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Employee");
        }
    }
}
