using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class NombreDeLaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Archived",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "ImageFileData",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "ImageFileType",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId",
                table: "ChordAttachments");

            migrationBuilder.AddColumn<int>(
                name: "ChordId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tempo",
                table: "ChordDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_ChordId",
                table: "ScheduledSongs",
                column: "ChordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Chords_ChordId",
                table: "ScheduledSongs",
                column: "ChordId",
                principalTable: "Chords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Chords_ChordId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_ChordId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "ChordId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "Tempo",
                table: "ChordDocuments");

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId",
                table: "SongAttachments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "ScheduledSongs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ScheduledSongs",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFileData",
                table: "ScheduledSongs",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileType",
                table: "ScheduledSongs",
                type: "text",
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
    }
}
