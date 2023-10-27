using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class ChangesTypesRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotosURL",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "Room");

            migrationBuilder.AddColumn<int>(
                name: "MaxAmountOfPeople",
                table: "Types",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotosURL",
                table: "Types",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoomNumber",
                table: "Types",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Types",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FacilitiesTypes",
                columns: table => new
                {
                    FacilitiesIdFacility = table.Column<int>(type: "int", nullable: false),
                    TypesIdType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitiesTypes", x => new { x.FacilitiesIdFacility, x.TypesIdType });
                    table.ForeignKey(
                        name: "FK_FacilitiesTypes_Facilities_FacilitiesIdFacility",
                        column: x => x.FacilitiesIdFacility,
                        principalTable: "Facilities",
                        principalColumn: "IdFacility",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilitiesTypes_Types_TypesIdType",
                        column: x => x.TypesIdType,
                        principalTable: "Types",
                        principalColumn: "IdType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilitiesTypes_TypesIdType",
                table: "FacilitiesTypes",
                column: "TypesIdType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitiesTypes");

            migrationBuilder.DropColumn(
                name: "MaxAmountOfPeople",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "PhotosURL",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Types");

            migrationBuilder.AddColumn<string>(
                name: "PhotosURL",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Room",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RoomNumber",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
