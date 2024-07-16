using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddVoucherClass2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_VoucherPrice_VoucherPriceId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_VoucherType_VoucherTypeId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_VoucherPriceId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_VoucherTypeId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherPriceId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherTypeId",
                table: "Vouchers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherPriceId",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoucherTypeId",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_VoucherPriceId",
                table: "Vouchers",
                column: "VoucherPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_VoucherTypeId",
                table: "Vouchers",
                column: "VoucherTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherPrice_VoucherPriceId",
                table: "Vouchers",
                column: "VoucherPriceId",
                principalTable: "VoucherPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherType_VoucherTypeId",
                table: "Vouchers",
                column: "VoucherTypeId",
                principalTable: "VoucherType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
