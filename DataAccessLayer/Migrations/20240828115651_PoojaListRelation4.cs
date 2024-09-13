using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class PoojaListRelation4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoojaList");

            migrationBuilder.RenameColumn(
                name: "PoojaListId",
                table: "PoojaRecord",
                newName: "PoojaCategoryId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaCategory_PoojaCategoryId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_PoojaCategoryId",
                table: "PoojaRecord");

            migrationBuilder.RenameColumn(
                name: "PoojaCategoryId",
                table: "PoojaRecord",
                newName: "PoojaListId");

            migrationBuilder.CreateTable(
                name: "PoojaList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoojaCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoojaList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoojaList_PoojaCategory_PoojaCategoryId",
                        column: x => x.PoojaCategoryId,
                        principalTable: "PoojaCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoojaList_PoojaCategoryId",
                table: "PoojaList",
                column: "PoojaCategoryId");
        }
    }
}
