using EPCISEvent.MasterData.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.SupportingClasses
{
    public class OutboundMapping
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int? FromBusinessId { get; set; }
        public Company FromBusiness { get; set; }

        public int? ShipFromId { get; set; }
        public Location ShipFrom { get; set; }

        public int? ToBusinessId { get; set; }
        public Company ToBusiness { get; set; }

        public int? ShipToId { get; set; }
        public Location ShipTo { get; set; }
    }
}
