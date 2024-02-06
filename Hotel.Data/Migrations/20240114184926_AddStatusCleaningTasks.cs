using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddStatusCleaningTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CleaningTask");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "CleaningTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_StatusId",
                table: "CleaningTask",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Status_StatusId",
                table: "CleaningTask",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Status_StatusId",
                table: "CleaningTask");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_StatusId",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "CleaningTask");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CleaningTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
