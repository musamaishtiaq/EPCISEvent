using EPCISEvent.MasterData.BaseClasses;
using EPCISEvent.MasterData.SupportingClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.MainClasses
{
    public class Company : GS1Location
    {
        public int Id { get; set; }
        public string GS1CompanyPrefix { get; set; }

        public int? CompanyTypeId { get; set; }
        public CompanyType CompanyType { get; set; }

        public ICollection<Location> Locations { get; set; }
        public ICollection<TradeItem> TradeItems { get; set; }
    }
}
