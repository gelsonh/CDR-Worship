using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentsCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId",
                table: "SongAttachments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId",
                table: "ChordAttachments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongAttachments_ScheduledSongId",
                table: "SongAttachments",
                column: "ScheduledSongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChordAttachments_ScheduledSongId",
                table: "ChordAttachments",
                column: "ScheduledSongId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChordAttachments_ScheduledSongs_ScheduledSongId",
                table: "ChordAttachments",
                column: "ScheduledSongId",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongAttachments_ScheduledSongs_ScheduledSongId",
                table: "SongAttachments",
                column: "ScheduledSongId",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChordAttachments_ScheduledSongs_ScheduledSongId",
                table: "ChordAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SongAttachments_ScheduledSongs_ScheduledSongId",
                table: "SongAttachments");

            migrationBuilder.DropIndex(
                name: "IX_SongAttachments_ScheduledSongId",
                table: "SongAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ChordAttachments_ScheduledSongId",
                table: "ChordAttachments");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId",
                table: "SongAttachments");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId",
                table: "ChordAttachments");
        }
    }
}
