using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.BaseClasses
{
    public class GS1Location : Address
    {
        public string GLN13 { get; set; }
        public string SGLN { get; set; }
    }
}
