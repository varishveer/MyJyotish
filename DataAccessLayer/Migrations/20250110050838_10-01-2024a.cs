using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _10012024a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishChargeManagement_JyotishRecords_JyotishId",
                table: "JyotishChargeManagement");

            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "JyotishChargeManagement",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishChargeManagement_JyotishRecords_JyotishId",
                table: "JyotishChargeManagement",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishChargeManagement_JyotishRecords_JyotishId",
                table: "JyotishChargeManagement");

            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "JyotishChargeManagement",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishChargeManagement_JyotishRecords_JyotishId",
                table: "JyotishChargeManagement",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
