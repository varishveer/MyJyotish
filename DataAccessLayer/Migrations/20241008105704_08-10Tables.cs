using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _0810Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishGalleryModel_JyotishRecords_JyotishId",
                table: "JyotishGalleryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishVideosModel_JyotishRecords_JyotishId",
                table: "JyotishVideosModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishVideosModel",
                table: "JyotishVideosModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishGalleryModel",
                table: "JyotishGalleryModel");

            migrationBuilder.RenameTable(
                name: "JyotishVideosModel",
                newName: "JyotishVideos");

            migrationBuilder.RenameTable(
                name: "JyotishGalleryModel",
                newName: "JyotishGallery");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishVideosModel_JyotishId",
                table: "JyotishVideos",
                newName: "IX_JyotishVideos_JyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishGalleryModel_JyotishId",
                table: "JyotishGallery",
                newName: "IX_JyotishGallery_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishVideos",
                table: "JyotishVideos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishGallery",
                table: "JyotishGallery",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishGallery_JyotishRecords_JyotishId",
                table: "JyotishGallery",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishVideos_JyotishRecords_JyotishId",
                table: "JyotishVideos",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JyotishGallery_JyotishRecords_JyotishId",
                table: "JyotishGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_JyotishVideos_JyotishRecords_JyotishId",
                table: "JyotishVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishVideos",
                table: "JyotishVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JyotishGallery",
                table: "JyotishGallery");

            migrationBuilder.RenameTable(
                name: "JyotishVideos",
                newName: "JyotishVideosModel");

            migrationBuilder.RenameTable(
                name: "JyotishGallery",
                newName: "JyotishGalleryModel");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishVideos_JyotishId",
                table: "JyotishVideosModel",
                newName: "IX_JyotishVideosModel_JyotishId");

            migrationBuilder.RenameIndex(
                name: "IX_JyotishGallery_JyotishId",
                table: "JyotishGalleryModel",
                newName: "IX_JyotishGalleryModel_JyotishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishVideosModel",
                table: "JyotishVideosModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JyotishGalleryModel",
                table: "JyotishGalleryModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishGalleryModel_JyotishRecords_JyotishId",
                table: "JyotishGalleryModel",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JyotishVideosModel_JyotishRecords_JyotishId",
                table: "JyotishVideosModel",
                column: "JyotishId",
                principalTable: "JyotishRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
