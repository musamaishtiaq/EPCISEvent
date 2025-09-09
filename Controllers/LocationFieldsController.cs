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
    public class LocationFieldsController : Controller
    {
        private readonly MasterDataContext _context;

        public LocationFieldsController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: LocationFields
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.LocationFields.Include(l => l.Location);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: LocationFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationField = await _context.LocationFields
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationField == null)
            {
                return NotFound();
            }

            return View(locationField);
        }

        // GET: LocationFields/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: LocationFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,Id,Name,Value,Description")] LocationField locationField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", locationField.LocationId);
            return View(locationField);
        }

        // GET: LocationFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationField = await _context.LocationFields.FindAsync(id);
            if (locationField == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", locationField.LocationId);
            return View(locationField);
        }

        // POST: LocationFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Id,Name,Value,Description")] LocationField locationField)
        {
            if (id != locationField.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationFieldExists(locationField.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", locationField.LocationId);
            return View(locationField);
        }

        // GET: LocationFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationField = await _context.LocationFields
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationField == null)
            {
                return NotFound();
            }

            return View(locationField);
        }

        // POST: LocationFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationField = await _context.LocationFields.FindAsync(id);
            if (locationField != null)
            {
                _context.LocationFields.Remove(locationField);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationFieldExists(int id)
        {
            return _context.LocationFields.Any(e => e.Id == id);
        }
    }
}
