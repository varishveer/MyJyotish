using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _24102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProblemSolution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    Problem1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solution1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Problem2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solution2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Problem3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solution3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemSolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemSolution_JyotishRecords_JyotishId",
                        column: x => x.JyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemSolution_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_JyotishId",
                table: "ProblemSolution",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_UserId",
                table: "ProblemSolution",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemSolution");
        }
    }
}
