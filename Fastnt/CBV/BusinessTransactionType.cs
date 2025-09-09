using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public enum BusinessTransactionType
    {
        [EnumMember(Value = "urn:epcglobal:cbv:btt:bol")]
        BillOfLading,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:desadv")]
        DespatchAdvice,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:inv")]
        Invoice,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:pedigree")]
        Pedigree,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:po")]
        PurchaseOrder,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:poc")]
        PurchaseOrderConfirmation,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:prodorder")]
        ProductionOrder,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:recadv")]
        ReceivingAdvice,

        [EnumMember(Value = "urn:epcglobal:cbv:btt:rma")]
        ReturnMerchandiseAuthorization
    }
}
