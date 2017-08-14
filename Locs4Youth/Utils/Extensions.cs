using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Locs4Youth.Utils
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplay(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                        typeof(DisplayAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Name;

            return value.ToString();
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/a/17187010
        /// </summary>
        public static string Capitalise(this string text, string targets)
        {
            bool capitalise = true;
            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (capitalise)
                {
                    result.Append(char.ToUpper(c, CultureInfo.InvariantCulture));
                    capitalise = false;
                }
                else
                {
                    if (targets.Contains(c))
                        capitalise = true;

                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}