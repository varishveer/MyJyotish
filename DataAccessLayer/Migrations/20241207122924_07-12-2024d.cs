using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _07122024d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPoojaCategory",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "PoojaList",
                table: "Sliders");

            migrationBuilder.AlterColumn<string>(
                name: "HomePage",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sliders");

            migrationBuilder.AlterColumn<string>(
                name: "HomePage",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BookPoojaCategory",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoojaList",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
