using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _30102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JyotishWalletHistroy");

            migrationBuilder.DropTable(
                name: "UserWalletHistroy");

            migrationBuilder.CreateTable(
                name: "WalletHistroy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JId = table.Column<int>(type: "int", nullable: true),
                    UId = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletHistroy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletHistroy_JyotishRecords_JId",
                        column: x => x.JId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WalletHistroy_Users_UId",
                        column: x => x.UId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistroy_JId",
                table: "WalletHistroy",
                column: "JId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistroy_UId",
                table: "WalletHistroy",
                column: "UId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletHistroy");

            migrationBuilder.CreateTable(
                name: "JyotishWalletHistroy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JId = table.Column<int>(type: "int", nullable: false),
                    PaymentAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JyotishWalletHistroy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JyotishWalletHistroy_JyotishRecords_JId",
                        column: x => x.JId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWalletHistroy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UId = table.Column<int>(type: "int", nullable: false),
                    PaymentAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWalletHistroy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWalletHistroy_Users_UId",
                        column: x => x.UId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JyotishWalletHistroy_JId",
                table: "JyotishWalletHistroy",
                column: "JId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWalletHistroy_UId",
                table: "UserWalletHistroy",
                column: "UId");
        }
    }
}
