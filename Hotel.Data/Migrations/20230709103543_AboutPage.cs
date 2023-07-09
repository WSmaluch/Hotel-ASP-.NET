using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class AboutPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutPage",
                columns: table => new
                {
                    IdAboutPage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content1Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content1Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content1Picture1Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content1Picture2Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content2Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content2Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content3Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content3Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_AboutPage", x => x.IdAboutPage);
                });

            migrationBuilder.CreateTable(
                name: "AboutSilderPhoto",
                columns: table => new
                {
                    IdAboutSilderPhoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_AboutSilderPhoto", x => x.IdAboutSilderPhoto);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPage");

            migrationBuilder.DropTable(
                name: "AboutSilderPhoto");
        }
    }
}
