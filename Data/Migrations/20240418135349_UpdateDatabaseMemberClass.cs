using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Worship.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseMemberClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Member",
                newName: "MemberName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberName",
                table: "Member",
                newName: "Name");
        }
    }
}
