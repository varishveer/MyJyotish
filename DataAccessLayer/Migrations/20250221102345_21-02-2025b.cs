using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _21022025b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInterviewFeedback_Employees_EmployeeId",
                table: "EmployeeInterviewFeedback");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeInterviewFeedback",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInterviewFeedback_Employees_EmployeeId",
                table: "EmployeeInterviewFeedback",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInterviewFeedback_Employees_EmployeeId",
                table: "EmployeeInterviewFeedback");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeInterviewFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInterviewFeedback_Employees_EmployeeId",
                table: "EmployeeInterviewFeedback",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
