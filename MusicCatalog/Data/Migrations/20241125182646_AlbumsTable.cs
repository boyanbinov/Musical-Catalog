using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlbumsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idAlbum = table.Column<int>(type: "int", nullable: false),
                    idArtist = table.Column<int>(type: "int", nullable: false),
                    idLabel = table.Column<int>(type: "int", nullable: true),
                    strAlbum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intYearReleased = table.Column<int>(type: "int", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strReleaseFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strAlbumThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");
        }
    }
}
