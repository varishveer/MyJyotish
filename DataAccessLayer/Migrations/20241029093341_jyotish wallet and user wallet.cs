using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class jyotishwalletanduserwallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JyotishWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jyotishId = table.Column<int>(type: "int", nullable: false),
                    WalletAmount = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JyotishWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JyotishWallets_JyotishRecords_jyotishId",
                        column: x => x.jyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    WalletAmount = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWallets_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentRecord_UserId",
                table: "UserPaymentRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JyotishPaymentRecord_JyotishId",
                table: "JyotishPaymentRecord",
                column: "JyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_JyotishWallets_jyotishId",
                table: "JyotishWallets",
                column: "jyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWallets_userId",
                table: "UserWallets",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishPaymentRecord_JyotishRecords_JyotishId",
                table: "JyotishPaymentRecord",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPaymentRecord_Users_UserId",
                table: "UserPaymentRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishPaymentRecord_JyotishRecords_JyotishId",
                table: "JyotishPaymentRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPaymentRecord_Users_UserId",
                table: "UserPaymentRecord");

            migrationBuilder.DropTable(
                name: "JyotishWallets");

            migrationBuilder.DropTable(
                name: "UserWallets");

            migrationBuilder.DropIndex(
                name: "IX_UserPaymentRecord_UserId",
                table: "UserPaymentRecord");

            migrationBuilder.DropIndex(
                name: "IX_JyotishPaymentRecord_JyotishId",
                table: "JyotishPaymentRecord");
        }
    }
}
