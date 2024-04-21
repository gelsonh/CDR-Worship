using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClassInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackingVocalistId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BassistId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrummerId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadGuitaristId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadSingerId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondGuitaristId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackingVocalistId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "BassistId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "DrummerId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "LeadGuitaristId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "LeadSingerId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "SecondGuitaristId",
                table: "ScheduledSongs");
        }
    }
}
