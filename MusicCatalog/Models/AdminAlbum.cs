using MusicCatalog.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MusicCatalogNoAPI_Test.Models
{
    public class AdminAlbum
    {
        [Key]
        public int idAlbum { get; set; }
        [DisplayName("Artist")]
        [Required]
        public int idArtist { get; set; }
        // Navigation property for the related Artist
        [ForeignKey(nameof(idArtist))]
        public AdminArtist? Artist { get; set; }
        public int? idLabel { get; set; }
        [Required]
        public string? strAlbum { get; set; }
        public string? strArtist { get; set; }
        public int? intYearReleased { get; set; }
        public string? strStyle { get; set; }
        public string? strGenre { get; set; }
        public string? strLabel { get; set; }
        public string? strReleaseFormat { get; set; }
        public string? strAlbumThumb { get; set; }
        public string? strDescriptionEN { get; set; }

        // Navigation property for tracks
        public ICollection<AdminTrack> Tracks { get; set; } = new List<AdminTrack>();
    }
}
