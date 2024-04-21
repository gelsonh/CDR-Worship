using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ChordDocuments",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "ChordDocuments");
        }
    }
}
