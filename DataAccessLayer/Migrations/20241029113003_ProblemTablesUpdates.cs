using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProblemTablesUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_AppointmentSolutionRecord_AppointmentSolutionId",
                table: "ProblemSolution");

            migrationBuilder.DropTable(
                name: "AppointmentSolutionRecord");

            migrationBuilder.RenameColumn(
                name: "AppointmentSolutionId",
                table: "ProblemSolution",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSolution_AppointmentSolutionId",
                table: "ProblemSolution",
                newName: "IX_ProblemSolution_AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRecords_JyotishId",
                table: "AppointmentRecords",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRecords_UserId",
                table: "AppointmentRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentRecords_JyotishRecords_JyotishId",
                table: "AppointmentRecords",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentRecords_Users_UserId",
                table: "AppointmentRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AppointmentRecords_JyotishRecords_JyotishId",
                table: "AppointmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentRecords_Users_UserId",
                table: "AppointmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_AppointmentRecords_AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentRecords_JyotishId",
                table: "AppointmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentRecords_UserId",
                table: "AppointmentRecords");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "ProblemSolution",
                newName: "AppointmentSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSolution_AppointmentId",
                table: "ProblemSolution",
                newName: "IX_ProblemSolution_AppointmentSolutionId");

            migrationBuilder.CreateTable(
                name: "AppointmentSolutionRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSolutionRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentSolutionRecord_AppointmentRecords_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "AppointmentRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentSolutionRecord_JyotishRecords_JyotishId",
                        column: x => x.JyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentSolutionRecord_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSolutionRecord_AppointmentId",
                table: "AppointmentSolutionRecord",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSolutionRecord_JyotishId",
                table: "AppointmentSolutionRecord",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSolutionRecord_UserId",
                table: "AppointmentSolutionRecord",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_AppointmentSolutionRecord_AppointmentSolutionId",
                table: "ProblemSolution",
                column: "AppointmentSolutionId",
                principalTable: "AppointmentSolutionRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
