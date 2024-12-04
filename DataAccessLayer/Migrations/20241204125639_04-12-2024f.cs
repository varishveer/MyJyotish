using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04122024f : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "SlotBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "SlotBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "SlotBooking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LevelAccessValidation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    levels = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    pages = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelAccessValidation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelAccessValidation_Department_department",
                        column: x => x.department,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LevelAccessValidation_EmployeeAccessPages_pages",
                        column: x => x.pages,
                        principalTable: "EmployeeAccessPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LevelAccessValidation_Levels_levels",
                        column: x => x.levels,
                        principalTable: "Levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelAccessValidation_department",
                table: "LevelAccessValidation",
                column: "department");

            migrationBuilder.CreateIndex(
                name: "IX_LevelAccessValidation_levels",
                table: "LevelAccessValidation",
                column: "levels");

            migrationBuilder.CreateIndex(
                name: "IX_LevelAccessValidation_pages",
                table: "LevelAccessValidation",
                column: "pages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelAccessValidation");

            migrationBuilder.DropColumn(
                name: "endDate",
                table: "SlotBooking");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "SlotBooking");

            migrationBuilder.DropColumn(
                name: "status",
                table: "SlotBooking");
        }
    }
}
