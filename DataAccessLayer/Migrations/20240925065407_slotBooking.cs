using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class slotBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotBooking_JyotishRecords_JyotishRecordsId",
                table: "SlotBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_JyotishRecords_JyotishModelId",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_Slots_JyotishModelId",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_SlotBooking_JyotishRecordsId",
                table: "SlotBooking");

            migrationBuilder.DropColumn(
                name: "JyotishModelId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "JyotishRecordsId",
                table: "SlotBooking");

            migrationBuilder.CreateIndex(
                name: "IX_SlotBooking_JyotishId",
                table: "SlotBooking",
                column: "JyotishId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotBooking_JyotishRecords_JyotishId",
                table: "SlotBooking",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotBooking_JyotishRecords_JyotishId",
                table: "SlotBooking");

            migrationBuilder.DropIndex(
                name: "IX_SlotBooking_JyotishId",
                table: "SlotBooking");

            migrationBuilder.AddColumn<int>(
                name: "JyotishModelId",
                table: "Slots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JyotishRecordsId",
                table: "SlotBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slots_JyotishModelId",
                table: "Slots",
                column: "JyotishModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotBooking_JyotishRecordsId",
                table: "SlotBooking",
                column: "JyotishRecordsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotBooking_JyotishRecords_JyotishRecordsId",
                table: "SlotBooking",
                column: "JyotishRecordsId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_JyotishRecords_JyotishModelId",
                table: "Slots",
                column: "JyotishModelId",
                principalTable: "JyotishRecords",
                principalColumn: "Id");
        }
    }
}
