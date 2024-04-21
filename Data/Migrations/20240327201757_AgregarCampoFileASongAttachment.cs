using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoFileASongAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedChordAttachment",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "SelectedSongAttachment",
                table: "ScheduledSongs");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "SongAttachments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ScheduledSongs",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PdfData",
                table: "ChordDocuments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ChordAttachments",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "SongAttachments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "PdfData",
                table: "ChordDocuments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ChordAttachments");

            migrationBuilder.AddColumn<int>(
                name: "SelectedChordAttachment",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedSongAttachment",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);
        }
    }
}
