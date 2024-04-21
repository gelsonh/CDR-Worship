using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseMemberClass2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId1",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId2",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId3",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId4",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduledSongId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduledSongId1",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduledSongId2",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduledSongId3",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduledSongId4",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId1",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId2",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId3",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduledSongId4",
                table: "Member");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "ScheduledSongs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_MemberId",
                table: "ScheduledSongs",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Member_MemberId",
                table: "ScheduledSongs",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Member_MemberId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_MemberId",
                table: "ScheduledSongs");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ScheduledSongs");

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId",
                table: "Member",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId1",
                table: "Member",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId2",
                table: "Member",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId3",
                table: "Member",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledSongId4",
                table: "Member",
                type: "integer",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId",
                table: "Member",
                column: "ScheduledSongId",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId1",
                table: "Member",
                column: "ScheduledSongId1",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId2",
                table: "Member",
                column: "ScheduledSongId2",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId3",
                table: "Member",
                column: "ScheduledSongId3",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduledSongs_ScheduledSongId4",
                table: "Member",
                column: "ScheduledSongId4",
                principalTable: "ScheduledSongs",
                principalColumn: "Id");
        }
    }
}
