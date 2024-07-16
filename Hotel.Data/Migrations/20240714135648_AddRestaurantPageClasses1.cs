using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AddRestaurantPageClasses1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantPage",
                columns: table => new
                {
                    IdAboutPage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content1Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content1Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content1Picture1Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content1Picture2Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content2Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content2Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content3Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content3Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_RestaurantPage", x => x.IdAboutPage);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantPage");
        }
    }
}
