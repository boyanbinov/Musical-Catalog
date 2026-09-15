using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAdminTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminArtist",
                columns: table => new
                {
                    idArtist = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idLabel = table.Column<int>(type: "int", nullable: true),
                    intFormedYear = table.Column<int>(type: "int", nullable: true),
                    intBornYear = table.Column<int>(type: "int", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strBiographyEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    intMembers = table.Column<int>(type: "int", nullable: true),
                    strCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strArtistThumb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminArtist", x => x.idArtist);
                });

            migrationBuilder.CreateTable(
                name: "AdminAlbum",
                columns: table => new
                {
                    idAlbum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idArtist = table.Column<int>(type: "int", nullable: false),
                    idLabel = table.Column<int>(type: "int", nullable: true),
                    strAlbum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    intYearReleased = table.Column<int>(type: "int", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strReleaseFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strAlbumThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAlbum", x => x.idAlbum);
                    table.ForeignKey(
                        name: "FK_AdminAlbum_AdminArtist_idArtist",
                        column: x => x.idArtist,
                        principalTable: "AdminArtist",
                        principalColumn: "idArtist",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminTrack",
                columns: table => new
                {
                    idTrack = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idAlbum = table.Column<int>(type: "int", nullable: false),
                    idArtist = table.Column<int>(type: "int", nullable: false),
                    strTrack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    strAlbum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strArtist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    intDuration = table.Column<int>(type: "int", nullable: false),
                    strGenre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strTrackThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strMusicVidCompany = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTrack", x => x.idTrack);
                    table.ForeignKey(
                        name: "FK_AdminTrack_AdminAlbum_idAlbum",
                        column: x => x.idAlbum,
                        principalTable: "AdminAlbum",
                        principalColumn: "idAlbum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAlbum_idArtist",
                table: "AdminAlbum",
                column: "idArtist");

            migrationBuilder.CreateIndex(
                name: "IX_AdminTrack_idAlbum",
                table: "AdminTrack",
                column: "idAlbum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminTrack");

            migrationBuilder.DropTable(
                name: "AdminAlbum");

            migrationBuilder.DropTable(
                name: "AdminArtist");
        }
    }
}
