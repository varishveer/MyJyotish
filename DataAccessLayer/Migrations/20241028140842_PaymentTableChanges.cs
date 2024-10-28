using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserPaymentRecord",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "UserPaymentRecord",
                newName: "SignatureId");

            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "JyotishPaymentRecord",
                newName: "SignatureId");

            migrationBuilder.RenameColumn(
                name: "JyotishName",
                table: "JyotishPaymentRecord",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "UserPaymentRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "JyotishPaymentRecord",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "UserPaymentRecord");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "JyotishPaymentRecord");

            migrationBuilder.RenameColumn(
                name: "SignatureId",
                table: "UserPaymentRecord",
                newName: "Signature");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "UserPaymentRecord",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "SignatureId",
                table: "JyotishPaymentRecord",
                newName: "Signature");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "JyotishPaymentRecord",
                newName: "JyotishName");
        }
    }
}
