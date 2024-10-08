using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class JyotishPooja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaCategory_PoojaCategoryId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_PoojaCategoryId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "PoojaCategoryId",
                table: "PoojaRecord");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "PoojaRecord",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PoojaCategoryId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_PoojaCategoryId",
                table: "PoojaRecord",
                column: "PoojaCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_PoojaCategory_PoojaCategoryId",
                table: "PoojaRecord",
                column: "PoojaCategoryId",
                principalTable: "PoojaCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
