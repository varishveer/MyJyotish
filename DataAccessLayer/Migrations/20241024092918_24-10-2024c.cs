using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _24102024c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "AppointmentRecords");

            migrationBuilder.DropColumn(
                name: "TimeDuration",
                table: "AppointmentRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AppointmentRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "AppointmentRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppointmentRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Solution",
                table: "AppointmentRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "AppointmentRecords",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "TimeDuration",
                table: "AppointmentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
