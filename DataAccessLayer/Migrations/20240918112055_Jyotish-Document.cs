using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class JyotishDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_JId",
                table: "Documents",
                column: "JId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_JyotishRecords_JId",
                table: "Documents",
                column: "JId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_JyotishRecords_JId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_JId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "JId",
                table: "Documents");
        }
    }
}
