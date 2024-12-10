using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _10122024y : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "Reviews",
                table: "PoojaRecord");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PoojaRecord",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Headline",
                table: "PoojaRecord",
                newName: "Image");

            migrationBuilder.AddColumn<int>(
                name: "PoojaType",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "jyotishId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "poojaId",
                table: "PoojaRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "PoojaRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BookedPoojaList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoojaId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedPoojaList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedPoojaList_PoojaRecord_PoojaId",
                        column: x => x.PoojaId,
                        principalTable: "PoojaRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedPoojaList_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_jyotishId",
                table: "PoojaRecord",
                column: "jyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_PoojaRecord_poojaId",
                table: "PoojaRecord",
                column: "poojaId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedPoojaList_PoojaId",
                table: "BookedPoojaList",
                column: "PoojaId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedPoojaList_userId",
                table: "BookedPoojaList",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_JyotishRecords_jyotishId",
                table: "PoojaRecord",
                column: "jyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PoojaRecord_PoojaList_poojaId",
                table: "PoojaRecord",
                column: "poojaId",
                principalTable: "PoojaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_JyotishRecords_jyotishId",
                table: "PoojaRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PoojaRecord_PoojaList_poojaId",
                table: "PoojaRecord");

            migrationBuilder.DropTable(
                name: "BookedPoojaList");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_jyotishId",
                table: "PoojaRecord");

            migrationBuilder.DropIndex(
                name: "IX_PoojaRecord_poojaId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "PoojaType",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "jyotishId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "poojaId",
                table: "PoojaRecord");

            migrationBuilder.DropColumn(
                name: "status",
                table: "PoojaRecord");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "PoojaRecord",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "PoojaRecord",
                newName: "Headline");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "PoojaRecord",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PoojaRecord",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reviews",
                table: "PoojaRecord",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
