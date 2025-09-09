using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public enum Disposition
    {
        [EnumMember(Value = "urn:epcglobal:cbv:disp:active")]
        Active,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:container_closed")]
        ContainerClosed,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:damaged")]
        Damaged,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:destroyed")]
        Destroyed,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:dispensed")]
        Dispensed,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:disposed")]
        Disposed,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:encoded")]
        Encoded,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:expired")]
        Expired,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:in_progress")]
        InProgress,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:in_transit")]
        InTransit,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:inactive")]
        Inactive,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:no_pedigree_match")]
        NoPedigreeMatch,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:non_sellable_other")]
        NonSellableOther,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:partially_dispensed")]
        PartiallyDispensed,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:recalled")]
        Recalled,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:reserved")]
        Reserved,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:retail_sold")]
        RetailSold,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:returned")]
        Returned,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:sellable_accessible")]
        SellableAccessible,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:sellable_not_accessible")]
        SellableNotAccessible,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:stolen")]
        Stolen,

        [EnumMember(Value = "urn:epcglobal:cbv:disp:unknown")]
        Unknown
    }
}
