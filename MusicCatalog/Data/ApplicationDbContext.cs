using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Models;
using MusicCatalogNoAPI_Test.Models;

namespace MusicCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // add tables
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<AdminAlbum> AdminAlbum { get; set; }
        public DbSet<AdminArtist> AdminArtist { get; set; }
        public DbSet<AdminTrack> AdminTrack { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }
}
