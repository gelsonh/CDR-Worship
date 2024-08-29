using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class MajorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChordDocumentId = table.Column<int>(type: "integer", nullable: false),
                    SongDocumentId = table.Column<int>(type: "integer", nullable: false),
                    ScheduledSongId = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentComments_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentComments_ChordDocuments_ChordDocumentId",
                        column: x => x.ChordDocumentId,
                        principalTable: "ChordDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentComments_ScheduledSongs_ScheduledSongId",
                        column: x => x.ScheduledSongId,
                        principalTable: "ScheduledSongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentComments_SongDocuments_SongDocumentId",
                        column: x => x.SongDocumentId,
                        principalTable: "SongDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentComments_AppUserId",
                table: "DocumentComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentComments_ChordDocumentId",
                table: "DocumentComments",
                column: "ChordDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentComments_ScheduledSongId",
                table: "DocumentComments",
                column: "ScheduledSongId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentComments_SongDocumentId",
                table: "DocumentComments",
                column: "SongDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentComments");
        }
    }
}
