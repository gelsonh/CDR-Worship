using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddFileDataToChordAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChordAttachments_SongDocuments_SongDocumentId",
                table: "ChordAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SongAttachments_ChordDocuments_ChordDocumentId",
                table: "SongAttachments");

            migrationBuilder.DropIndex(
                name: "IX_SongAttachments_ChordDocumentId",
                table: "SongAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ChordAttachments_SongDocumentId",
                table: "ChordAttachments");

            migrationBuilder.DropColumn(
                name: "ChordDocumentId",
                table: "SongAttachments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "SongAttachments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ChordDocuments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ChordAttachments");

            migrationBuilder.DropColumn(
                name: "SongDocumentId",
                table: "ChordAttachments");

            migrationBuilder.RenameColumn(
                name: "File",
                table: "SongDocuments",
                newName: "FileData");

            migrationBuilder.RenameColumn(
                name: "PdfData",
                table: "ChordDocuments",
                newName: "FileData");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SongDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "SongDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ChordDocuments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SongDocuments");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "SongDocuments");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ChordDocuments");

            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "SongDocuments",
                newName: "File");

            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "ChordDocuments",
                newName: "PdfData");

            migrationBuilder.AddColumn<int>(
                name: "ChordDocumentId",
                table: "SongAttachments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "SongAttachments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ChordDocuments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ChordAttachments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SongDocumentId",
                table: "ChordAttachments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongAttachments_ChordDocumentId",
                table: "SongAttachments",
                column: "ChordDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChordAttachments_SongDocumentId",
                table: "ChordAttachments",
                column: "SongDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChordAttachments_SongDocuments_SongDocumentId",
                table: "ChordAttachments",
                column: "SongDocumentId",
                principalTable: "SongDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongAttachments_ChordDocuments_ChordDocumentId",
                table: "SongAttachments",
                column: "ChordDocumentId",
                principalTable: "ChordDocuments",
                principalColumn: "Id");
        }
    }
}
