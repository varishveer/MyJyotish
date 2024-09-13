using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class PJDocuments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_JyotishRecords_JyotishId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PendingJyotishRecords_JyotishId",
                table: "Documents",
                column: "JyotishId",
                principalTable: "PendingJyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PendingJyotishRecords_JyotishId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_JyotishRecords_JyotishId",
                table: "Documents",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
