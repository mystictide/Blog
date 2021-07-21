using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Blog.Business.Helpers
{
    public class ToolHelpers
    {
        public static string ClearUrl(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";

            s = s.ToLower();
            s = s.Replace("ş", "s");
            s = s.Replace("Ş", "S");
            s = s.Replace("ğ", "g");
            s = s.Replace("Ğ", "G");
            s = s.Replace("İ", "I");
            s = s.Replace("ı", "i");
            s = s.Replace("ç", "c");
            s = s.Replace("Ç", "C");
            s = s.Replace("ö", "o");
            s = s.Replace("Ö", "O");
            s = s.Replace("ü", "u");
            s = s.Replace("Ü", "U");
            s = s.Replace("'", "");
            s = s.Replace("\"", "");
            Regex r = new Regex("[^a-zA-Z0-9_-]");
            //if (r.IsMatch(s))
            s = r.Replace(s, "-");
            if (!string.IsNullOrEmpty(s))
                while (s.IndexOf("--") > -1)
                    s = s.Replace("--", "-");
            if (s.StartsWith("-")) s = s.Substring(1);
            if (s.EndsWith("-")) s = s.Substring(0, s.Length - 1);

            return s;
        }
        public static decimal DecimalTryParse(string s)
        {
            if (decimal.TryParse(s, out decimal a))
            {
                return decimal.Parse(s, new CultureInfo("en-US"));
            }
            return 0;
        }
        public static int IntTryParse(string s)
        {
            if (int.TryParse(s, out int a))
            {
                return int.Parse(s, new CultureInfo("en-US"));
            }
            return 0;
        }
        public static string GetDisplayName(Enum value)
        {
            var type = value.GetType();
            if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

            var members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException(String.Format("Member '{0}' not found in type '{1}'", value, type.Name));

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length == 0) throw new ArgumentException(String.Format("'{0}.{1}' doesn't have DisplayAttribute", type.Name, value));

            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }
    }
}
