using EPCISEvent.MasterData.BaseClasses;
using EPCISEvent.MasterData.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.SupportingClasses
{
    public class CompanyType : GenericType
    {
        public ICollection<Company> Companies { get; set; }
    }
}
