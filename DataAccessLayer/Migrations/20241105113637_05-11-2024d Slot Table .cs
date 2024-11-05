using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _05112024dSlotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "SlotBooking");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Time",
                table: "Slots",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "SlotBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SlotBooking_SlotId",
                table: "SlotBooking",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotBooking_Slots_SlotId",
                table: "SlotBooking",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotBooking_Slots_SlotId",
                table: "SlotBooking");

            migrationBuilder.DropIndex(
                name: "IX_SlotBooking_SlotId",
                table: "SlotBooking");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "SlotBooking");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "Slots",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "SlotBooking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
