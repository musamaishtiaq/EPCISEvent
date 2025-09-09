using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EPCISEvent.MasterData;
using EPCISEvent.MasterData.SupportingClasses;

namespace EPCISEvent.Controllers
{
    public class LocationIdentifiersController : Controller
    {
        private readonly MasterDataContext _context;

        public LocationIdentifiersController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: LocationIdentifiers
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.LocationIdentifiers.Include(l => l.Location);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: LocationIdentifiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationIdentifier = await _context.LocationIdentifiers
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationIdentifier == null)
            {
                return NotFound();
            }

            return View(locationIdentifier);
        }

        // GET: LocationIdentifiers/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id");
            return View();
        }

        // POST: LocationIdentifiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identifier,IdentifierType,Description,LocationId")] LocationIdentifier locationIdentifier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationIdentifier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", locationIdentifier.LocationId);
            return View(locationIdentifier);
        }

        // GET: LocationIdentifiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationIdentifier = await _context.LocationIdentifiers.FindAsync(id);
            if (locationIdentifier == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", locationIdentifier.LocationId);
            return View(locationIdentifier);
        }

        // POST: LocationIdentifiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identifier,IdentifierType,Description,LocationId")] LocationIdentifier locationIdentifier)
        {
            if (id != locationIdentifier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationIdentifier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationIdentifierExists(locationIdentifier.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", locationIdentifier.LocationId);
            return View(locationIdentifier);
        }

        // GET: LocationIdentifiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationIdentifier = await _context.LocationIdentifiers
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationIdentifier == null)
            {
                return NotFound();
            }

            return View(locationIdentifier);
        }

        // POST: LocationIdentifiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationIdentifier = await _context.LocationIdentifiers.FindAsync(id);
            if (locationIdentifier != null)
            {
                _context.LocationIdentifiers.Remove(locationIdentifier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationIdentifierExists(int id)
        {
            return _context.LocationIdentifiers.Any(e => e.Id == id);
        }
    }
}
