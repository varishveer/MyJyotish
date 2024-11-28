using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _28112024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Appointment",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Appointment",
                table: "JyotishRecords",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appointment",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "Appointment",
                table: "JyotishRecords");
        }
    }
}
