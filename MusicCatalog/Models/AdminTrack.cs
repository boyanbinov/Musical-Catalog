using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MusicCatalogNoAPI_Test.Models;

namespace MusicCatalog.Models
{
    public class AdminTrack
    {
        [Key]
        public int idTrack { get; set; }
        [Required]
        public int idAlbum { get; set; }
        [ForeignKey(nameof(idAlbum))]
        // Navigation property
        public AdminAlbum? Album { get; set; }
        public int idArtist { get; set; }
        [Required]
        public string? strTrack { get; set; }
        public string? strAlbum { get; set; }
        public string? strArtist { get; set; }
        [Required]
        public int intDuration { get; set; }
        public string? strGenre { get; set; }
        public string? strStyle { get; set; }
        public string? strDescriptionEN { get; set; }
        public string? strTrackThumb { get; set; }
        public string? strMusicVidCompany { get; set; }
    }
}
