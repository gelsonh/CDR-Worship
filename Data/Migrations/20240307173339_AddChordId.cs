using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChordId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChordDocuments_Chords_ChordId",
                table: "ChordDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "ChordId",
                table: "ChordDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChordDocuments_Chords_ChordId",
                table: "ChordDocuments",
                column: "ChordId",
                principalTable: "Chords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChordDocuments_Chords_ChordId",
                table: "ChordDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "ChordId",
                table: "ChordDocuments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ChordDocuments_Chords_ChordId",
                table: "ChordDocuments",
                column: "ChordId",
                principalTable: "Chords",
                principalColumn: "Id");
        }
    }
}
