using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _12122024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_JyotishRecords_jyotishId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_jyotishId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "jyotishId",
                table: "PoojaRecord");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "jyotishId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_jyotishId",
                table: "PoojaRecord",
                column: "jyotishId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_JyotishRecords_jyotishId",
                table: "PoojaRecord",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
