using System.ComponentModel;

namespace Core.Extentions
{
    public static class EnumExtentions
    {
        public static string GetDescription<T>(this T enumValue) where T : struct, System.Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute []?) fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?.Length > 0 ? attributes [0].Description : enumValue.ToString();
        }
    }
}
