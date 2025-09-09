using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public static class Helper
    {
        public static string MakeTradeItemMasterDataUrn(
            string companyPrefix,
            string indicatorDigit,
            string itemReference,
            string lot = null,
            string serialNumber = null)
        {
            if (companyPrefix.Length + indicatorDigit.Length + itemReference.Length > 13)
            {
                throw new ArgumentException(
                    "The length of the companyPrefix, indicatorDigit and itemReference parameters " +
                    "must be 13 digits when combined.");
            }

            if (!string.IsNullOrEmpty(lot))
            {
                return $"urn:epc:class:lgtin:{companyPrefix}.{indicatorDigit}{itemReference}.{lot}";
            }
            else if (!string.IsNullOrEmpty(serialNumber))
            {
                return $"urn:epc:id:sgtin:{companyPrefix}.{indicatorDigit}{itemReference}.{serialNumber}";
            }
            else
            {
                return $"urn:epc:idpat:sgtin:{companyPrefix}.{indicatorDigit}{itemReference}.*";
            }
        }
        public static Enum GetIlmdEnumByValue(string value)
        {
            var ilmdTypes = typeof(InstanceLotMasterData).GetNestedTypes()
                .Where(t => t.IsEnum && t.Name.Contains("Attribute"));

            foreach (var enumType in ilmdTypes)
            {
                foreach (var enumValue in Enum.GetValues(enumType))
                {
                    var fieldInfo = enumType.GetField(enumValue.ToString());
                    var attribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();

                    if (attribute != null && attribute.Value == value)
                    {
                        return (Enum)enumValue;
                    }
                }
            }

            return null;
        }
    }
}
