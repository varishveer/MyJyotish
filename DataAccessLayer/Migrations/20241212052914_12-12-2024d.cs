using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _12122024d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentBookmark",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    JyotishId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentBookmark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentBookmark_AppointmentRecords_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "AppointmentRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentBookmark_JyotishRecords_JyotishId",
                        column: x => x.JyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentBookmark_AppointmentId",
                table: "AppointmentBookmark",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentBookmark_JyotishId",
                table: "AppointmentBookmark",
                column: "JyotishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentBookmark");
        }
    }
}
