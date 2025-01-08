using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _08012024b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPooja_JyotishRecords_JyotishId",
                table: "JyotishPooja");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPooja_PoojaList_poojaType",
                table: "JyotishPooja");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishPooja",
                table: "JyotishPooja");

            migrationBuilder.RenameTable(
                name: "JyotishPooja",
                newName: "JyotishPoojaModel");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPooja_poojaType",
                table: "JyotishPoojaModel",
                newName: "IX_JyotishPoojaModel_poojaType");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPooja_JyotishId",
                table: "JyotishPoojaModel",
                newName: "IX_JyotishPoojaModel_JyotishId");

            migrationBuilder.AddColumn<bool>(
                name: "ActiveStatus",
                table: "JyotishRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishPoojaModel",
                table: "JyotishPoojaModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "jyotishSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    session_id = table.Column<int>(type: "int", nullable: false),
                    jyotishId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jyotishSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jyotishSessions_JyotishRecords_jyotishId",
                        column: x => x.jyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jyotishSessions_jyotishId",
                table: "jyotishSessions",
                column: "jyotishId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPoojaModel_JyotishRecords_JyotishId",
                table: "JyotishPoojaModel");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPoojaModel_PoojaList_poojaType",
                table: "JyotishPoojaModel");

            migrationBuilder.DropTable(
                name: "jyotishSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishPoojaModel",
                table: "JyotishPoojaModel");

            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "JyotishRecords");

            migrationBuilder.RenameTable(
                name: "JyotishPoojaModel",
                newName: "JyotishPooja");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPoojaModel_poojaType",
                table: "JyotishPooja",
                newName: "IX_JyotishPooja_poojaType");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishPoojaModel_JyotishId",
                table: "JyotishPooja",
                newName: "IX_JyotishPooja_JyotishId");

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
        }
    }
}
