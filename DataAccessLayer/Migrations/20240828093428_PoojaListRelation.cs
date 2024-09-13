using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class PoojaListRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PoojaList_PoojaCategoryId",
                table: "PoojaList");

            migrationBuilder.CreateIndex(
                name: "IX_PoojaList_PoojaCategoryId",
                table: "PoojaList",
                column: "PoojaCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PoojaList_PoojaCategoryId",
                table: "PoojaList");

            migrationBuilder.CreateIndex(
                name: "IX_PoojaList_PoojaCategoryId",
                table: "PoojaList",
                column: "PoojaCategoryId",
                unique: true);
        }
    }
}
