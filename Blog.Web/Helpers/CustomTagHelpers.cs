using Blog.Entity.User;
using Blog.Panel.App_Start;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Helpers
{
    public static class CustomTagHelpers
    {
        public static IHtmlString GetEnumDisplayName(this HtmlHelper helper, Enum value)
        {
            return MvcHtmlString.Create(Business.Helpers.ToolHelpers.GetDisplayName(value));
        }
        public static IHtmlString GetWelcomeUserName(this HtmlHelper helper)
        {
            string UserName = "";
            try
            {
                Users user = UserAuthorize.GetAuthCookie();
                var names = user.Name.Split(' ');
                UserName = names.Length == 1 ? user.Name : user.Name.Replace(names[names.Length - 1], "");
            }
            catch
            {
            }
            return MvcHtmlString.Create(UserName);

        }

        public static string FriendlyURLTitle(string incomingText)
        {

            if (incomingText != null)
            {
                incomingText = incomingText.ToLower();
                incomingText = incomingText.Replace("ş", "s");
                incomingText = incomingText.Replace("ı", "i");
                incomingText = incomingText.Replace("ö", "o");
                incomingText = incomingText.Replace("ü", "u");
                incomingText = incomingText.Replace("ç", "c");
                incomingText = incomingText.Replace("Ç", "C");
                incomingText = incomingText.Replace("ğ", "g");
                incomingText = incomingText.Replace(" ", "-");
                incomingText = incomingText.Replace("---", "-");
                incomingText = incomingText.Replace("?", "");
                incomingText = incomingText.Replace("/", "");
                incomingText = incomingText.Replace(".", "");
                incomingText = incomingText.Replace("'", "");
                incomingText = incomingText.Replace("#", "");
                incomingText = incomingText.Replace("%", "");
                incomingText = incomingText.Replace("&", "");
                incomingText = incomingText.Replace("*", "");
                incomingText = incomingText.Replace("!", "");
                incomingText = incomingText.Replace("@", "");
                incomingText = incomingText.Replace("+", "-arti-");
                incomingText = incomingText.Trim();
                string encodedUrl = (incomingText ?? "").ToLower();
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                encodedUrl = encodedUrl.Replace("'", "");
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
                encodedUrl = encodedUrl.Trim('-');
                return encodedUrl;
            }
            else
            {
                return "";
            }
        }
    }
}