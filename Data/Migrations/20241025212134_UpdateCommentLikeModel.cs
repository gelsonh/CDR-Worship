using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentLikeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "CommentLikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "CommentLikes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
