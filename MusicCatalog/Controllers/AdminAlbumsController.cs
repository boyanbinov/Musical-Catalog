using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Data;
using MusicCatalog.Models;
using MusicCatalogNoAPI_Test.Models;

namespace MusicCatalog.Controllers
{
    public class AdminAlbumsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminAlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AdminAlbum.Include(a => a.Artist);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.AdminAlbum
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.idAlbum == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["idArtist"] = new SelectList(_context.AdminArtist, "idArtist", "strArtist");
            return View();
        }

        // POST: Albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idArtist, idLabel, strAlbum, strArtist, intYearReleased, strStyle, strGenre, strLabel, strReleaseFormat, strAlbumThumb, strDescriptionEN")] AdminAlbum album)
        {
            if (ModelState.IsValid)
            {
                // Find the artist by the selected idArtist
                var artist = await _context.AdminArtist.FindAsync(album.idArtist);
                if (artist != null)
                {
                    // Populate strArtist with the artist's name
                    album.strArtist = artist.strArtist; // Ensure strArtist gets populated correctly
                }

                // Add the album to the context and save changes
                _context.Add(album);
                await _context.SaveChangesAsync();

                // Redirect to the Index action
                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, repopulate the dropdown and return the view
            ViewData["idArtist"] = new SelectList(_context.AdminArtist, "idArtist", "strArtist", album.idArtist);
            return View(album);
        }





        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.AdminAlbum.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["idArtist"] = new SelectList(_context.AdminArtist, "idArtist", "idArtist", album.idArtist);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idAlbum,idArtist,idLabel,strAlbum,strArtist,intYearReleased,strStyle,strGenre,strLabel,strReleaseFormat,strAlbumThumb,strDescriptionEN")] AdminAlbum album)
        {
            if (id != album.idAlbum)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.idAlbum))
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
            ViewData["idArtist"] = new SelectList(_context.AdminArtist, "idArtist", "idArtist", album.idArtist);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.AdminAlbum
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.idAlbum == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.AdminAlbum.FindAsync(id);
            if (album != null)
            {
                _context.AdminAlbum.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.AdminAlbum.Any(e => e.idAlbum == id);
        }
    }
}

