using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EPCISEvent.MasterData;
using EPCISEvent.MasterData.MainClasses;

namespace EPCISEvent.Controllers
{
    public class TradeItemsController : Controller
    {
        private readonly MasterDataContext _context;

        public TradeItemsController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: TradeItems
        public async Task<IActionResult> Index()
        {
            var masterDataContext = _context.TradeItems.Include(t => t.Company);
            return View(await masterDataContext.ToListAsync());
        }

        // GET: TradeItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems
                .Include(t => t.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        // GET: TradeItems/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
            return View();
        }

        // POST: TradeItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GTIN14,NDC,NdcPattern,CompanyId,AdditionalId,AdditionalIdTypeCode,DescriptionShort,DosageFormType,FunctionalName,ManufacturerName,NetContentDescription,LabelDescription,RegulatedProductName,StrengthDescription,TradeItemDescription,SerialNumberLength,PackCount,CountryOfOrigin,DrainedWeight,DrainedWeightUom,GrossWeight,GrossWeightUom,NetWeight,NetWeightUom,PackageUom")] TradeItem tradeItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", tradeItem.CompanyId);
            return View(tradeItem);
        }

        // GET: TradeItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.FindAsync(id);
            if (tradeItem == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", tradeItem.CompanyId);
            return View(tradeItem);
        }

        // POST: TradeItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GTIN14,NDC,NdcPattern,CompanyId,AdditionalId,AdditionalIdTypeCode,DescriptionShort,DosageFormType,FunctionalName,ManufacturerName,NetContentDescription,LabelDescription,RegulatedProductName,StrengthDescription,TradeItemDescription,SerialNumberLength,PackCount,CountryOfOrigin,DrainedWeight,DrainedWeightUom,GrossWeight,GrossWeightUom,NetWeight,NetWeightUom,PackageUom")] TradeItem tradeItem)
        {
            if (id != tradeItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeItemExists(tradeItem.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", tradeItem.CompanyId);
            return View(tradeItem);
        }

        // GET: TradeItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems
                .Include(t => t.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        // POST: TradeItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeItem = await _context.TradeItems.FindAsync(id);
            if (tradeItem != null)
            {
                _context.TradeItems.Remove(tradeItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradeItemExists(int id)
        {
            return _context.TradeItems.Any(e => e.Id == id);
        }
    }
}
