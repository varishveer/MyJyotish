using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class interviewFeedBackTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedbackModel_JyotishRecords_JyotishId",
                table: "InterviewFeedbackModel");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedbackModel_Slots_InterviewId",
                table: "InterviewFeedbackModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterviewFeedbackModel",
                table: "InterviewFeedbackModel");

            migrationBuilder.RenameTable(
                name: "InterviewFeedbackModel",
                newName: "InterviewFeedback");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewFeedbackModel_JyotishId",
                table: "InterviewFeedback",
                newName: "IX_InterviewFeedback_JyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewFeedbackModel_InterviewId",
                table: "InterviewFeedback",
                newName: "IX_InterviewFeedback_InterviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterviewFeedback",
                table: "InterviewFeedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedback_Slots_InterviewId",
                table: "InterviewFeedback",
                column: "InterviewId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedback_JyotishRecords_JyotishId",
                table: "InterviewFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedback_Slots_InterviewId",
                table: "InterviewFeedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterviewFeedback",
                table: "InterviewFeedback");

            migrationBuilder.RenameTable(
                name: "InterviewFeedback",
                newName: "InterviewFeedbackModel");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewFeedback_JyotishId",
                table: "InterviewFeedbackModel",
                newName: "IX_InterviewFeedbackModel_JyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewFeedback_InterviewId",
                table: "InterviewFeedbackModel",
                newName: "IX_InterviewFeedbackModel_InterviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterviewFeedbackModel",
                table: "InterviewFeedbackModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedbackModel_JyotishRecords_JyotishId",
                table: "InterviewFeedbackModel",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedbackModel_Slots_InterviewId",
                table: "InterviewFeedbackModel",
                column: "InterviewId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
