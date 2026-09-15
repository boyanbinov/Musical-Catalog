using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArtistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idArtist = table.Column<int>(type: "int", nullable: false),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idLabel = table.Column<int>(type: "int", nullable: true),
                    intFormedYear = table.Column<int>(type: "int", nullable: true),
                    intBornYear = table.Column<int>(type: "int", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strBiographyEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intMembers = table.Column<int>(type: "int", nullable: true),
                    strCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strArtistThumb = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
