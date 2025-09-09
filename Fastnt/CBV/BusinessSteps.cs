using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public enum BusinessSteps
    {
        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:accepting")]
        Accepting,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:arriving")]
        Arriving,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:assembling")]
        Assembling,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:collecting")]
        Collecting,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:commissioning")]
        Commissioning,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:consigning")]
        Consigning,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:creating_class_instance")]
        CreatingClassInstance,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:cycle_counting")]
        CycleCounting,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:decommissioning")]
        Decommissioning,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:departing")]
        Departing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:destroying")]
        Destroying,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:disassembling")]
        Disassembling,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:dispensing")]
        Dispensing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:entering_exit")]
        EnteringExit,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:ingholding")]
        Ingholding,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:inspecting")]
        Inspecting,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:installing")]
        Installing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:killing")]
        Killing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:loading")]
        Loading,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:other")]
        Other,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:packing")]
        Packing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:picking")]
        Picking,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:receiving")]
        Receiving,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:removing")]
        Removing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:repackaging")]
        Repackaging,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:repairing")]
        Repairing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:replacing")]
        Replacing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:reserving")]
        Reserving,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:retail_selling")]
        RetailSelling,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:shipping")]
        Shipping,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:staging_outbound")]
        StagingOutbound,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:stock_taking")]
        StockTaking,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:stocking")]
        Stocking,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:storing")]
        Storing,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:transporting")]
        Transporting,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:unloading")]
        Unloading,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:unpacking")]
        Unpacking,

        [EnumMember(Value = "urn:epcglobal:cbv:bizstep:void_shipping")]
        VoidShipping
    }
}
