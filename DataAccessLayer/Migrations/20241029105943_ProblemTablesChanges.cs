using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProblemTablesChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_AppointmentRecords_AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_JyotishRecords_JyotishId",
                table: "ProblemSolution");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_Users_UserId",
                table: "ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_ProblemSolution_AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_ProblemSolution_JyotishId",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Image1",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "JyotishId",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Problem1",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Problem2",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Problem3",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Solution1",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Solution2",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Solution3",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ProblemSolution");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProblemSolution",
                newName: "AppointmentSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSolution_UserId",
                table: "ProblemSolution",
                newName: "IX_ProblemSolution_AppointmentSolutionId");

            migrationBuilder.AddColumn<string>(
                name: "Problem",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Solution",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppointmentSolutionRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "JyotishUserAttachmentRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JyotishUserAttachmentRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JyotishUserAttachmentRecord_JyotishRecords_JyotishId",
                        column: x => x.JyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JyotishUserAttachmentRecord_Users_UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_JyotishUserAttachmentRecord_JyotishId",
                table: "JyotishUserAttachmentRecord",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_JyotishUserAttachmentRecord_UserId",
                table: "JyotishUserAttachmentRecord",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_AppointmentSolutionRecord_AppointmentSolutionId",
                table: "ProblemSolution",
                column: "AppointmentSolutionId",
                principalTable: "AppointmentSolutionRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_AppointmentSolutionRecord_AppointmentSolutionId",
                table: "ProblemSolution");

            migrationBuilder.DropTable(
                name: "AppointmentSolutionRecord");

            migrationBuilder.DropTable(
                name: "JyotishUserAttachmentRecord");

            migrationBuilder.DropColumn(
                name: "Problem",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "ProblemSolution");

            migrationBuilder.RenameColumn(
                name: "AppointmentSolutionId",
                table: "ProblemSolution",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSolution_AppointmentSolutionId",
                table: "ProblemSolution",
                newName: "IX_ProblemSolution_UserId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "ProblemSolution",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProblemSolution",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JyotishId",
                table: "ProblemSolution",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Problem1",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Problem2",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Problem3",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solution1",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solution2",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solution3",
                table: "ProblemSolution",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "ProblemSolution",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_AppointmentId",
                table: "ProblemSolution",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_JyotishId",
                table: "ProblemSolution",
                column: "JyotishId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_AppointmentRecords_AppointmentId",
                table: "ProblemSolution",
                column: "AppointmentId",
                principalTable: "AppointmentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_JyotishRecords_JyotishId",
                table: "ProblemSolution",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_Users_UserId",
                table: "ProblemSolution",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
