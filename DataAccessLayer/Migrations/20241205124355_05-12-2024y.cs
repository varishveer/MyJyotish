using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _05122024y : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback");

            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "InterviewFeedback",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback");

            migrationBuilder.AlterColumn<int>(
                name: "JyotishId",
                table: "InterviewFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
