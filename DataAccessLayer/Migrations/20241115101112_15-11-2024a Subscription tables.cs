using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _15112024aSubscriptiontables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceUrl",
                table: "SubscriptionFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "SubscriptionFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCount",
                table: "ManageSubscriptionModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ManageSubscriptionModels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceUrl",
                table: "SubscriptionFeatures");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SubscriptionFeatures");

            migrationBuilder.DropColumn(
                name: "ServiceCount",
                table: "ManageSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ManageSubscriptionModels");
        }
    }
}
