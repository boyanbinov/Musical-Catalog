using System.ComponentModel.DataAnnotations;

namespace MusicCatalog.Models
{
    public class Album
    {
        [Key]
        public int id { get; set; }
        public string userId { get; set; }
        public int idAlbum { get; set; }
        public int idArtist { get; set; }
        public int? idLabel { get; set; }
        public string strAlbum { get; set; }
        public string strArtist { get; set; }
        public int? intYearReleased { get; set; }
        public string? strStyle { get; set; }
        public string? strGenre { get; set; }
        public string? strLabel { get; set; }
        public string? strReleaseFormat { get; set; }
        public string? strAlbumThumb { get; set; }
        public string? strDescriptionEN { get; set; }
        public bool IsLiked { get; set; }
    }
}
