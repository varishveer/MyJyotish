using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _091224y : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EId",
                table: "RedeemCodeRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCodeRequest_EId",
                table: "RedeemCodeRequest",
                column: "EId");

            migrationBuilder.AddForeignKey(
                name: "FK_RedeemCodeRequest_Employees_EId",
                table: "RedeemCodeRequest",
                column: "EId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedeemCodeRequest_Employees_EId",
                table: "RedeemCodeRequest");

            migrationBuilder.DropIndex(
                name: "IX_RedeemCodeRequest_EId",
                table: "RedeemCodeRequest");

            migrationBuilder.DropColumn(
                name: "EId",
                table: "RedeemCodeRequest");
        }
    }
}
