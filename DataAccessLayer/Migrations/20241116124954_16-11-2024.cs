using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _16112024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubsciptionManagementModel_JyotishRecords_JyotishId",
                table: "SubsciptionManagementModel");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsciptionManagementModel_Subscriptions_SubscriptionId",
                table: "SubsciptionManagementModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubsciptionManagementModel",
                table: "SubsciptionManagementModel");

            migrationBuilder.RenameTable(
                name: "SubsciptionManagementModel",
                newName: "PackageManager");

            migrationBuilder.RenameIndex(
                name: "IX_SubsciptionManagementModel_SubscriptionId",
                table: "PackageManager",
                newName: "IX_PackageManager_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_SubsciptionManagementModel_JyotishId",
                table: "PackageManager",
                newName: "IX_PackageManager_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageManager",
                table: "PackageManager",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageManager_JyotishRecords_JyotishId",
                table: "PackageManager",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageManager_Subscriptions_SubscriptionId",
                table: "PackageManager",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageManager_JyotishRecords_JyotishId",
                table: "PackageManager");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageManager_Subscriptions_SubscriptionId",
                table: "PackageManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageManager",
                table: "PackageManager");

            migrationBuilder.RenameTable(
                name: "PackageManager",
                newName: "SubsciptionManagementModel");

            migrationBuilder.RenameIndex(
                name: "IX_PackageManager_SubscriptionId",
                table: "SubsciptionManagementModel",
                newName: "IX_SubsciptionManagementModel_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageManager_JyotishId",
                table: "SubsciptionManagementModel",
                newName: "IX_SubsciptionManagementModel_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubsciptionManagementModel",
                table: "SubsciptionManagementModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubsciptionManagementModel_JyotishRecords_JyotishId",
                table: "SubsciptionManagementModel",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubsciptionManagementModel_Subscriptions_SubscriptionId",
                table: "SubsciptionManagementModel",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
