using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ScheduledSongId = table.Column<int>(type: "integer", nullable: true),
                    ScheduledSongId1 = table.Column<int>(type: "integer", nullable: true),
                    ScheduledSongId2 = table.Column<int>(type: "integer", nullable: true),
                    ScheduledSongId3 = table.Column<int>(type: "integer", nullable: true),
                    ScheduledSongId4 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_ScheduledSongs_ScheduledSongId",
                        column: x => x.ScheduledSongId,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Member_ScheduledSongs_ScheduledSongId1",
                        column: x => x.ScheduledSongId1,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Member_ScheduledSongs_ScheduledSongId2",
                        column: x => x.ScheduledSongId2,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Member_ScheduledSongs_ScheduledSongId3",
                        column: x => x.ScheduledSongId3,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Member_ScheduledSongs_ScheduledSongId4",
                        column: x => x.ScheduledSongId4,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduledSongId",
                table: "Member",
                column: "ScheduledSongId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduledSongId1",
                table: "Member",
                column: "ScheduledSongId1");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduledSongId2",
                table: "Member",
                column: "ScheduledSongId2");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduledSongId3",
                table: "Member",
                column: "ScheduledSongId3");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduledSongId4",
                table: "Member",
                column: "ScheduledSongId4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Member");
        }
    }
}
