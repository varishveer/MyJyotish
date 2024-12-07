using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _07122024x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RedeemCodeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jyotishId = table.Column<int>(type: "int", nullable: false),
                    planId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCodeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedeemCodeRequest_JyotishRecords_jyotishId",
                        column: x => x.jyotishId,
                        principalTable: "JyotishRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedeemCodeRequest_Subscriptions_planId",
                        column: x => x.planId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCodeRequest_jyotishId",
                table: "RedeemCodeRequest",
                column: "jyotishId");

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCodeRequest_planId",
                table: "RedeemCodeRequest",
                column: "planId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedeemCodeRequest");
        }
    }
}
