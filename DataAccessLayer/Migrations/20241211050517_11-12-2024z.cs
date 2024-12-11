using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _11122024z : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaList_poojaId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_poojaId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "poojaId",
                table: "PoojaRecord");

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_PoojaType",
                table: "PoojaRecord",
                column: "PoojaType");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_PoojaList_PoojaType",
                table: "PoojaRecord",
                column: "PoojaType",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaList_PoojaType",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_PoojaType",
                table: "PoojaRecord");

            migrationBuilder.AddColumn<int>(
                name: "poojaId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_poojaId",
                table: "PoojaRecord",
                column: "poojaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_PoojaList_poojaId",
                table: "PoojaRecord",
                column: "poojaId",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
