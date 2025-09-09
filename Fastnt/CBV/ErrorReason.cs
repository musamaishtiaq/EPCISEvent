using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public enum ErrorReason
    {
        [EnumMember(Value = "urn:epcglobal:cbv:er:did_not_occur")]
        DidNotOccur,

        [EnumMember(Value = "urn:epcglobal:cbv:er:incorrect_data")]
        IncorrectData
    }
}
