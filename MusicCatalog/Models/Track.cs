using System.ComponentModel.DataAnnotations;

namespace MusicCatalog.Models
{
    public class Track
    {
        [Key]
        public int id { get; set; }
        public string userId { get; set; }
        public int idTrack { get; set; }
        public int? idAlbum { get; set; }
        public int idArtist { get; set; }
        public string? strTrack { get; set; }
        public string? strAlbum { get; set; }
        public string? strArtist { get; set; }
        public int intDuration { get; set; }
        public string? strGenre { get; set; }
        public string? strStyle { get; set; }
        public string? strDescriptionEN { get; set; }
        public string? strTrackThumb { get; set; }
        public string? strMusicVidCompany { get; set; }
        public bool IsLiked { get; set; }
    }
}
