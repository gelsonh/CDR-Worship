using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePropertiesToScheduledSongAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "ScheduledSongs",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ScheduledSongs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ScheduledSongs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ScheduledSongs");
        }
    }
}
