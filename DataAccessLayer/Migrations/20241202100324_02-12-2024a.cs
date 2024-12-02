using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _02122024a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "countryCode",
                table: "JyotishRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JyotishRecords_countryCode",
                table: "JyotishRecords",
                column: "countryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishRecords_CountryCode_countryCode",
                table: "JyotishRecords",
                column: "countryCode",
                principalTable: "CountryCode",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishRecords_CountryCode_countryCode",
                table: "JyotishRecords");

            migrationBuilder.DropIndex(
                name: "IX_JyotishRecords_countryCode",
                table: "JyotishRecords");

            migrationBuilder.DropColumn(
                name: "countryCode",
                table: "JyotishRecords");
        }
    }
}
