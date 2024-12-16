using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _16122024z : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "jyotishId",
                table: "BookedPoojaList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookedPoojaList_jyotishId",
                table: "BookedPoojaList",
                column: "jyotishId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedPoojaList_JyotishRecords_jyotishId",
                table: "BookedPoojaList",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedPoojaList_JyotishRecords_jyotishId",
                table: "BookedPoojaList");

            migrationBuilder.DropIndex(
                name: "IX_BookedPoojaList_jyotishId",
                table: "BookedPoojaList");

            migrationBuilder.DropColumn(
                name: "jyotishId",
                table: "BookedPoojaList");
        }
    }
}
