using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public enum SourceDestinationTypes
    {
        [EnumMember(Value = "urn:epcglobal:cbv:sdt:owning_party")]
        OwningParty,

        [EnumMember(Value = "urn:epcglobal:cbv:sdt:possessing_party")]
        PossessingParty,

        [EnumMember(Value = "urn:epcglobal:cbv:sdt:location")]
        Location
    }
}
