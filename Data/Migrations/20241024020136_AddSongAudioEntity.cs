using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Migrations
{
    /// <inheritdoc />
    public partial class AddSongAudioEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SongAudios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SongName = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    AudioData = table.Column<byte[]>(type: "bytea", nullable: true),
                    AudioType = table.Column<string>(type: "text", nullable: true),
                    VoicePart = table.Column<string>(type: "text", nullable: true),
                    Tonality = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongAudios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongAudios");
        }
    }
}
