using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Slot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressProofStatus",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProofStatus",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalCertificateStatus",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenthCertificateStatus",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwelveCertificateStatus",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SlotBooking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JyotishId = table.Column<int>(type: "int", nullable: false),
                    JyotishRecordsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotBooking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlotBooking_JyotishRecords_JyotishRecordsId",
                        column: x => x.JyotishRecordsId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JyotishModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slots_JyotishRecords_JyotishModelId",
                        column: x => x.JyotishModelId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SlotBooking_JyotishRecordsId",
                table: "SlotBooking",
                column: "JyotishRecordsId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_JyotishModelId",
                table: "Slots",
                column: "JyotishModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SlotBooking");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropColumn(
                name: "AddressProofStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IdProofStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ProfessionalCertificateStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TenthCertificateStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TwelveCertificateStatus",
                table: "Documents");
        }
    }
}
