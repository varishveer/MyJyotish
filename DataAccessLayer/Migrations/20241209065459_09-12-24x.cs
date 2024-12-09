using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _091224x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "appstatus",
                table: "RedeemCodeRequest");

            migrationBuilder.AddColumn<int>(
                name: "EId",
                table: "RedeamCode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "appstatus",
                table: "RedeamCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RedeamCode_EId",
                table: "RedeamCode",
                column: "EId");

            migrationBuilder.AddForeignKey(
                name: "FK_RedeamCode_Employees_EId",
                table: "RedeamCode",
                column: "EId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedeamCode_Employees_EId",
                table: "RedeamCode");

            migrationBuilder.DropIndex(
                name: "IX_RedeamCode_EId",
                table: "RedeamCode");

            migrationBuilder.DropColumn(
                name: "EId",
                table: "RedeamCode");

            migrationBuilder.DropColumn(
                name: "appstatus",
                table: "RedeamCode");

            migrationBuilder.AddColumn<int>(
                name: "EId",
                table: "RedeemCodeRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "appstatus",
                table: "RedeemCodeRequest",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
