using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _13112024b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "AppointmentSlots",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_JyotishId",
                table: "AppointmentSlots",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRecords_SlotId",
                table: "AppointmentRecords",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentRecords_AppointmentSlots_SlotId",
                table: "AppointmentRecords",
                column: "SlotId",
                principalTable: "AppointmentSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_JyotishRecords_JyotishId",
                table: "AppointmentSlots",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentRecords_AppointmentSlots_SlotId",
                table: "AppointmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_JyotishRecords_JyotishId",
                table: "AppointmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentSlots_JyotishId",
                table: "AppointmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentRecords_SlotId",
                table: "AppointmentRecords");

            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "AppointmentSlots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
