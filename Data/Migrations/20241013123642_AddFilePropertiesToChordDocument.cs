using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePropertiesToChordDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfData",
                table: "ChordDocuments",
                newName: "FileData");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ChordDocuments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ChordDocuments");

            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "ChordDocuments",
                newName: "PdfData");
        }
    }
}
