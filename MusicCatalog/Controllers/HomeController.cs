using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Data;
using MusicCatalog.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace MusicCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ArtistData(string artist_name)
        {
            if (string.IsNullOrEmpty(artist_name))
            {
                ViewData["found"] = "Artist name cannot be empty.";
                return View(new Artist());
            }

            Artist artist = new Artist();

            // Search for the artist in the database
            var userId = User.Identity.Name; // Get current user ID
            AdminArtist adminArtist = _context.AdminArtist
                .FirstOrDefault(a => a.strArtist.ToLower() == artist_name.ToLower());

            if (adminArtist != null)
            {
                // Map AdminArtist to Artist object
                artist = new Artist
                {
                    idArtist = adminArtist.idArtist,
                    strArtist = adminArtist.strArtist,
                    strLabel = adminArtist.strLabel,
                    intFormedYear = adminArtist.intFormedYear ?? 0,
                    intBornYear = adminArtist.intBornYear ?? 0,
                    strStyle = adminArtist.strStyle,
                    strGenre = adminArtist.strGenre,
                    strBiographyEN = adminArtist.strBiographyEN,
                    strGender = adminArtist.strGender,
                    intMembers = adminArtist.intMembers ?? 0,
                    strCountry = adminArtist.strCountry,
                    strArtistThumb = adminArtist.strArtistThumb
                };
                userId = User.Identity.Name;
                if (userId != null)
                {
                    bool isLiked = _context.Artists
                        .Any(a => a.idArtist == artist.idArtist && a.userId == userId);
                    ViewData["isLiked"] = isLiked;
                }
                else
                {
                    ViewData["isLiked"] = false;
                }
                ViewData["artist"] = artist;
                ViewData["found"] = "Artist found in the database.";
                return View(artist); // Return the mapped Artist object to the view
            }

            // Artist not found in the database, search via API
            else
            {
                var url = "https://www.theaudiodb.com/api/v1/json/523532/search.php?s=";

                var client = new WebClient();

                string body = "";

                if (artist_name != null && artist_name != "")
                {
                    body = client.DownloadString(url + artist_name);

                    // JSON to JObject
                    JObject data = JObject.Parse(body);

                    if (data != null)
                    {
                        //ViewData["artist_name"] = data["artists"][0];

                        JToken result = data["artists"][0];
                        if (result != null)
                        {
                            artist = result.ToObject<Artist>();

                        }
                        else
                        {
                            ViewData["found"] = "Artist not found " + artist_name;
                        }
                    }
                    else
                    {
                        ViewData["found"] = "Няма намерена винетка " + artist_name;
                    }

                    ViewData["body"] = body;
                }

                // Check if the artist is liked by the user
                userId = User.Identity.Name;
                if (userId != null)
                {
                    bool isLiked = _context.Artists
                        .Any(a => a.idArtist == artist.idArtist && a.userId == userId);
                    ViewData["isLiked"] = isLiked;
                }
                else
                {
                    ViewData["isLiked"] = false;
                }

                ViewData["artist"] = artist;

                return View(artist);
            }
        }


        [HttpPost]
        public IActionResult AlbumsData(string artist_name)
        {
            // Here we process the POST data
            return RedirectToAction("AlbumsData", new { artist_name = artist_name });
        }

        [HttpGet]
        public IActionResult AlbumsData(string artist_name, int page = 1)
        {
            if (string.IsNullOrEmpty(artist_name))
            {
                ViewData["found"] = "Artist name cannot be empty.";
                return View(new List<Album>());
            }

            // Check for albums in the AdminAlbum model
            List<Album> albums = _context.AdminAlbum
                .Where(a => a.strArtist.ToLower() == artist_name.ToLower())
                .Select(a => new Album
                {
                    idAlbum = a.idAlbum,
                    strAlbum = a.strAlbum,
                    strArtist = a.strArtist,
                    strAlbumThumb = a.strAlbumThumb,
                    intYearReleased = a.intYearReleased,
                    // Map any additional properties as needed
                })
                .ToList();

            // If no albums are found in the database, search via API
            if (albums.Count == 0)
            {
                var url = "https://www.theaudiodb.com/api/v1/json/523532/searchalbum.php?s=";
                var client = new WebClient();

                try
                {
                    // URL encode the artist name to ensure it's valid for the API
                    string encodedArtistName = HttpUtility.UrlEncode(artist_name);

                    // Make the API call
                    string body = client.DownloadString(url + encodedArtistName);

                    // Parse the JSON response
                    JObject data = JObject.Parse(body);

                    if (data != null && data["album"] != null)
                    {
                        JArray results = (JArray)data["album"];
                        if (results.Count > 0)
                        {
                            // Convert JSON data to a list of Album objects
                            albums = results.ToObject<List<Album>>();
                        }
                        else
                        {
                            ViewData["found"] = "No albums found for " + artist_name;
                        }
                    }
                    else
                    {
                        ViewData["found"] = "No albums found for " + artist_name;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["found"] = "Error fetching albums: " + ex.Message;
                    return View(new List<Album>());
                }
            }

            // Handle pagination
            const int pageSize = 8; // Number of albums per page
            int totalAlbums = albums.Count;
            int totalPages = (int)Math.Ceiling((double)totalAlbums / pageSize);

            // Validate current page number
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            // Get the albums for the current page
            var albumsToShow = albums.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Check if albums are liked by the current user
            var userId = User.Identity.Name;
            if (userId != null)
            {
                foreach (var album in albumsToShow)
                {
                    bool isLiked = _context.Albums
                        .Any(a => a.idAlbum == album.idAlbum && a.userId == userId);
                    album.IsLiked = isLiked; // Assuming Album model has an IsLiked property
                }
            }

            // Pass data to the view
            ViewData["albums"] = albumsToShow;
            ViewData["currentPage"] = page;
            ViewData["totalPages"] = totalPages;
            ViewData["artist_name"] = artist_name;

            return View(albumsToShow);
        }

        // POST Method for TracksData
        [HttpPost]
        public IActionResult TracksData(string artist_name, string track_name)
        {
            return RedirectToAction("TracksDataGet", new { artist_name = artist_name, track_name = track_name });
        }

        // GET Method for TracksData
        [HttpGet]
        public IActionResult TracksDataGet(string artist_name, string track_name)
        {
            if (string.IsNullOrEmpty(artist_name) || string.IsNullOrEmpty(track_name))
            {
                ViewData["found"] = "Please provide both an artist name and a track name.";
                return View(new Track());
            }

            Track track = null;  // Variable to store track data
            var userId = User.Identity.Name;

            // Step 1: Search in AdminTracks database
            var adminTrack = _context.AdminTrack
                .FirstOrDefault(t => t.strArtist.ToLower() == artist_name.ToLower() && t.strTrack.ToLower() == track_name.ToLower());

            if (adminTrack != null)
            {
                // Map AdminTrack to Track object
                track = new Track
                {
                    idTrack = adminTrack.idTrack,
                    strTrack = adminTrack.strTrack,
                    strArtist = adminTrack.strArtist,
                    strAlbum = adminTrack.strAlbum,
                    intDuration = adminTrack.intDuration,
                    strGenre = adminTrack.strGenre,
                    strStyle = adminTrack.strStyle,
                    strDescriptionEN = adminTrack.strDescriptionEN,
                    strTrackThumb = adminTrack.strTrackThumb
                };

                // Check if the track is liked by the user
                if (userId != null)
                {
                    bool isLiked = _context.Tracks
                        .Any(t => t.idTrack == track.idTrack && t.userId == userId);
                    track.IsLiked = isLiked;
                }

                ViewData["found"] = "Track found in the database.";
                ViewData["track"] = track;
                ViewData["artist_name"] = artist_name;
                ViewData["track_name"] = track_name;

                return View(track); // Return the track data from the database
            }

            // Step 2: If not found, fetch from API
            var client = new WebClient();
            string body = "";

            body = client.DownloadString($"https://www.theaudiodb.com/api/v1/json/523532/searchtrack.php?s={HttpUtility.UrlEncode(artist_name)}&t={HttpUtility.UrlEncode(track_name)}");

            // Parse the JSON response
            JObject data = JObject.Parse(body);

            if (data != null)
            {
                JToken result = data["track"]?.FirstOrDefault(); // Extract the first track object if available

                if (result != null)
                {
                    // Convert the JSON data to a Track object
                    track = result.ToObject<Track>();

                    // Check if the track is liked by the user
                    if (userId != null)
                    {
                        bool isLiked = _context.Tracks
                            .Any(t => t.idTrack == track.idTrack && t.userId == userId);
                        track.IsLiked = isLiked;
                    }

                    ViewData["found"] = "Track found via API.";
                }
                else
                {
                    ViewData["found"] = "No track found for " + artist_name + " - " + track_name;
                }
            }
            else
            {
                ViewData["found"] = "Error retrieving track data.";
            }

            // Pass the raw response body to ViewData for debugging or logging purposes
            ViewData["body"] = body;

            // Pass the track data and search parameters to the view
            ViewData["track"] = track;
            ViewData["artist_name"] = artist_name;
            ViewData["track_name"] = track_name;

            // Return the track data to the view
            return View(track);
        }


        public IActionResult UserProfile(string activeTab = "artists")
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            List<Artist> likedArtists = _context.Artists
                                                .Where(a => a.userId == userId)
                                                .ToList();
            List<Album> likedAlbums = _context.Albums
                                              .Where(a => a.userId == userId)
                                              .ToList();
            List<Track> likedTracks = _context.Tracks
                                              .Where(t => t.userId == userId)
                                              .ToList();

            ViewData["LikedArtists"] = likedArtists;
            ViewData["LikedAlbums"] = likedAlbums;
            ViewData["LikedTracks"] = likedTracks;
            ViewData["activeTab"] = activeTab;  // Pass activeTab to the view

            return View();
        }




        [HttpPost]
        public IActionResult LikeArtist(Artist likedArtist)
        {
            var userId = User.Identity.Name; // Assuming user authentication is set up
            if (userId == null)
                return Unauthorized();

            likedArtist.userId = userId; // Set the userId of the artist to the current user's ID

            // Check if the artist with the same idArtist and userId already exists for this user
            var existingArtist = _context.Artists
                .FirstOrDefault(a => a.idArtist == likedArtist.idArtist && a.userId == userId);

            if (existingArtist != null)
            {
                TempData["Message"] = $"{likedArtist.strArtist} is already liked by you!";
                return RedirectToAction("ArtistData", new { artist_name = likedArtist.strArtist });
            }

            // If the artist is not already liked by the user, add it
            _context.Add(likedArtist);
            _context.SaveChanges();

            TempData["Message"] = $"{likedArtist.strArtist} has been liked!";
            return RedirectToAction("ArtistData", new { artist_name = likedArtist.strArtist });
        }

        [HttpPost]
        public IActionResult UnlikeArtist(int idArtist)
        {
            var userId = User.Identity.Name; // Assuming user authentication is set up
            if (userId == null)
                return Unauthorized();

            // Find the artist entry for the current user
            var likedArtist = _context.Artists
                .FirstOrDefault(a => a.idArtist == idArtist && a.userId == userId);

            if (likedArtist != null)
            {
                // Remove the artist from the database
                _context.Artists.Remove(likedArtist);
                _context.SaveChanges();

                TempData["Message"] = $"{likedArtist.strArtist} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Artist not found in your liked list.";
            }


            // Redirect back to the ArtistData view
            return RedirectToAction("ArtistData", new { artist_name = likedArtist?.strArtist });
        }

        [HttpPost]
        public IActionResult LikeAlbum(Album album, int currentPage)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Check if the album is already liked by the user
            var existingAlbum = _context.Albums
                .FirstOrDefault(a => a.idAlbum == album.idAlbum && a.userId == userId);

            if (existingAlbum != null)
            {
                TempData["Message"] = $"{album.strAlbum} is already liked by you!";
                return RedirectToAction("AlbumsData", new { artist_name = album.strArtist, page = currentPage });
            }

            // Add the album to the liked list
            var likedAlbum = new Album
            {
                userId = userId,
                idAlbum = album.idAlbum,
                idArtist = album.idArtist,
                idLabel = album.idLabel,
                strAlbum = album.strAlbum,
                strArtist = album.strArtist,
                strAlbumThumb = album.strAlbumThumb,
                intYearReleased = album.intYearReleased,
                strStyle = album.strStyle,
                strGenre = album.strGenre,
                strLabel = album.strLabel,
                strReleaseFormat = album.strReleaseFormat,
                strDescriptionEN = album.strDescriptionEN,
            };

            _context.Albums.Add(likedAlbum);
            _context.SaveChanges();

            TempData["Message"] = $"{album.strAlbum} has been liked!";
            return RedirectToAction("AlbumsData", new { artist_name = album.strArtist, page = currentPage });
        }


        [HttpPost]
        public IActionResult UnlikeAlbum(int idAlbum, int currentPage)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Find the liked album entry
            var likedAlbum = _context.Albums
                .FirstOrDefault(a => a.idAlbum == idAlbum && a.userId == userId);

            if (likedAlbum != null)
            {
                _context.Albums.Remove(likedAlbum);
                _context.SaveChanges();

                TempData["Message"] = $"{likedAlbum.strAlbum} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Album not found in your liked list.";
            }

            return RedirectToAction("AlbumsData", new { artist_name = likedAlbum?.strArtist, page = currentPage });
        }

        [HttpPost]
        public IActionResult LikeTrack(Track likedTrack)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Check if the track is already liked by the user
            var existingTrack = _context.Tracks
                .FirstOrDefault(t => t.idTrack == likedTrack.idTrack && t.userId == userId);

            if (existingTrack != null)
            {
                TempData["Message"] = $"{likedTrack.strTrack} is already liked by you!";
                return RedirectToAction("TracksDataGet", new { artist_name = likedTrack.strArtist, track_name = likedTrack.strTrack });
            }

            // Add the track to the liked list
            var newTrack = new Track
            {
                idTrack = likedTrack.idTrack,
                userId = userId,
                idArtist = likedTrack.idArtist,
                strTrack = likedTrack.strTrack,
                strArtist = likedTrack.strArtist,
                strAlbum = likedTrack.strAlbum,
                strGenre = likedTrack.strGenre,
                strStyle = likedTrack.strStyle,
                intDuration = likedTrack.intDuration,
                strMusicVidCompany = likedTrack.strMusicVidCompany,
                strDescriptionEN = likedTrack.strDescriptionEN,
                strTrackThumb = likedTrack.strTrackThumb,
                IsLiked = likedTrack.IsLiked
            };

            _context.Tracks.Add(newTrack);
            _context.SaveChanges();

            TempData["Message"] = $"{likedTrack.strTrack} has been liked!";
            return RedirectToAction("TracksDataGet", new { artist_name = likedTrack.strArtist, track_name = likedTrack.strTrack });
        }


        [HttpPost]
        public IActionResult UnlikeTrack(int idTrack, string artistName, string trackName)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Find the liked track entry
            var likedTrack = _context.Tracks
                .FirstOrDefault(t => t.idTrack == idTrack && t.userId == userId);

            if (likedTrack != null)
            {
                _context.Tracks.Remove(likedTrack);
                _context.SaveChanges();

                TempData["Message"] = $"{likedTrack.strTrack} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Track not found in your liked list.";
            }

            return RedirectToAction("TracksDataGet", new { artist_name = artistName, track_name = trackName });
        }

        [HttpPost]
        public IActionResult UnlikeArtistFromUserProfile(int idArtist)
        {
            var userId = User.Identity.Name; // Assuming user authentication is set up
            if (userId == null)
                return Unauthorized();

            // Find the artist entry for the current user
            var likedArtist = _context.Artists
                .FirstOrDefault(a => a.idArtist == idArtist && a.userId == userId);

            if (likedArtist != null)
            {
                // Remove the artist from the database
                _context.Artists.Remove(likedArtist);
                _context.SaveChanges();

                TempData["Message"] = $"{likedArtist.strArtist} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Artist not found in your liked list.";
            }


            // Redirect back to the ArtistData view
            return RedirectToAction("UserProfile");
        }

        [HttpPost]
        public IActionResult UnlikeAlbumFromUserProfile(int idAlbum, string activeTab)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Find the liked album entry
            var likedAlbum = _context.Albums
                .FirstOrDefault(a => a.idAlbum == idAlbum && a.userId == userId);

            if (likedAlbum != null)
            {
                _context.Albums.Remove(likedAlbum);
                _context.SaveChanges();

                TempData["Message"] = $"{likedAlbum.strAlbum} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Album not found in your liked list.";
            }

            // Redirect to the correct tab (artists or albums)
            return RedirectToAction("UserProfile", new { activeTab = activeTab });
        }

        [HttpPost]
        public IActionResult UnlikeTrackFromUserProfile(int idTrack, string artistName, string trackName, string activeTab)
        {
            var userId = User.Identity.Name;
            if (userId == null)
                return Unauthorized();

            // Find the liked track entry
            var likedTrack = _context.Tracks
                .FirstOrDefault(t => t.idTrack == idTrack && t.userId == userId);

            if (likedTrack != null)
            {
                _context.Tracks.Remove(likedTrack);
                _context.SaveChanges();

                TempData["Message"] = $"{likedTrack.strTrack} has been unliked!";
            }
            else
            {
                TempData["Message"] = "Track not found in your liked list.";
            }

            // Pass the activeTab value to maintain the current tab
            return RedirectToAction("UserProfile", new { activeTab });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
