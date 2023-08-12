using System.ComponentModel;
using System.Reflection;

namespace DesafioHyperativa.Util;

public static class EnumExtensions
{
    public static string ToDescription(this Enum value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo != null)
        {
            DescriptionAttribute[] attributes =
                fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString();
    }
}
