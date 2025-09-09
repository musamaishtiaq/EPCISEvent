using EPCISEvent.MasterData.BaseClasses;
using EPCISEvent.MasterData.SupportingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.MainClasses
{
    public class Location : GS1Location
    {
        public int Id { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? SiteId { get; set; }
        public Location Site { get; set; }
        public ICollection<Location> SubSites { get; set; }

        public int? LocationTypeId { get; set; }
        public LocationType LocationType { get; set; }

        public short? SST { get; set; } // Sub-Site Type
        public string SSA { get; set; } // Sub-Site Attribute

        public ICollection<LocationField> Fields { get; set; }
        public ICollection<LocationIdentifier> Identifiers { get; set; }
    }
}
