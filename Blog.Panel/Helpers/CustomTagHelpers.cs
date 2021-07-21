using Blog.Entity.User;
using Blog.Panel.App_Start;
using System;
using System.Web;
using System.Web.Mvc;

namespace Blog.Panel.Helpers
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
    }
}