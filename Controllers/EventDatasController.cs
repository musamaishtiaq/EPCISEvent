using EPCISEvent.Extensions;
using EPCISEvent.Fastnt;
using EPCISEvent.Fastnt.CBV;
using EPCISEvent.MasterData;
using EPCISEvent.MasterData.MainClasses;
using EPCISEvent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Action = EPCISEvent.Fastnt.Action;

namespace EPCISEvent.Controllers
{
    public class EventDatasController : Controller
    {
        private readonly MasterDataContext _context;

        public EventDatasController(MasterDataContext context)
        {
            _context = context;
        }

        // GET: EventDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventDatas.ToListAsync());
        }

        // GET: EventDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventData = await _context.EventDatas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventData == null)
            {
                return NotFound();
            }

            return View(eventData);
        }

        // GET: EventDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventType,EventLog,CreatedAt")] EventData eventData)
        {
            eventData.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(eventData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventData);
        }

        // GET: EventDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventData = await _context.EventDatas.FindAsync(id);
            if (eventData == null)
            {
                return NotFound();
            }
            var model = new ObjectEventViewModel
            {
                Id = eventData.Id,
                EventType = Enum.TryParse<EventType>(eventData.EventType, out var eventType) ? eventType : default,
                EventTime = DateTime.UtcNow,
                EventTimeZoneOffset = "+00:00"
            };

            ViewBag.Companies = GetCompanyList();
            return View($"Add{eventData.EventType}", model);
        }

        private List<Destination2> GetDestinationList(ObjectEventViewModel model)
        {
            var destinations = new List<Destination2>();
            if (!string.IsNullOrEmpty(model.DestinationLocationId))
            {
                destinations.Add(new Destination2
                {
                    Type = SourceDestinationTypes.OwningParty.GetEnumMemberValue(),
                    Destination = EPCISUtilities.Gln13ToSglnUrn(model.DestinationLocationId),
                });
            }
            if (!string.IsNullOrEmpty(model.DestinationSubLocationId))
            {
                destinations.Add(new Destination2
                {
                    Type = SourceDestinationTypes.Location.GetEnumMemberValue(),
                    Destination = EPCISUtilities.Gln13ToSglnUrn(model.DestinationSubLocationId),
                });
            }
            return destinations;
        }

        private List<Source2> GetSourceList(ObjectEventViewModel model)
        {
            var sources = new List<Source2>();
            if (!string.IsNullOrEmpty(model.BizLocationId))
            {
                sources.Add(new Source2
                {
                    Type = SourceDestinationTypes.PossessingParty.GetEnumMemberValue(),
                    Source = EPCISUtilities.Gln13ToSglnUrn(model.BizLocationId),
                });
            }
            if (!string.IsNullOrEmpty(model.ReadPointId))
            {
                sources.Add(new Source2
                {
                    Type = SourceDestinationTypes.Location.GetEnumMemberValue(),
                    Source = EPCISUtilities.Gln13ToSglnUrn(model.ReadPointId),
                });
            }
            return sources;
        }

        private List<BusinessTransaction> GetBusinessTransactionList(ObjectEventViewModel model)
        {
            var bizTransactions = new List<BusinessTransaction>();
            foreach (var biz in model.BizTransactionList)
            {
                Enum.TryParse<BusinessTransactionType>(biz.Type, out var businessTransactionType);
                bizTransactions.Add(new BusinessTransaction(EPCISUtilities.BusinessTransactionToBtUrn(biz.BizTransaction), businessTransactionType.GetEnumMemberValue()));
            }
            return bizTransactions;
        }

        private async Task<EPCISHeader> GenerateMasterDataAsync(ObjectEventViewModel model, List<BusinessTransaction> bizTransactions)
        {
            var dataList = new List<string>();

            var masterData = new EPCISHeader
            {
                EpcisMasterData = new EPCISMasterData
                {
                    VocabularyList = new List<Vocabulary>()
                }
            };

            // Add EPC Class vocabulary (products)
            var epcVocabulary = new Vocabulary
            {
                Type = "urn:epcglobal:epcis:vtype:EPCClass",
                VocabularyElementList = new List<VocabularyElement>()
            };

            foreach (var epc in model.EpcList)
            {
                var parts = epc.Split(':').Last().Split('.');
                var gtin = $"{parts[0]}{parts[1]}";
                if (!dataList.Contains(gtin))
                {
                    dataList.Add(gtin);
                    var product = await _context.TradeItems.FirstOrDefaultAsync(ti => ti.GTIN14.StartsWith(gtin));
                    epcVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = epc,
                        Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "productName", Attribute = product.RegulatedProductName },
                        new VocabularyAttribute { Id = "gtin", Attribute = product.GTIN14 },
                        new VocabularyAttribute { Id = "brand", Attribute = product.ManufacturerName },
                        new VocabularyAttribute { Id = "description", Attribute = product.DescriptionShort }
                    }
                    });
                }
            }

            // Add Location vocabulary
            var locationVocabulary = new Vocabulary
            {
                Type = "urn:epcglobal:epcis:vtype:Location",
                VocabularyElementList = new List<VocabularyElement>()
            };

            // Add readPoint location
            if (!string.IsNullOrEmpty(model.ReadPointId))
            {
                if (!dataList.Contains(model.ReadPointId))
                {
                    dataList.Add(model.ReadPointId);
                    var readPoint = await _context.Locations.FirstOrDefaultAsync(l => l.SGLN == model.ReadPointId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(model.ReadPointId),
                        Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "name", Attribute = readPoint.Name },
                        new VocabularyAttribute { Id = "gln", Attribute = readPoint.GLN13 },
                        new VocabularyAttribute { Id = "address", Attribute = readPoint.Address1 }
                    }
                    });
                }
            }

            // Add bizLocation
            if (!string.IsNullOrEmpty(model.BizLocationId))
            {
                if (!dataList.Contains(model.BizLocationId))
                {
                    dataList.Add(model.BizLocationId);
                    var bizLocation = await _context.Companies.FirstOrDefaultAsync(c => c.SGLN == model.BizLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(model.BizLocationId),
                        Attributes = new List<VocabularyAttribute>
                        {
                            new VocabularyAttribute { Id = "name", Attribute = bizLocation.Name },
                            new VocabularyAttribute { Id = "gln", Attribute = bizLocation.GLN13 },
                            new VocabularyAttribute { Id = "address", Attribute = bizLocation.Address1 }
                        }
                    });
                }
            }

            // Add destination locations
            if (!string.IsNullOrEmpty(model.DestinationLocationId))
            {
                if (!dataList.Contains(model.DestinationLocationId))
                {
                    dataList.Add(model.DestinationLocationId);
                    var destLocation = await _context.Companies.FirstOrDefaultAsync(c => c.SGLN == model.DestinationLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(model.DestinationLocationId),
                        Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "name", Attribute = destLocation.Name },
                        new VocabularyAttribute { Id = "gln", Attribute = destLocation.GLN13 },
                        new VocabularyAttribute { Id = "address", Attribute = destLocation.Address1 }
                    }
                    });
                }
            }

            if (!string.IsNullOrEmpty(model.DestinationSubLocationId))
            {
                if (!dataList.Contains(model.DestinationSubLocationId))
                {
                    dataList.Add(model.DestinationSubLocationId);
                    var destSubLocation = await _context.Locations.FirstOrDefaultAsync(l => l.SGLN == model.DestinationSubLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(model.DestinationSubLocationId),
                        Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "name", Attribute = destSubLocation.Name },
                        new VocabularyAttribute { Id = "gln", Attribute = destSubLocation.GLN13 },
                        new VocabularyAttribute { Id = "address", Attribute = destSubLocation.Address1 }
                    }
                    });
                }
            }

            // Add Business Transaction vocabulary
            var bizTransVocabulary = new Vocabulary
            {
                Type = "urn:epcglobal:epcis:vtype:BusinessTransaction",
                VocabularyElementList = new List<VocabularyElement>()
            };

            foreach (var bizTrans in bizTransactions)
            {
                //var bizTransDetails = _bizTransService.GetBusinessTransactionDetails(bizTrans.BizTransaction);
                bizTransVocabulary.VocabularyElementList.Add(new VocabularyElement
                {
                    Id = bizTrans.BizTransaction,
                    Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "type", Attribute = bizTrans.Type },
                        new VocabularyAttribute { Id = "documentDate", Attribute = DateTime.Now.ToString("yyyy-MM-dd") }
                    }
                });
            }

            //// Add Business Step and Disposition vocabularies
            //var bizStepVocabulary = new Vocabulary
            //{
            //    Type = "urn:epcglobal:epcis:vtype:BusinessStep",
            //    VocabularyElementList = new List<VocabularyElement>
            //    {
            //        new VocabularyElement
            //        {
            //            Id = model.BizStep.GetEnumMemberValue(),
            //            Attributes = new List<VocabularyAttribute>
            //            {
            //                new VocabularyAttribute { Id = "description", Attribute = "Business step description" }
            //            }
            //        }
            //    }
            //};

            //var dispositionVocabulary = new Vocabulary
            //{
            //    Type = "urn:epcglobal:epcis:vtype:Disposition",
            //    VocabularyElementList = new List<VocabularyElement>
            //    {
            //        new VocabularyElement
            //        {
            //            Id = model.Disposition.GetEnumMemberValue(),
            //            Attributes = new List<VocabularyAttribute>
            //            {
            //                new VocabularyAttribute { Id = "description", Attribute = "Disposition description" }
            //            }
            //        }
            //    }
            //};

            // Add all vocabularies to master data
            masterData.EpcisMasterData.VocabularyList.Add(epcVocabulary);
            masterData.EpcisMasterData.VocabularyList.Add(locationVocabulary);
            masterData.EpcisMasterData.VocabularyList.Add(bizTransVocabulary);
            //masterData.EpcisMasterData.VocabularyList.Add(bizStepVocabulary);
            //masterData.EpcisMasterData.VocabularyList.Add(dispositionVocabulary);

            return masterData;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditObjectEvent(int id, ObjectEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var destinations = GetDestinationList(model);
                    var sources = GetSourceList(model);
                    var bizTransactions = GetBusinessTransactionList(model);

                    // Generate master data
                    var masterData = await GenerateMasterDataAsync(model, bizTransactions);

                    // Convert view model to ObjectEvent2
                    var objectEvent = new ObjectEvent2
                    {
                        Type = model.EventType.ToString(),
                        EventTime = model.EventTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        EventTimeZoneOffset = model.EventTimeZoneOffset,
                        BizStep = model.BizStep.GetEnumMemberValue().Split(':').Last(),
                        Disposition = model.Disposition.GetEnumMemberValue().Split(':').Last(),
                        ReadPoint = model.ReadPointId != null ? new IdWrapper { Id = EPCISUtilities.Gln13ToSglnUrn(model.ReadPointId) } : null,
                        BizLocation = model.BizLocationId != null ? new IdWrapper { Id = EPCISUtilities.Gln13ToSglnUrn(model.BizLocationId) } : null,
                        Action = model.Action.GetEnumMemberValue(),
                        EpcList = model.EpcList,
                        QuantityList = model.QuantityList,
                        Ilmd = model.Ilmd,
                        BizTransactionList = bizTransactions,
                        SourceList = sources,
                        DestinationList = destinations
                    };

                    // Create complete EPCIS document
                    var epcisDocument = new EPCISDocument2
                    {
                        Type = "EPCISDocument",
                        SchemaVersion = "2.0",
                        CreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        EpcisHeader = masterData,
                        EpcisBody = new EPCISBody
                        {
                            EventList = new List<EPCISEvent2> { objectEvent }
                        },
                        Context = new List<object>
                        {
                            "https://ref.gs1.org/standards/epcis/2.0.0/epcis-context.jsonld",
                            {
                                new
                                {
                                    example = "http://ns.example.com/epcis/",
                                    epcis = "urn:epcglobal:epcis:",
                                    cbv = "urn:epcglobal:cbv:"
                                }
                            }
                        }
                    };

                    // Serialize and process the document
                    var encoder = new JsonEncoder();
                    var json = encoder.Encode(epcisDocument);

                    ViewBag.Companies = GetCompanyList();
                    return View($"Add{model.EventType}", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating event: {ex.Message}");
                }
            }

            // If we got this far, something failed; redisplay form
            return View(model);
        }

        public SelectList GetCompanyList()
        {
            var companies = _context.Companies
                .Select(l => new { l.SGLN, l.Name })
                .ToList();
            return new SelectList(companies, "SGLN", "Name");
        }

        public JsonResult GetLocationsByCompany(string companySGLN)
        {
            var locations = _context.Locations
                .Where(l => l.Company.SGLN == companySGLN)
                .Select(l => new { l.SGLN, l.LocationType.Identifier })
                .ToList();

            return Json(locations);
        }

        public static SelectList GetActionSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(Action))
                .Cast<Action>()
                .Select(a => new { Value = a, Text = a.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetEventTypeSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(EventType))
                .Cast<EventType>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetBusinessStepSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(BusinessSteps))
                .Cast<BusinessSteps>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetBusinessTransactionTypeList()
        {
            return new SelectList(Enum.GetValues(typeof(BusinessTransactionType))
                .Cast<BusinessTransactionType>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetDispositionSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(Disposition))
                .Cast<Disposition>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetErrorReasonList()
        {
            return new SelectList(Enum.GetValues(typeof(ErrorReason))
                .Cast<ErrorReason>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        // POST: EventDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventType,EventLog,CreatedAt")] EventData eventData)
        {
            if (id != eventData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventDataExists(eventData.Id))
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
            return View(eventData);
        }

        // GET: EventDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventData = await _context.EventDatas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventData == null)
            {
                return NotFound();
            }

            return View(eventData);
        }

        // POST: EventDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventData = await _context.EventDatas.FindAsync(id);
            if (eventData != null)
            {
                _context.EventDatas.Remove(eventData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventDataExists(int id)
        {
            return _context.EventDatas.Any(e => e.Id == id);
        }
    }
}
