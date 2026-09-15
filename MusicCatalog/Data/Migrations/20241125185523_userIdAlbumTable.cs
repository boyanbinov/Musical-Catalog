using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class userIdAlbumTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Albums");
        }
    }
}
