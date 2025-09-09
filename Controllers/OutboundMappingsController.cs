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
    public class OutboundMappingsController : Controller
    {
        private readonly MasterDataContext _context;

        public OutboundMappingsController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: OutboundMappings
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.OutboundMappings.Include(o => o.Company).Include(o => o.FromBusiness).Include(o => o.ShipFrom).Include(o => o.ShipTo).Include(o => o.ToBusiness);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: OutboundMappings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outboundMapping = await _context.OutboundMappings
                .Include(o => o.Company)
                .Include(o => o.FromBusiness)
                .Include(o => o.ShipFrom)
                .Include(o => o.ShipTo)
                .Include(o => o.ToBusiness)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outboundMapping == null)
            {
                return NotFound();
            }

            return View(outboundMapping);
        }

        // GET: OutboundMappings/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["FromBusinessId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["ShipFromId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["ShipToId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["ToBusinessId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: OutboundMappings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,FromBusinessId,ShipFromId,ToBusinessId,ShipToId")] OutboundMapping outboundMapping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outboundMapping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.CompanyId);
            ViewData["FromBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.FromBusinessId);
            ViewData["ShipFromId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipFromId);
            ViewData["ShipToId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipToId);
            ViewData["ToBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.ToBusinessId);
            return View(outboundMapping);
        }

        // GET: OutboundMappings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outboundMapping = await _context.OutboundMappings.FindAsync(id);
            if (outboundMapping == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.CompanyId);
            ViewData["FromBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.FromBusinessId);
            ViewData["ShipFromId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipFromId);
            ViewData["ShipToId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipToId);
            ViewData["ToBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.ToBusinessId);
            return View(outboundMapping);
        }

        // POST: OutboundMappings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,FromBusinessId,ShipFromId,ToBusinessId,ShipToId")] OutboundMapping outboundMapping)
        {
            if (id != outboundMapping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outboundMapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutboundMappingExists(outboundMapping.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.CompanyId);
            ViewData["FromBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.FromBusinessId);
            ViewData["ShipFromId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipFromId);
            ViewData["ShipToId"] = new SelectList(_context.Locations, "Id", "Name", outboundMapping.ShipToId);
            ViewData["ToBusinessId"] = new SelectList(_context.Companies, "Id", "Name", outboundMapping.ToBusinessId);
            return View(outboundMapping);
        }

        // GET: OutboundMappings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outboundMapping = await _context.OutboundMappings
                .Include(o => o.Company)
                .Include(o => o.FromBusiness)
                .Include(o => o.ShipFrom)
                .Include(o => o.ShipTo)
                .Include(o => o.ToBusiness)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outboundMapping == null)
            {
                return NotFound();
            }

            return View(outboundMapping);
        }

        // POST: OutboundMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outboundMapping = await _context.OutboundMappings.FindAsync(id);
            if (outboundMapping != null)
            {
                _context.OutboundMappings.Remove(outboundMapping);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutboundMappingExists(int id)
        {
            return _context.OutboundMappings.Any(e => e.Id == id);
        }
    }
}
