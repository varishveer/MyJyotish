using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _12112024bSomechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_Id",
                table: "ClientMembers");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ClientMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMembers_AppointmentId",
                table: "ClientMembers",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_AppointmentId",
                table: "ClientMembers",
                column: "AppointmentId",
                principalTable: "AppointmentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_AppointmentId",
                table: "ClientMembers");

            migrationBuilder.DropIndex(
                name: "IX_ClientMembers_AppointmentId",
                table: "ClientMembers");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ClientMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_Id",
                table: "ClientMembers",
                column: "Id",
                principalTable: "AppointmentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
