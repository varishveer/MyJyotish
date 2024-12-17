using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _17122024z : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JId",
                table: "ClientMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMembers_JId",
                table: "ClientMembers",
                column: "JId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMembers_JyotishRecords_JId",
                table: "ClientMembers",
                column: "JId",
                principalTable: "JyotishRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMembers_JyotishRecords_JId",
                table: "ClientMembers");

            migrationBuilder.DropIndex(
                name: "IX_ClientMembers_JId",
                table: "ClientMembers");

            migrationBuilder.DropColumn(
                name: "JId",
                table: "ClientMembers");
        }
    }
}
