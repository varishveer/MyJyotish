using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _12112024gSomechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_AppointmentId",
                table: "ClientMembers");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "ClientMembers",
                newName: "UId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientMembers_AppointmentId",
                table: "ClientMembers",
                newName: "IX_ClientMembers_UId");

            migrationBuilder.AddColumn<int>(
                name: "memberId",
                table: "ProblemSolution",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSolution_memberId",
                table: "ProblemSolution",
                column: "memberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMembers_Users_UId",
                table: "ClientMembers",
                column: "UId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSolution_ClientMembers_memberId",
                table: "ProblemSolution",
                column: "memberId",
                principalTable: "ClientMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMembers_Users_UId",
                table: "ClientMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSolution_ClientMembers_memberId",
                table: "ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_ProblemSolution_memberId",
                table: "ProblemSolution");

            migrationBuilder.DropColumn(
                name: "memberId",
                table: "ProblemSolution");

            migrationBuilder.RenameColumn(
                name: "UId",
                table: "ClientMembers",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientMembers_UId",
                table: "ClientMembers",
                newName: "IX_ClientMembers_AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMembers_AppointmentRecords_AppointmentId",
                table: "ClientMembers",
                column: "AppointmentId",
                principalTable: "AppointmentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
