using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtwoclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedChordAttachment",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "SelectedSongAttachment",
                table: "ScheduledSongs");
        }
    }
}
