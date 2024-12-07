using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _07122024y : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryCodeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryCodeId",
                table: "Users",
                column: "CountryCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CountryCode_CountryCodeId",
                table: "Users",
                column: "CountryCodeId",
                principalTable: "CountryCode",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CountryCode_CountryCodeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountryCodeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryCodeId",
                table: "Users");
        }
    }
}
