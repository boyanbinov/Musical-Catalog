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
    public class AdminArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminArtistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminArtists
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminArtist.ToListAsync());
        }

        // GET: AdminArtists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminArtist = await _context.AdminArtist
                .FirstOrDefaultAsync(m => m.idArtist == id);
            if (adminArtist == null)
            {
                return NotFound();
            }

            return View(adminArtist);
        }

        // GET: AdminArtists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminArtists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idArtist,strArtist,strLabel,idLabel,intFormedYear,intBornYear,strStyle,strGenre,strBiographyEN,strGender,intMembers,strCountry,strArtistThumb")] AdminArtist adminArtist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminArtist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminArtist);
        }

        // GET: AdminArtists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminArtist = await _context.AdminArtist.FindAsync(id);
            if (adminArtist == null)
            {
                return NotFound();
            }
            return View(adminArtist);
        }

        // POST: AdminArtists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idArtist,strArtist,strLabel,idLabel,intFormedYear,intBornYear,strStyle,strGenre,strBiographyEN,strGender,intMembers,strCountry,strArtistThumb")] AdminArtist adminArtist)
        {
            if (id != adminArtist.idArtist)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminArtist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminArtistExists(adminArtist.idArtist))
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
            return View(adminArtist);
        }

        // GET: AdminArtists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminArtist = await _context.AdminArtist
                .FirstOrDefaultAsync(m => m.idArtist == id);
            if (adminArtist == null)
            {
                return NotFound();
            }

            return View(adminArtist);
        }

        // POST: AdminArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminArtist = await _context.AdminArtist.FindAsync(id);
            if (adminArtist != null)
            {
                _context.AdminArtist.Remove(adminArtist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminArtistExists(int id)
        {
            return _context.AdminArtist.Any(e => e.idArtist == id);
        }
    }
}
