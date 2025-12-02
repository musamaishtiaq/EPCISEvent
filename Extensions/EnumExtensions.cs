using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

        public static string GetDisplayName(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return value.ToString();
            DisplayAttribute attribute = (DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute));
            return attribute != null ? attribute.Name : value.ToString();
        }
    }
}
