using EPCISEvent.MasterData.BaseClasses;
using EPCISEvent.MasterData.SupportingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.MasterData.MainClasses
{
    public class TradeItem : ItemInstance
    {
        public int Id { get; set; }
        public string GTIN14 { get; set; }
        public string NDC { get; set; }
        public string NdcPattern { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string AdditionalId { get; set; }
        public string AdditionalIdTypeCode { get; set; }
        public string DescriptionShort { get; set; }
        public string DosageFormType { get; set; }
        public string FunctionalName { get; set; }
        public string ManufacturerName { get; set; }
        public string NetContentDescription { get; set; }
        public string LabelDescription { get; set; }
        public string RegulatedProductName { get; set; }
        public string StrengthDescription { get; set; }
        public string TradeItemDescription { get; set; }
        public int? SerialNumberLength { get; set; }
        public int? PackCount { get; set; }

        public ICollection<TradeItemField> Fields { get; set; }

        public string NDC11Digit
        {
            get
            {
                if (string.IsNullOrEmpty(NDC)) return null;

                var ndcVals = NDC.Split('-');
                return NdcPattern switch
                {
                    "4-4-2" => $"0{ndcVals[0]}-{ndcVals[1]}-{ndcVals[2]}",
                    "5-3-2" => $"{ndcVals[0]}-0{ndcVals[1]}-{ndcVals[2]}",
                    "5-4-1" => $"{ndcVals[0]}-{ndcVals[1]}-0{ndcVals[2]}",
                    _ => null
                };
            }
        }

        public string NDC11Format => "5-4-2";
    }
}
