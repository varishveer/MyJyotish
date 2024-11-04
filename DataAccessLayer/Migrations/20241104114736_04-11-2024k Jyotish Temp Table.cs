using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04112024kJyotishTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pincode",
                table: "JyotishRecords",
                newName: "Pincode");

            migrationBuilder.AddColumn<int>(
                name: "TempRecordId",
                table: "JyotishRecords",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempRecordId",
                table: "JyotishRecords");

            migrationBuilder.RenameColumn(
                name: "Pincode",
                table: "JyotishRecords",
                newName: "pincode");
        }
    }
}
