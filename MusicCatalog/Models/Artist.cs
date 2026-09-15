using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicCatalog.Models
{
    public class Artist
    {
        [Key]
        public int id { get; set; }
        public string userId { get; set; }
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

        //TODO
        /*
         add artists albums
         add artist tracks
         add artists label   
         */
    }
}
