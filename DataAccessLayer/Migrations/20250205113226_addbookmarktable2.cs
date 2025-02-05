using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addbookmarktable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaBookMark_PoojaRecord_poojaId",
                table: "PoojaBookMark");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaBookMark_BookedPoojaList_poojaId",
                table: "PoojaBookMark",
                column: "poojaId",
                principalTable: "BookedPoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaBookMark_BookedPoojaList_poojaId",
                table: "PoojaBookMark");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaBookMark_PoojaRecord_poojaId",
                table: "PoojaBookMark",
                column: "poojaId",
                principalTable: "PoojaRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
