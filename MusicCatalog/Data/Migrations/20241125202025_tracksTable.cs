using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class tracksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idTrack = table.Column<int>(type: "int", nullable: false),
                    idAlbum = table.Column<int>(type: "int", nullable: true),
                    idArtist = table.Column<int>(type: "int", nullable: false),
                    strTrack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strAlbum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intDuration = table.Column<int>(type: "int", nullable: false),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strTrackThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strMusicVidCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracks");
        }
    }
}
