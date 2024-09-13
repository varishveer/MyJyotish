using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Poojacat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pooja",
                table: "Pooja");

            migrationBuilder.RenameTable(
                name: "Pooja",
                newName: "PoojaCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PoojaCategory",
                table: "PoojaCategory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PoojaCategory",
                table: "PoojaCategory");

            migrationBuilder.RenameTable(
                name: "PoojaCategory",
                newName: "Pooja");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pooja",
                table: "Pooja",
                column: "Id");
        }
    }
}
