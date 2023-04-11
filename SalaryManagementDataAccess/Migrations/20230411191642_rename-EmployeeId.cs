using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalaryManagementDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class renameEmployeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Empoyees_EmpoyeeId",
                table: "Salaries");

            migrationBuilder.RenameColumn(
                name: "EmpoyeeId",
                table: "Salaries",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Salaries_EmpoyeeId",
                table: "Salaries",
                newName: "IX_Salaries_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmpoyeeId",
                table: "Empoyees",
                newName: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Empoyees_EmployeeId",
                table: "Salaries",
                column: "EmployeeId",
                principalTable: "Empoyees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Empoyees_EmployeeId",
                table: "Salaries");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Salaries",
                newName: "EmpoyeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Salaries_EmployeeId",
                table: "Salaries",
                newName: "IX_Salaries_EmpoyeeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Empoyees",
                newName: "EmpoyeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Empoyees_EmpoyeeId",
                table: "Salaries",
                column: "EmpoyeeId",
                principalTable: "Empoyees",
                principalColumn: "EmpoyeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
