using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _24102024b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "ProblemSolution",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_AppointmentId",
                table: "ProblemSolution",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_AppointmentRecords_AppointmentId",
                table: "ProblemSolution",
                column: "AppointmentId",
                principalTable: "AppointmentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_AppointmentRecords_AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_ProblemSolution_AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "ProblemSolution");
        }
    }
}
