using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class PoojaListRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PoojaListId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_PoojaListId",
                table: "PoojaRecord",
                column: "PoojaListId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_PoojaList_PoojaListId",
                table: "PoojaRecord",
                column: "PoojaListId",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaList_PoojaListId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_PoojaListId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "PoojaListId",
                table: "PoojaRecord");
        }
    }
}
