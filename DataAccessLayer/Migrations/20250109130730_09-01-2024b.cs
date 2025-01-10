using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _09012024b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JyotishSessions");

            migrationBuilder.CreateTable(
                name: "JyotishChargeManagement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    Charge = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JyotishChargeManagement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JyotishChargeManagement_JyotishRecords_JyotishId",
                        column: x => x.JyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JyotishChargeManagement_JyotishId",
                table: "JyotishChargeManagement",
                column: "JyotishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JyotishChargeManagement");

            migrationBuilder.CreateTable(
                name: "JyotishSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jyotishId = table.Column<int>(type: "int", nullable: false),
                    session_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JyotishSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JyotishSessions_JyotishRecords_jyotishId",
                        column: x => x.jyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JyotishSessions_jyotishId",
                table: "JyotishSessions",
                column: "jyotishId");
        }
    }
}
