using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class DocumentMessagesInTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressMessage",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProofMessage",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalCertificateMessage",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenthCertificateMessage",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwelveCertificateMessage",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressMessage",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IdProofMessage",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ProfessionalCertificateMessage",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TenthCertificateMessage",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TwelveCertificateMessage",
                table: "Documents");
        }
    }
}
