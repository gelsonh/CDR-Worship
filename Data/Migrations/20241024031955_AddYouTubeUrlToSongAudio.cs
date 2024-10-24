using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddYouTubeUrlToSongAudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioData",
                table: "SongAudios");

            migrationBuilder.DropColumn(
                name: "AudioType",
                table: "SongAudios");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SongAudios");

            migrationBuilder.DropColumn(
                name: "Tonality",
                table: "SongAudios");

            migrationBuilder.RenameColumn(
                name: "VoicePart",
                table: "SongAudios",
                newName: "YouTubeUrl");

            migrationBuilder.AddColumn<int>(
                name: "ChordId",
                table: "SongAudios",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongAudios_ChordId",
                table: "SongAudios",
                column: "ChordId");

            migrationBuilder.AddForeignKey(
                name: "FK_SongAudios_Chords_ChordId",
                table: "SongAudios",
                column: "ChordId",
                principalTable: "Chords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongAudios_Chords_ChordId",
                table: "SongAudios");

            migrationBuilder.DropIndex(
                name: "IX_SongAudios_ChordId",
                table: "SongAudios");

            migrationBuilder.DropColumn(
                name: "ChordId",
                table: "SongAudios");

            migrationBuilder.RenameColumn(
                name: "YouTubeUrl",
                table: "SongAudios",
                newName: "VoicePart");

            migrationBuilder.AddColumn<byte[]>(
                name: "AudioData",
                table: "SongAudios",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioType",
                table: "SongAudios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SongAudios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tonality",
                table: "SongAudios",
                type: "text",
                nullable: true);
        }
    }
}
