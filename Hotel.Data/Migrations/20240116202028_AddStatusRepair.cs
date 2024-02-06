using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddStatusRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "RepairTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairTask_StatusId",
                table: "RepairTask",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_Status_StatusId",
                table: "RepairTask",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_Status_StatusId",
                table: "RepairTask");

            migrationBuilder.DropIndex(
                name: "IX_RepairTask_StatusId",
                table: "RepairTask");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "RepairTask");
        }
    }
}
