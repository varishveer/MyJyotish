using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _14112024cJyotishUserTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "JyotishUserAttachmentRecord",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "JyotishUserAttachmentRecord",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JyotishUserAttachmentRecord_AppointmentId",
                table: "JyotishUserAttachmentRecord",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JyotishUserAttachmentRecord_MemberId",
                table: "JyotishUserAttachmentRecord",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishUserAttachmentRecord_AppointmentRecords_AppointmentId",
                table: "JyotishUserAttachmentRecord",
                column: "AppointmentId",
                principalTable: "AppointmentRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishUserAttachmentRecord_ClientMembers_MemberId",
                table: "JyotishUserAttachmentRecord",
                column: "MemberId",
                principalTable: "ClientMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishUserAttachmentRecord_AppointmentRecords_AppointmentId",
                table: "JyotishUserAttachmentRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishUserAttachmentRecord_ClientMembers_MemberId",
                table: "JyotishUserAttachmentRecord");

            migrationBuilder.DropIndex(
                name: "IX_JyotishUserAttachmentRecord_AppointmentId",
                table: "JyotishUserAttachmentRecord");

            migrationBuilder.DropIndex(
                name: "IX_JyotishUserAttachmentRecord_MemberId",
                table: "JyotishUserAttachmentRecord");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "JyotishUserAttachmentRecord");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "JyotishUserAttachmentRecord");
        }
    }
}
