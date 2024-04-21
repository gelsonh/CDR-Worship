using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_BackingVocalistId",
                table: "ScheduledSongs",
                column: "BackingVocalistId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_BassistId",
                table: "ScheduledSongs",
                column: "BassistId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_DrummerId",
                table: "ScheduledSongs",
                column: "DrummerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_LeadGuitaristId",
                table: "ScheduledSongs",
                column: "LeadGuitaristId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_LeadSingerId",
                table: "ScheduledSongs",
                column: "LeadSingerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSongs_SecondGuitaristId",
                table: "ScheduledSongs",
                column: "SecondGuitaristId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_BackingVocalistId",
                table: "ScheduledSongs",
                column: "BackingVocalistId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_BassistId",
                table: "ScheduledSongs",
                column: "BassistId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_DrummerId",
                table: "ScheduledSongs",
                column: "DrummerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_LeadGuitaristId",
                table: "ScheduledSongs",
                column: "LeadGuitaristId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_LeadSingerId",
                table: "ScheduledSongs",
                column: "LeadSingerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledSongs_Members_SecondGuitaristId",
                table: "ScheduledSongs",
                column: "SecondGuitaristId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_BackingVocalistId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_BassistId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_DrummerId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_LeadGuitaristId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_LeadSingerId",
                table: "ScheduledSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledSongs_Members_SecondGuitaristId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_BackingVocalistId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_BassistId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_DrummerId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_LeadGuitaristId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_LeadSingerId",
                table: "ScheduledSongs");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledSongs_SecondGuitaristId",
                table: "ScheduledSongs");
        }
    }
}
