using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public interface InstanceLotMasterData
    {
        string Value { get; }
    }

    public enum TradeItemLevelAttributeName
    {
        [EnumMember(Value = "additionalTradeItemIdentification")]
        AdditionalTradeItemIdentification,

        [EnumMember(Value = "additionalTradeItemIdentificationTypeCode")]
        AdditionalTradeItemIdentificationTypeCode,

        [EnumMember(Value = "countryOfOrigin")]
        CountryOfOrigin,

        [EnumMember(Value = "descriptionShort")]
        DescriptionShort,

        [EnumMember(Value = "dosageFormType")]
        DosageFormType,

        [EnumMember(Value = "drainedWeight")]
        DrainedWeight,

        [EnumMember(Value = "functionalName")]
        FunctionalName,

        [EnumMember(Value = "grossWeight")]
        GrossWeight,

        [EnumMember(Value = "manufacturerOfTradeItemPartyName")]
        ManufacturerOfTradeItemPartyName,

        [EnumMember(Value = "netWeight")]
        NetWeight,

        [EnumMember(Value = "labelDescription")]
        LabelDescription,

        [EnumMember(Value = "regulatedProductName")]
        RegulatedProductName,

        [EnumMember(Value = "strengthDescription")]
        StrengthDescription,

        [EnumMember(Value = "tradeItemDescription")]
        TradeItemDescription
    }

    public enum LotLevelAttributeName
    {
        [EnumMember(Value = "bestBeforeDate")]
        BestBeforeDate,

        [EnumMember(Value = "countryOfOrigin")]
        CountryOfOrigin,

        [EnumMember(Value = "farmList")]
        FarmList,

        [EnumMember(Value = "firstFreezeDate")]
        FirstFreezeDate,

        [EnumMember(Value = "growingMethodCode")]
        GrowingMethodCode,

        [EnumMember(Value = "harvestEndDate")]
        HarvestEndDate,

        [EnumMember(Value = "harvestStartDate")]
        HarvestStartDate,

        [EnumMember(Value = "itemExpirationDate")]
        ItemExpirationDate,

        [EnumMember(Value = "sellByDate")]
        SellByDate,

        [EnumMember(Value = "storageStateCode")]
        StorageStateCode
    }

    public enum ItemLevelAttributeName
    {
        [EnumMember(Value = "countryOfOrigin")]
        CountryOfOrigin,

        [EnumMember(Value = "drainedWeight")]
        DrainedWeight,

        [EnumMember(Value = "grossWeight")]
        GrossWeight,

        [EnumMember(Value = "lotNumber")]
        LotNumber,

        [EnumMember(Value = "netWeight")]
        NetWeight,

        [EnumMember(Value = "measurement")]
        Measurement,

        [EnumMember(Value = "measurementUnitCode")]
        MeasurementUnitCode
    }

    public enum FarmListAttributeName
    {
        [EnumMember(Value = "farmIdentification")]
        FarmIdentification,

        [EnumMember(Value = "farmIdentificationTypeCode")]
        FarmIdentificationTypeCode
    }

    public enum MeasurementAttributeName
    {
        [EnumMember(Value = "measurement")]
        Measurement,

        [EnumMember(Value = "measurementUnitCode")]
        MeasurementUnitCode
    }

    public class InstanceLotMasterDataAttribute
    {
        private string _value;
        private string _name;

        public InstanceLotMasterDataAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Value
        {
            get => _value;
            set => _value = value;
        }
    }

    public static class ILMDExtensions
    {
        public static string ToValueString(this InstanceLotMasterData attribute)
        {
            var enumType = attribute.GetType();
            var memberInfo = enumType.GetMember(attribute.ToString());
            var attributeValue = memberInfo[0]
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault()?
                .Value;

            return attributeValue ?? attribute.ToString();
        }
    }
}
