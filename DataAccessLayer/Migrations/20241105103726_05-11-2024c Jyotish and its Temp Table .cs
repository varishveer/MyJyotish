using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _05112024cJyotishanditsTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempRecordId",
                table: "JyotishRecords");

            migrationBuilder.RenameColumn(
                name: "pincode",
                table: "jyotishTempRecords",
                newName: "Pincode");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "JyotishId",
                table: "jyotishTempRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JyotishId",
                table: "jyotishTempRecords");

            migrationBuilder.RenameColumn(
                name: "Pincode",
                table: "jyotishTempRecords",
                newName: "pincode");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TempRecordId",
                table: "JyotishRecords",
                type: "int",
                nullable: true);
        }
    }
}
