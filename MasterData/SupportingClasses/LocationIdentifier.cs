using EPCISEvent.MasterData.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.SupportingClasses
{
    public class LocationIdentifier
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string IdentifierType { get; set; }
        public string Description { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }

}
