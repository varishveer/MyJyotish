using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _21102024b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "AppointmentRecords",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "AppointmentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "AppointmentRecords",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "AppointmentRecords");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "AppointmentRecords",
                newName: "DateTime");
        }
    }
}
