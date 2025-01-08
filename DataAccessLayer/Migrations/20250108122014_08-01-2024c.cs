using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _08012024c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPoojaModel_JyotishRecords_JyotishId",
                table: "JyotishPoojaModel");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPoojaModel_PoojaList_poojaType",
                table: "JyotishPoojaModel");

            migrationBuilder.DropForeignKey(
                name: "FK_jyotishSessions_JyotishRecords_jyotishId",
                table: "jyotishSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_jyotishSessions",
                table: "jyotishSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishPoojaModel",
                table: "JyotishPoojaModel");

            migrationBuilder.RenameTable(
                name: "jyotishSessions",
                newName: "JyotishSessions");

            migrationBuilder.RenameTable(
                name: "JyotishPoojaModel",
                newName: "JyotishPooja");

            migrationBuilder.RenameIndex(
                name: "IX_jyotishSessions_jyotishId",
                table: "JyotishSessions",
                newName: "IX_JyotishSessions_jyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPoojaModel_poojaType",
                table: "JyotishPooja",
                newName: "IX_JyotishPooja_poojaType");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPoojaModel_JyotishId",
                table: "JyotishPooja",
                newName: "IX_JyotishPooja_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishSessions",
                table: "JyotishSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishPooja",
                table: "JyotishPooja",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishPooja_JyotishRecords_JyotishId",
                table: "JyotishPooja",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishPooja_PoojaList_poojaType",
                table: "JyotishPooja",
                column: "poojaType",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishSessions_JyotishRecords_jyotishId",
                table: "JyotishSessions",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPooja_JyotishRecords_JyotishId",
                table: "JyotishPooja");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPooja_PoojaList_poojaType",
                table: "JyotishPooja");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishSessions_JyotishRecords_jyotishId",
                table: "JyotishSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishSessions",
                table: "JyotishSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishPooja",
                table: "JyotishPooja");

            migrationBuilder.RenameTable(
                name: "JyotishSessions",
                newName: "jyotishSessions");

            migrationBuilder.RenameTable(
                name: "JyotishPooja",
                newName: "JyotishPoojaModel");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishSessions_jyotishId",
                table: "jyotishSessions",
                newName: "IX_jyotishSessions_jyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPooja_poojaType",
                table: "JyotishPoojaModel",
                newName: "IX_JyotishPoojaModel_poojaType");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPooja_JyotishId",
                table: "JyotishPoojaModel",
                newName: "IX_JyotishPoojaModel_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jyotishSessions",
                table: "jyotishSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishPoojaModel",
                table: "JyotishPoojaModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishPoojaModel_JyotishRecords_JyotishId",
                table: "JyotishPoojaModel",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishPoojaModel_PoojaList_poojaType",
                table: "JyotishPoojaModel",
                column: "poojaType",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_jyotishSessions_JyotishRecords_jyotishId",
                table: "jyotishSessions",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
