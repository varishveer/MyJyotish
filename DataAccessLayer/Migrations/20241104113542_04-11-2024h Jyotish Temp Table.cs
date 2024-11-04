using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04112024hJyotishTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentCharges",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "Pooja",
                table: "JyotishRecords");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "jyotishTempRecords",
                newName: "Image");

            migrationBuilder.AlterColumn<bool>(
                name: "Pooja",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AboutSection",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddressSection",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailbilitySection",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasicSection",
                table: "jyotishTempRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutSection",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "AddressSection",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "AvailbilitySection",
                table: "jyotishTempRecords");

            migrationBuilder.DropColumn(
                name: "BasicSection",
                table: "jyotishTempRecords");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "jyotishTempRecords",
                newName: "ProfileImageUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Pooja",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentCharges",
                table: "jyotishTempRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "jyotishTempRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pooja",
                table: "JyotishRecords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
