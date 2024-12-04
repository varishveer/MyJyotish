using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04122024c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DepartmentPagesAccess_DepartmentId",
                table: "DepartmentPagesAccess",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentPagesAccess_PageId",
                table: "DepartmentPagesAccess",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentPagesAccess_Department_DepartmentId",
                table: "DepartmentPagesAccess",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentPagesAccess_EmployeeAccessPages_PageId",
                table: "DepartmentPagesAccess",
                column: "PageId",
                principalTable: "EmployeeAccessPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentPagesAccess_Department_DepartmentId",
                table: "DepartmentPagesAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentPagesAccess_EmployeeAccessPages_PageId",
                table: "DepartmentPagesAccess");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentPagesAccess_DepartmentId",
                table: "DepartmentPagesAccess");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentPagesAccess_PageId",
                table: "DepartmentPagesAccess");
        }
    }
}
