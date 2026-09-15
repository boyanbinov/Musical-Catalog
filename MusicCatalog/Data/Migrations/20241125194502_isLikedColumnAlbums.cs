using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class isLikedColumnAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "Albums",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "Albums");
        }
    }
}
