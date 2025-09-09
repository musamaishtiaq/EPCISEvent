using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.BaseClasses
{
    public class ItemInstance
    {
        public string CountryOfOrigin { get; set; }
        public float? DrainedWeight { get; set; }
        public string DrainedWeightUom { get; set; }
        public float? GrossWeight { get; set; }
        public string GrossWeightUom { get; set; }
        public float? NetWeight { get; set; }
        public string NetWeightUom { get; set; }
        public string PackageUom { get; set; }
    }
}
