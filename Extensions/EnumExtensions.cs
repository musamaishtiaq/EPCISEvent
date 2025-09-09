using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue<T>(this T value) where T : Enum
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, value);
            if (name == null) return null;

            var fieldInfo = enumType.GetField(name);
            if (fieldInfo == null) return null;

            var attribute = fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .FirstOrDefault() as EnumMemberAttribute;

            return attribute?.Value;
        }
    }
}
