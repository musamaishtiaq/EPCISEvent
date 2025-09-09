using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.BaseClasses
{
    public class Measurement
    {
        public int Id { get; set; }
        public float MeasurementValue { get; set; }
        public string MeasurementUnitCode { get; set; }
    }
}
