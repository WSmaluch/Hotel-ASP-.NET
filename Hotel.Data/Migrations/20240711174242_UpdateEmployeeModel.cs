using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Data.Migrations
{
    public partial class UpdateEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Employee_EmployeeID",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Employee_EmployeeID",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Qualification_Employee_EmployeeID",
                table: "Qualification");

            migrationBuilder.DropForeignKey(
                name: "FK_Salary_Employee_EmployeeID",
                table: "Salary");

            migrationBuilder.DropIndex(
                name: "IX_Salary_EmployeeID",
                table: "Salary");

            migrationBuilder.DropIndex(
                name: "IX_Qualification_EmployeeID",
                table: "Qualification");

            migrationBuilder.DropIndex(
                name: "IX_Department_EmployeeID",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Contact_EmployeeID",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Contact");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QualificationId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalaryId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ContactId",
                table: "Employee",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_QualificationId",
                table: "Employee",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SalaryId",
                table: "Employee",
                column: "SalaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Contact_ContactId",
                table: "Employee",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department_DepartmentId",
                table: "Employee",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Qualification_QualificationId",
                table: "Employee",
                column: "QualificationId",
                principalTable: "Qualification",
                principalColumn: "QualificationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Salary_SalaryId",
                table: "Employee",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "SalaryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Contact_ContactId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Qualification_QualificationId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Salary_SalaryId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ContactId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_QualificationId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SalaryId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "QualificationId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SalaryId",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Salary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Qualification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeeID",
                table: "Salary",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_EmployeeID",
                table: "Qualification",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Department_EmployeeID",
                table: "Department",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_EmployeeID",
                table: "Contact",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Employee_EmployeeID",
                table: "Contact",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Employee_EmployeeID",
                table: "Department",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Qualification_Employee_EmployeeID",
                table: "Qualification",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salary_Employee_EmployeeID",
                table: "Salary",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
