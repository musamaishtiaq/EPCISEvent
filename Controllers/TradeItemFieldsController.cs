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
    public class TradeItemFieldsController : Controller
    {
        private readonly MasterDataContext _context;

        public TradeItemFieldsController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: TradeItemFields
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.TradeItemFields.Include(t => t.TradeItem);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: TradeItemFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItemField = await _context.TradeItemFields
                .Include(t => t.TradeItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItemField == null)
            {
                return NotFound();
            }

            return View(tradeItemField);
        }

        // GET: TradeItemFields/Create
        public IActionResult Create()
        {
            ViewData["TradeItemId"] = new SelectList(_context.TradeItems, "Id", "GTIN14");
            return View();
        }

        // POST: TradeItemFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TradeItemId,Id,Name,Value,Description")] TradeItemField tradeItemField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradeItemField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TradeItemId"] = new SelectList(_context.TradeItems, "Id", "GTIN14", tradeItemField.TradeItemId);
            return View(tradeItemField);
        }

        // GET: TradeItemFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItemField = await _context.TradeItemFields.FindAsync(id);
            if (tradeItemField == null)
            {
                return NotFound();
            }
            ViewData["TradeItemId"] = new SelectList(_context.TradeItems, "Id", "GTIN14", tradeItemField.TradeItemId);
            return View(tradeItemField);
        }

        // POST: TradeItemFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TradeItemId,Id,Name,Value,Description")] TradeItemField tradeItemField)
        {
            if (id != tradeItemField.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradeItemField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeItemFieldExists(tradeItemField.Id))
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
            ViewData["TradeItemId"] = new SelectList(_context.TradeItems, "Id", "GTIN14", tradeItemField.TradeItemId);
            return View(tradeItemField);
        }

        // GET: TradeItemFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItemField = await _context.TradeItemFields
                .Include(t => t.TradeItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItemField == null)
            {
                return NotFound();
            }

            return View(tradeItemField);
        }

        // POST: TradeItemFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeItemField = await _context.TradeItemFields.FindAsync(id);
            if (tradeItemField != null)
            {
                _context.TradeItemFields.Remove(tradeItemField);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradeItemFieldExists(int id)
        {
            return _context.TradeItemFields.Any(e => e.Id == id);
        }
    }
}
