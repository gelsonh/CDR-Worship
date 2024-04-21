using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddICollectionChordDocumentToScheduledSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId",
                table: "ChordDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChordDocuments_ScheduledSongId",
                table: "ChordDocuments",
                column: "ScheduledSongId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChordDocuments_ScheduledSongs_ScheduledSongId",
                table: "ChordDocuments",
                column: "ScheduledSongId",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChordDocuments_ScheduledSongs_ScheduledSongId",
                table: "ChordDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ChordDocuments_ScheduledSongId",
                table: "ChordDocuments");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId",
                table: "ChordDocuments");
        }
    }
}
