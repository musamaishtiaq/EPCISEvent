using EPCISEvent.Extensions;
using EPCISEvent.Fastnt;
using EPCISEvent.Fastnt.CBV;
using EPCISEvent.Helpers;
using EPCISEvent.Interfaces;
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
        private readonly IFastntApi _fastntApi;


        public EventDatasController(MasterDataContext context, IFastntApi fastntApi)
        {
            _context = context;
            _fastntApi = fastntApi;
        }

        // GET: EventDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventDatas.OrderByDescending(e => e.CreatedAt).ToListAsync());
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
            // Convert string to enum safely (case-insensitive)
            if (!Enum.TryParse<BusinessEventType>(eventData.EventType, true, out var businessEvent))
                throw new Exception($"Unknown business event type: {eventData.EventType}");

            var eventType = GetEventType(businessEvent);
            ViewBag.Companies = GetCompanyList();
            ViewBag.Dispositions = GetDispositionTypes(businessEvent);
            if (eventType == EventType.ObjectEvent)
            {
                var model = new ObjectEventViewModel
                {
                    Id = eventData.Id,
                    EventType = eventType,
                    EventTime = DateTime.UtcNow,
                    EventTimeZoneOffset = "+00:00",
                    Action = GetActionForBusinessEvent(businessEvent),
                    BizStep = GetBizStepForBusinessEvent(businessEvent)
                };
                return View($"Add{eventType}", model);
            }
            else
            {
                var model = new AggregationEventViewModel
                {
                    Id = eventData.Id,
                    EventType = eventType,
                    EventTime = DateTime.UtcNow,
                    EventTimeZoneOffset = "+00:00",
                    Action = GetActionForBusinessEvent(businessEvent),
                    BizStep = GetBizStepForBusinessEvent(businessEvent)
                };
                return View($"Add{eventType}", model);
            }
        }

        [HttpPost]
        public IActionResult ConvertEpc([FromBody] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Json(input);

            if (input.StartsWith("]d2"))
                input = input.Substring(3);

            if (input.StartsWith("00"))
            {
                string company = input.Substring(2, 11);
                string serial = input.Substring(13, 6);

                string sscc = $"urn:epc:id:sscc:{company}.{serial}";
                return Json(sscc);
            }

            if (input.StartsWith("01"))
                input = input.Substring(2);

            string gtin = input.Substring(0, 14);
            string afterGtin = input.Substring(14);
            if (afterGtin.StartsWith("21"))
            {
                afterGtin = afterGtin.Substring(2);
            }
            else
            {
                return Json($"ERROR: Missing 21 before serial for EPC {input}");
            }
            string serialDigits = new string(
                afterGtin
                    .TakeWhile(char.IsDigit)
                    .Take(20)
                    .ToArray()
            );


            string companyPrefix = gtin.Substring(0, 11);
            string itemRef = gtin.Substring(11, 2);
            string sgtin = $"urn:epc:id:sgtin:{companyPrefix}.{itemRef}.{serialDigits}";

            return Json(sgtin);
        }

        public EventType GetEventType(BusinessEventType bEvent)
        {
            return bEvent switch
            {
                BusinessEventType.Commissioning => EventType.ObjectEvent,
                BusinessEventType.Packing => EventType.AggregationEvent,
                BusinessEventType.Shipping => EventType.ObjectEvent,
                BusinessEventType.Receiving => EventType.ObjectEvent,
                BusinessEventType.Unpacking => EventType.AggregationEvent,
                BusinessEventType.VoidShipping => EventType.ObjectEvent,
                BusinessEventType.Inspection => EventType.ObjectEvent,
                BusinessEventType.Decommissioning => EventType.ObjectEvent,
                BusinessEventType.Dispensing => EventType.ObjectEvent,
                _ => throw new Exception("Not implemented")
            };
        }

        private List<Destination2> GetDestinationList(string DestinationLocationId, string DestinationSubLocationId)
        {
            var destinations = new List<Destination2>();
            if (!string.IsNullOrEmpty(DestinationLocationId))
            {
                destinations.Add(new Destination2
                {
                    Type = SourceDestinationTypes.OwningParty.GetEnumMemberValue().Split(':').Last(),
                    Destination = EPCISUtilities.Gln13ToSglnUrn(DestinationLocationId),
                });
            }
            if (!string.IsNullOrEmpty(DestinationSubLocationId))
            {
                destinations.Add(new Destination2
                {
                    Type = SourceDestinationTypes.Location.GetEnumMemberValue().Split(':').Last(),
                    Destination = EPCISUtilities.Gln13ToSglnUrn(DestinationSubLocationId),
                });
            }
            return destinations;
        }

        private List<Source2> GetSourceList(string BizLocationId, string ReadPointId)
        {
            var sources = new List<Source2>();
            if (!string.IsNullOrEmpty(BizLocationId))
            {
                sources.Add(new Source2
                {
                    Type = SourceDestinationTypes.PossessingParty.GetEnumMemberValue().Split(':').Last(),
                    Source = EPCISUtilities.Gln13ToSglnUrn(BizLocationId),
                });
            }
            if (!string.IsNullOrEmpty(ReadPointId))
            {
                sources.Add(new Source2
                {
                    Type = SourceDestinationTypes.Location.GetEnumMemberValue().Split(':').Last(),
                    Source = EPCISUtilities.Gln13ToSglnUrn(ReadPointId),
                });
            }
            return sources;
        }

        private List<BusinessTransaction> GetBusinessTransactionList(List<BusinessTransaction> BizTransactionList)
        {
            var bizTransactions = new List<BusinessTransaction>();
            foreach (var biz in BizTransactionList)
            {
                Enum.TryParse<BusinessTransactionType>(biz.Type, out var businessTransactionType);
                bizTransactions.Add(new BusinessTransaction(EPCISUtilities.BusinessTransactionToBtUrn(biz.BizTransaction), businessTransactionType.GetEnumMemberValue().Split(':').Last()));
            }
            return bizTransactions;
        }

        //private List<InstanceLotMasterDataAttribute> GetILMDAttributeList(ObjectEventViewModel model)
        //{
        //    var ilmdAttributes = new List<InstanceLotMasterDataAttribute>();
        //    foreach (var ilmd in model.BizTransactionList)
        //    {
        //        Enum.TryParse<IlmdAttribute>(ilmd.Type, out var ilmdType);
        //        ilmdAttributes.Add(new InstanceLotMasterDataAttribute(EPCISUtilities.BusinessTransactionToBtUrn(ilmd.), ilmdType.GetEnumMemberValue()));
        //    }
        //    return ilmdAttributes;
        //}

        private async Task<EPCISHeader> GenerateMasterDataAsync(List<string> EpcList, string ReadPointId, string BizLocationId, string DestinationLocationId, string DestinationSubLocationId, List<BusinessTransaction> bizTransactions)
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

            foreach (var epc in EpcList)
            {
                var parts = epc.Split(':').Last().Split('.');
                if (parts.Length > 2)
                {
                    var gtin = $"{parts[0]}{parts[1]}";
                    if (!dataList.Contains(gtin))
                    {
                        dataList.Add(gtin);
                        var product = await _context.TradeItems.FirstOrDefaultAsync(ti => ti.GTIN14.StartsWith(gtin));
                        if (product != null)
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
            }

            // Add Location vocabulary
            var locationVocabulary = new Vocabulary
            {
                Type = "urn:epcglobal:epcis:vtype:Location",
                VocabularyElementList = new List<VocabularyElement>()
            };

            // Add readPoint location
            if (!string.IsNullOrEmpty(ReadPointId))
            {
                if (!dataList.Contains(ReadPointId))
                {
                    dataList.Add(ReadPointId);
                    var readPoint = await _context.Locations.FirstOrDefaultAsync(l => l.SGLN == ReadPointId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(ReadPointId),
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
            if (!string.IsNullOrEmpty(BizLocationId))
            {
                if (!dataList.Contains(BizLocationId))
                {
                    dataList.Add(BizLocationId);
                    var bizLocation = await _context.Companies.FirstOrDefaultAsync(c => c.SGLN == BizLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(BizLocationId),
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
            if (!string.IsNullOrEmpty(DestinationLocationId))
            {
                if (!dataList.Contains(DestinationLocationId))
                {
                    dataList.Add(DestinationLocationId);
                    var destLocation = await _context.Companies.FirstOrDefaultAsync(c => c.SGLN == DestinationLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(DestinationLocationId),
                        Attributes = new List<VocabularyAttribute>
                    {
                        new VocabularyAttribute { Id = "name", Attribute = destLocation.Name },
                        new VocabularyAttribute { Id = "gln", Attribute = destLocation.GLN13 },
                        new VocabularyAttribute { Id = "address", Attribute = destLocation.Address1 }
                    }
                    });
                }
            }

            if (!string.IsNullOrEmpty(DestinationSubLocationId))
            {
                if (!dataList.Contains(DestinationSubLocationId))
                {
                    dataList.Add(DestinationSubLocationId);
                    var destSubLocation = await _context.Locations.FirstOrDefaultAsync(l => l.SGLN == DestinationSubLocationId);
                    locationVocabulary.VocabularyElementList.Add(new VocabularyElement
                    {
                        Id = EPCISUtilities.Gln13ToSglnUrn(DestinationSubLocationId),
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
            ViewBag.Companies = GetCompanyList();
            if (ModelState.IsValid)
            {
                try
                {
                    var destinations = GetDestinationList(model.DestinationLocationId, model.DestinationSubLocationId);
                    var sources = GetSourceList(model.BizLocationId, model.ReadPointId);
                    var bizTransactions = GetBusinessTransactionList(model.BizTransactionList);
                    var newIlmd = new Dictionary<string, string>();
                    foreach (var item in model.Ilmd)
                    {
                        if (string.IsNullOrWhiteSpace(item?.Name))
                            continue;
                        Enum.TryParse<IlmdAttribute>(item.Name, out var ilmdAttribute);
                        newIlmd[ilmdAttribute.GetEnumMemberValue()] = item.Value;
                    }

                    // Generate master data
                    var masterData = await GenerateMasterDataAsync(model.EpcList, model.ReadPointId, model.BizLocationId, model.DestinationLocationId, model.DestinationSubLocationId, bizTransactions);

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
                        QuantityList = model.QuantityList.Count > 0 ? model.QuantityList : null,
                        Ilmd = (newIlmd != null && newIlmd.Count > 0) ? newIlmd : null,
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

                    var eventInDb = await _context.EventDatas.FindAsync(id);
                    if (eventInDb == null)
                        return View($"Add{model.EventType}", model);

                    await _fastntApi.CaptureJsonEvent(json);
                    eventInDb.EventLog = json;
                    _context.Update(eventInDb);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating event: {ex.Message}");
                }
            }

            // If we got this far, something failed; redisplay form
            return View($"Add{model.EventType}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAggregationEvent(int id, AggregationEventViewModel model)
        {
            ViewBag.Companies = GetCompanyList();
            if (ModelState.IsValid)
            {
                try
                {
                    var destinations = GetDestinationList(model.DestinationLocationId, model.DestinationSubLocationId);
                    var sources = GetSourceList(model.BizLocationId, model.ReadPointId);
                    var bizTransactions = GetBusinessTransactionList(model.BizTransactionList);
                    
                    // Generate master data
                    var masterData = await GenerateMasterDataAsync(model.EpcList, model.ReadPointId, model.BizLocationId, model.DestinationLocationId, model.DestinationSubLocationId, bizTransactions);

                    var aggregationEvent = new AggregationEvent2
                    {
                        Type = model.EventType.ToString(),
                        EventTime = model.EventTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        EventTimeZoneOffset = model.EventTimeZoneOffset,
                        BizStep = model.BizStep.GetEnumMemberValue().Split(':').Last(),
                        Disposition = model.Disposition.GetEnumMemberValue().Split(':').Last(),
                        ReadPoint = model.ReadPointId != null ? new IdWrapper { Id = EPCISUtilities.Gln13ToSglnUrn(model.ReadPointId) } : null,
                        BizLocation = model.BizLocationId != null ? new IdWrapper { Id = EPCISUtilities.Gln13ToSglnUrn(model.BizLocationId) } : null,
                        Action = model.Action.GetEnumMemberValue(),
                        ParentId = model.ParentEpc,
                        ChildEpcs = model.EpcList,
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
                            EventList = new List<EPCISEvent2> { aggregationEvent }
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

                    var eventInDb = await _context.EventDatas.FindAsync(id);
                    if (eventInDb == null)
                        return View($"Add{model.EventType}", model);

                    await _fastntApi.CaptureJsonEvent(json);
                    eventInDb.EventLog = json;
                    _context.Update(eventInDb);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating event: {ex.Message}");
                }
            }

            // If we got this far, something failed; redisplay form
            return View($"Add{model.EventType}", model);
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

        public static SelectList GetBusinessEventTypeSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(BusinessEventType))
                .Cast<BusinessEventType>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public static SelectList GetIlmdAttributeSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(IlmdAttribute))
                .Cast<IlmdAttribute>()
                .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        public Action GetActionForBusinessEvent(BusinessEventType bEvent)
        {
            return bEvent switch
            {
                BusinessEventType.Commissioning => Action.Add,
                BusinessEventType.Packing => Action.Add,
                BusinessEventType.Shipping => Action.Observe,
                BusinessEventType.Receiving => Action.Observe,
                BusinessEventType.Unpacking => Action.Delete,
                BusinessEventType.VoidShipping => Action.Delete,
                BusinessEventType.Inspection => Action.Observe,
                BusinessEventType.Decommissioning => Action.Delete,
                BusinessEventType.Dispensing => Action.Observe,
                _ => throw new Exception($"Action not defined for event {bEvent}")
            };
        }

        public BusinessSteps GetBizStepForBusinessEvent(BusinessEventType bEvent)
        {
            return bEvent switch
            {
                BusinessEventType.Commissioning => BusinessSteps.Commissioning,
                BusinessEventType.Packing => BusinessSteps.Packing,
                BusinessEventType.Shipping => BusinessSteps.Shipping,
                BusinessEventType.Receiving => BusinessSteps.Receiving,
                BusinessEventType.Unpacking => BusinessSteps.Unpacking,
                BusinessEventType.VoidShipping => BusinessSteps.Shipping,
                BusinessEventType.Inspection => BusinessSteps.Inspecting,
                BusinessEventType.Decommissioning => BusinessSteps.Decommissioning,
                BusinessEventType.Dispensing => BusinessSteps.Dispensing,
                _ => throw new Exception($"BizStep not defined for event {bEvent}")
            };
        }

        public SelectList GetDispositionTypes(BusinessEventType bEvent)
        {
            var disposition = BusinessEventDispositionMappings.Mapping[bEvent];

            var result = disposition.Select(x => new
            {
                Value = x.ToString(),
                Text = x.GetDisplayName()
            });

            return new SelectList(result, "Value", "Text");
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
