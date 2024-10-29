using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentReplyAndCommentLikeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentId = table.Column<int>(type: "integer", nullable: false),
                    DocumentCommentId = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    LikedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentLikes_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentLikes_DocumentComments_DocumentCommentId",
                        column: x => x.DocumentCommentId,
                        principalTable: "DocumentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentCommentId = table.Column<int>(type: "integer", nullable: false),
                    ReplyText = table.Column<string>(type: "text", nullable: true),
                    ScheduledSongId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReplies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_AppUserId",
                table: "CommentLikes",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_DocumentCommentId",
                table: "CommentLikes",
                column: "DocumentCommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "CommentReplies");
        }
    }
}
