using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Data;
using MusicCatalog.Models;

namespace MusicCatalog.Controllers
{
    public class AdminTracksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminTracksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AdminTrack.Include(t => t.Album);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.AdminTrack
                .Include(t => t.Album)
                .FirstOrDefaultAsync(m => m.idTrack == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        public IActionResult Create()
        {
            var albums = _context.AdminAlbum.Include(a => a.Artist)
                .Select(a => new { a.idAlbum, a.strAlbum, a.Artist.strArtist })
                .ToList();

            // Populate the album list with album names and artist names
            ViewData["idAlbum"] = new SelectList(albums, "idAlbum", "strAlbum");

            return View();
        }

        // POST: Tracks/Create
        // POST: Tracks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idAlbum,strTrack,strArtist,intDuration,strGenre,strStyle,strDescriptionEN,strTrackThumb,strMusicVidCompany")] AdminTrack track)
        {
            if (ModelState.IsValid)
            {
                // Fetch the album and its associated artist from the database based on the selected idAlbum
                var album = await _context.AdminAlbum.Include(a => a.Artist).FirstOrDefaultAsync(a => a.idAlbum == track.idAlbum);

                if (album != null)
                {
                    track.strAlbum = album.strAlbum; // Set the album name
                    track.strArtist = album.Artist.strArtist; // Set the artist name

                    // Set the artist id from the selected album's artist
                    track.idArtist = album.Artist.idArtist; // Set idArtist from the album's artist
                }

                // Add the track to the database and save changes
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, re-populate the album dropdown list
            var albums = _context.AdminAlbum.Include(a => a.Artist)
                .Select(a => new { a.idAlbum, a.strAlbum, a.Artist.strArtist })
                .ToList();
            ViewData["idAlbum"] = new SelectList(albums, "idAlbum", "strAlbum", track.idAlbum);

            return View(track);
        }




        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.AdminTrack.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            ViewData["idAlbum"] = new SelectList(_context.AdminAlbum, "idAlbum", "idAlbum", track.idAlbum);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idTrack,idAlbum,idArtist,strTrack,strAlbum,strArtist,intDuration,strGenre,strStyle,strDescriptionEN,strTrackThumb,strMusicVidCompany")] AdminTrack track)
        {
            if (id != track.idTrack)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.idTrack))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["idAlbum"] = new SelectList(_context.AdminAlbum, "idAlbum", "idAlbum", track.idAlbum);
            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.AdminTrack
                .Include(t => t.Album)
                .FirstOrDefaultAsync(m => m.idTrack == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var track = await _context.AdminTrack.FindAsync(id);
            if (track != null)
            {
                _context.AdminTrack.Remove(track);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(int id)
        {
            return _context.AdminTrack.Any(e => e.idTrack == id);
        }
    }
}
