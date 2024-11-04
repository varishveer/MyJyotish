using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _04112024dJyotishTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_jyotishSignUpTempRecords",
                table: "jyotishSignUpTempRecords");

            migrationBuilder.RenameTable(
                name: "jyotishSignUpTempRecords",
                newName: "jyotishTempRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jyotishTempRecords",
                table: "jyotishTempRecords",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_jyotishTempRecords",
                table: "jyotishTempRecords");

            migrationBuilder.RenameTable(
                name: "jyotishTempRecords",
                newName: "jyotishSignUpTempRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jyotishSignUpTempRecords",
                table: "jyotishSignUpTempRecords",
                column: "Id");
        }
    }
}
