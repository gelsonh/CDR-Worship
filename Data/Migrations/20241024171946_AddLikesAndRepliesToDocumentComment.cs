using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndRepliesToDocumentComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "DocumentComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "DocumentComments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentComments_ParentCommentId",
                table: "DocumentComments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentComments_DocumentComments_ParentCommentId",
                table: "DocumentComments",
                column: "ParentCommentId",
                principalTable: "DocumentComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentComments_DocumentComments_ParentCommentId",
                table: "DocumentComments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentComments_ParentCommentId",
                table: "DocumentComments");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "DocumentComments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "DocumentComments");
        }
    }
}
