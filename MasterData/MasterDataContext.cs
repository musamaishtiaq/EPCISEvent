using Microsoft.EntityFrameworkCore;
using EPCISEvent.MasterData.MainClasses;
using EPCISEvent.MasterData.SupportingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData
{
    public class MasterDataContext : DbContext
    {
        protected string Schema => "MasterData";

        public MasterDataContext(DbContextOptions<MasterDataContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<LocationField> LocationFields { get; set; }
        public DbSet<LocationIdentifier> LocationIdentifiers { get; set; }
        public DbSet<TradeItem> TradeItems { get; set; }
        public DbSet<TradeItemField> TradeItemFields { get; set; }
        public DbSet<OutboundMapping> OutboundMappings { get; set; }
        public DbSet<EventData> EventDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrEmpty(Schema))
                modelBuilder.HasDefaultSchema(Schema);
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Locations)
                .WithOne(l => l.Company)
                .HasForeignKey(l => l.CompanyId);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.TradeItems)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.SubSites)
                .WithOne(l => l.Site)
                .HasForeignKey(l => l.SiteId);

            // Configure inheritance for Address/GS1Location
            // These are implemented as interfaces in C# rather than true inheritance
            // since multiple inheritance isn't supported

            // Add indexes
            modelBuilder.Entity<TradeItem>()
                .HasIndex(t => t.GTIN14)
                .IsUnique();

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.GS1CompanyPrefix)
                .IsUnique();
        }
    }
}
