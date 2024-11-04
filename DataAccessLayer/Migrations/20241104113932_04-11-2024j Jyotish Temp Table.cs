using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04112024jJyotishTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "pincode",
                table: "jyotishTempRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pincode",
                table: "JyotishRecords",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pincode",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "pincode",
                table: "JyotishRecords");
        }
    }
}
