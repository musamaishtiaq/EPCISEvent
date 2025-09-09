using EPCISEvent.MasterData.BaseClasses;
using EPCISEvent.MasterData.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.SupportingClasses
{
    public class TradeItemField : Field
    {
        public int TradeItemId { get; set; }
        public TradeItem TradeItem { get; set; }
    }
}
