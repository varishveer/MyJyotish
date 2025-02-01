using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _01022025b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "jyotishId",
                table: "PurchaseAdvertisement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdvertisement_jyotishId",
                table: "PurchaseAdvertisement",
                column: "jyotishId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseAdvertisement_JyotishRecords_jyotishId",
                table: "PurchaseAdvertisement",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseAdvertisement_JyotishRecords_jyotishId",
                table: "PurchaseAdvertisement");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseAdvertisement_jyotishId",
                table: "PurchaseAdvertisement");

            migrationBuilder.DropColumn(
                name: "jyotishId",
                table: "PurchaseAdvertisement");
        }
    }
}
