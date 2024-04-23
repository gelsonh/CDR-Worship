using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoristaTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackingVocalistTwoId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_BackingVocalistTwoId",
                table: "ScheduledSongs",
                column: "BackingVocalistTwoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_BackingVocalistTwoId",
                table: "ScheduledSongs",
                column: "BackingVocalistTwoId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_BackingVocalistTwoId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_BackingVocalistTwoId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "BackingVocalistTwoId",
                table: "ScheduledSongs");
        }
    }
}
