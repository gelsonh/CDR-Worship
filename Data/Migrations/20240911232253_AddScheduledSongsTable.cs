using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledSongsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarDayId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalendarId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    CalendarId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarDays_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_CalendarDayId",
                table: "ScheduledSongs",
                column: "CalendarDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_CalendarId",
                table: "ScheduledSongs",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarId",
                table: "CalendarDays",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_CalendarDays_CalendarDayId",
                table: "ScheduledSongs",
                column: "CalendarDayId",
                principalTable: "CalendarDays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Calendars_CalendarId",
                table: "ScheduledSongs",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_CalendarDays_CalendarDayId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Calendars_CalendarId",
                table: "ScheduledSongs");

            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_CalendarDayId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_CalendarId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "CalendarDayId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "ScheduledSongs");
        }
    }
}
