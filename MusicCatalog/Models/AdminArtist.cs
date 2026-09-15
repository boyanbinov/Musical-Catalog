using MusicCatalogNoAPI_Test.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicCatalog.Models
{
    public class AdminArtist
    {
        [Key]
        public int idArtist { get; set; }
        public string? strArtist { get; set; }
        public string? strLabel { get; set; }
        public int? idLabel { get; set; }
        public int? intFormedYear { get; set; }
        public int? intBornYear { get; set; }
        public string? strStyle { get; set; }
        public string? strGenre { get; set; }
        public string? strBiographyEN { get; set; }
        public string? strGender { get; set; }
        public int? intMembers { get; set; }
        public string? strCountry { get; set; }
        public string? strArtistThumb { get; set; }

        // Navigation Property for Albums
        public ICollection<AdminAlbum> Albums { get; set; } = new List<AdminAlbum>();
    }
}
