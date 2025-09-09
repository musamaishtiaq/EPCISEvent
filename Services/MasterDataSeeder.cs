using EPCISEvent.MasterData;
using EPCISEvent.MasterData.MainClasses;
using EPCISEvent.MasterData.SupportingClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPCISEvent.Services
{
    public class MasterDataSeeder
    {
        private readonly MasterDataContext _context;
        private readonly ILogger<MasterDataSeeder> _logger;

        public MasterDataSeeder(MasterDataContext context, ILogger<MasterDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedCompanyTypes();
                await SeedLocationTypes();
                await SeedCompanies();
                await SeedLocations();
                await SeedTradeItems();
                //await SeedOutboundMappings();
                await SeedCustomFields();

                _logger.LogInformation("Master data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding master data");
                throw;
            }
        }

        private async Task SeedCompanyTypes()
        {
            if (await _context.CompanyTypes.AnyAsync()) return;

            var companyTypes = new[]
            {
                new CompanyType { Identifier = "MANUFACTURER", Description = "Product Manufacturer" },
                new CompanyType { Identifier = "DISTRIBUTOR", Description = "Product Distributor" },
                new CompanyType { Identifier = "PHARMACY", Description = "Retail Pharmacy" },
                new CompanyType { Identifier = "HOSPITAL", Description = "Healthcare Hospital" },
                new CompanyType { Identifier = "WHOLESALER", Description = "Wholesale Distributor" }
            };

            await _context.CompanyTypes.AddRangeAsync(companyTypes);
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocationTypes()
        {
            if (await _context.LocationTypes.AnyAsync()) return;

            var locationTypes = new[]
            {
                new LocationType { Identifier = "WAREHOUSE", Description = "Storage Warehouse" },
                new LocationType { Identifier = "STORAGE_ROOM", Description = "Storage Room" },
                new LocationType { Identifier = "PHARMACY", Description = "Retail Pharmacy" },
                new LocationType { Identifier = "DISPENSARY", Description = "Medication Dispensary" },
                new LocationType { Identifier = "SHIPPING_DOCK", Description = "Shipping Dock Area" }
            };

            await _context.LocationTypes.AddRangeAsync(locationTypes);
            await _context.SaveChangesAsync();
        }

        private async Task SeedCompanies()
        {
            if (await _context.Companies.AnyAsync()) return;

            var manufacturerType = await _context.CompanyTypes.FirstAsync(ct => ct.Identifier == "MANUFACTURER");
            var distributorType = await _context.CompanyTypes.FirstAsync(ct => ct.Identifier == "DISTRIBUTOR");

            var companies = new[]
                    {
                new Company
                {
                    Name = "PharmaCorp Manufacturing",
                    GS1CompanyPrefix = "0614141",
                    CompanyTypeId = manufacturerType.Id,
                    Address1 = "123 Pharma Way",
                    City = "Boston",
                    StateProvince = "MA",
                    PostalCode = "02115",
                    Country = "USA",
                    GLN13 = "0614141000008",
                    SGLN = "0614141.00000.8"
                },
                new Company
                {
                    Name = "MediDistribute Inc.",
                    GS1CompanyPrefix = "0087654",
                    CompanyTypeId = distributorType.Id,
                    Address1 = "456 Distribution Ave",
                    City = "Chicago",
                    StateProvince = "IL",
                    PostalCode = "60601",
                    Country = "USA",
                    GLN13 = "0087654000005",
                    SGLN = "0087654.00000.5"
                }
            };

            await _context.Companies.AddRangeAsync(companies);
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocations()
        {
            if (await _context.Locations.AnyAsync()) return;

            var companies = await _context.Companies.ToListAsync();
            var locationTypes = await _context.LocationTypes.ToListAsync();

            var warehouseType = locationTypes.First(lt => lt.Identifier == "WAREHOUSE");
            var pharmacyType = locationTypes.First(lt => lt.Identifier == "PHARMACY");

            var locations = new[]
            {
                new Location
                {
                    Name = "Main Manufacturing Facility",
                    CompanyId = companies[0].Id,
                    LocationTypeId = warehouseType.Id,
                    Address1 = "123 Pharma Way",
                    City = "Boston",
                    StateProvince = "MA",
                    PostalCode = "02115",
                    Country = "USA",
                    GLN13 = "0614141000001",
                    SGLN = "0614141.00001.1",
                    SST = 1,
                    SSA = "A1"
                },
                new Location
                {
                    Name = "Central Distribution Center",
                    CompanyId = companies[1].Id,
                    LocationTypeId = warehouseType.Id,
                    Address1 = "456 Distribution Ave",
                    City = "Chicago",
                    StateProvince = "IL",
                    PostalCode = "60601",
                    Country = "USA",
                    GLN13 = "0087654000001",
                    SGLN = "0087654.00001.1",
                    SST = 1,
                    SSA = "DC1"
                },
                new Location
                {
                    Name = "Retail Pharmacy Store #101",
                    CompanyId = companies[1].Id,
                    LocationTypeId = pharmacyType.Id,
                    SiteId = 2, // Reference to Distribution Center
                    Address1 = "789 Retail Blvd",
                    City = "Chicago",
                    StateProvince = "IL",
                    PostalCode = "60602",
                    Country = "USA",
                    GLN13 = "0087654000101",
                    SGLN = "0087654.00101.101",
                    SST = 2,
                    SSA = "S101"
                }
            };

            await _context.Locations.AddRangeAsync(locations);
            await _context.SaveChangesAsync();
        }

        private async Task SeedTradeItems()
        {
            if (await _context.TradeItems.AnyAsync()) return;

            var companies = await _context.Companies.ToListAsync();

            var tradeItems = new[]
            {
                new TradeItem
                {
                    GTIN14 = "00614141123456",
                    CompanyId = companies[0].Id,
                    DescriptionShort = "Aspirin 500mg Tablets",
                    FunctionalName = "Pain Relief Tablets",
                    ManufacturerName = "PharmaCorp Manufacturing",
                    RegulatedProductName = "Aspirin 500mg",
                    StrengthDescription = "500mg per tablet",
                    TradeItemDescription = "Bottle of 100 aspirin tablets 500mg",
                    SerialNumberLength = 10,
                    PackCount = 100,
                    CountryOfOrigin = "USA",
                    NetWeight = 0.5f,
                    NetWeightUom = "g",
                    PackageUom = "EA",
                    NDC = "12345-6789-01",
                    NdcPattern = "5-4-2"
                },
                new TradeItem
                {
                    GTIN14 = "00614141123478",
                    CompanyId = companies[0].Id,
                    DescriptionShort = "Ibuprofen 200mg Capsules",
                    FunctionalName = "Anti-inflammatory Capsules",
                    ManufacturerName = "PharmaCorp Manufacturing",
                    RegulatedProductName = "Ibuprofen 200mg",
                    StrengthDescription = "200mg per capsule",
                    TradeItemDescription = "Box of 24 ibuprofen capsules 200mg",
                    SerialNumberLength = 8,
                    PackCount = 24,
                    CountryOfOrigin = "USA",
                    NetWeight = 0.3f,
                    NetWeightUom = "g",
                    PackageUom = "EA",
                    NDC = "12345-6790-02",
                    NdcPattern = "5-4-2"
                }
            };

            await _context.TradeItems.AddRangeAsync(tradeItems);
            await _context.SaveChangesAsync();
        }

        private async Task SeedOutboundMappings()
        {
            if (await _context.OutboundMappings.AnyAsync()) return;

            var companies = await _context.Companies.ToListAsync();
            var locations = await _context.Locations.ToListAsync();

            var mappings = new[]
            {
                new OutboundMapping
                {
                    CompanyId = companies[0].Id,
                    FromBusinessId = companies[0].Id,
                    ShipFromId = locations[0].Id, // Manufacturing facility
                    ToBusinessId = companies[1].Id,
                    ShipToId = locations[1].Id // Distribution center
                }
            };

            await _context.OutboundMappings.AddRangeAsync(mappings);
            await _context.SaveChangesAsync();
        }

        private async Task SeedCustomFields()
        {
            if (await _context.TradeItemFields.AnyAsync()) return;

            var tradeItems = await _context.TradeItems.ToListAsync();
            var locations = await _context.Locations.ToListAsync();

            // Trade Item Fields
            var tradeItemFields = new[]
            {
                new TradeItemField
                {
                    TradeItemId = tradeItems[0].Id,
                    Name = "StorageTemperature",
                    Value = "Room Temperature",
                    Description = "Required storage conditions"
                },
                new TradeItemField
                {
                    TradeItemId = tradeItems[0].Id,
                    Name = "ShelfLife",
                    Value = "24 Months",
                    Description = "Product shelf life"
                }
            };

            // Location Fields
            var locationFields = new[]
            {
                new LocationField
                {
                    LocationId = locations[0].Id,
                    Name = "TemperatureZone",
                    Value = "Controlled Room Temp",
                    Description = "Storage temperature zone"
                }
            };

            await _context.TradeItemFields.AddRangeAsync(tradeItemFields);
            await _context.LocationFields.AddRangeAsync(locationFields);
            await _context.SaveChangesAsync();
        }
    }
}
