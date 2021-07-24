using Blog.Entity.User;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Blog.Panel.App_Start
{
    public class UserAuthorize
    {
        public static Users GetAuthCookie()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Users user = new JavaScriptSerializer().Deserialize<Users>(HttpContext.Current.User.Identity.Name);
                if (user.ID == 0)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(FormsAuthentication.FormsCookieName, true));
                }
                return user;
            }
            else
            {
                var defaultUrl = FormsAuthentication.GetRedirectUrl(FormsAuthentication.FormsCookieName, true);
                HttpContext.Current.Response.Redirect(defaultUrl);
            }

            return null;

        }
    }
}