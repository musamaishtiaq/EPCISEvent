using EPCISEvent.Fastnt.CBV;
using EPCISEvent.MasterData;
using EPCISEvent.MasterData.MainClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPCISEvent.Controllers
{
    public class LocationsController : Controller
    {
        private readonly MasterDataContext _context;

        public LocationsController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.Locations.Include(l => l.Company).Include(l => l.LocationType).Include(l => l.Site);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Company)
                .Include(l => l.LocationType)
                .Include(l => l.Site)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        public static SelectList GetSubSiteTypeList()
        {
            return new SelectList(Enum.GetValues(typeof(SubSiteTypeMasterDataAttribute))
                .Cast<SubSiteTypeMasterDataAttribute>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetSubSiteAttributesList()
        {
            return new SelectList(Enum.GetValues(typeof(SubSiteAttributesMasterDataAttribute))
                .Cast<SubSiteAttributesMasterDataAttribute>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Identifier");
            ViewData["SiteId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,SiteId,LocationTypeId,SST,SSA,GLN13,SGLN,Name,Address1,Address2,Address3,Country,City,StateProvince,PostalCode,Latitude,Longitude")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Identifier", location.LocationTypeId);
            ViewData["SiteId"] = new SelectList(_context.Locations, "Id", "Name", location.SiteId);
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Identifier", location.LocationTypeId);
            ViewData["SiteId"] = new SelectList(_context.Locations, "Id", "Name", location.SiteId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,SiteId,LocationTypeId,SST,SSA,GLN13,SGLN,Name,Address1,Address2,Address3,Country,City,StateProvince,PostalCode,Latitude,Longitude")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Identifier", location.LocationTypeId);
            ViewData["SiteId"] = new SelectList(_context.Locations, "Id", "Name", location.SiteId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Company)
                .Include(l => l.LocationType)
                .Include(l => l.Site)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
