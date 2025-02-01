using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _01022025a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseAdvertisement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activeStatus = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAdvertisement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseAdvertisement_AdvertisementPackage_adId",
                        column: x => x.adId,
                        principalTable: "AdvertisementPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdvertisement_adId",
                table: "PurchaseAdvertisement",
                column: "adId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseAdvertisement");
        }
    }
}
