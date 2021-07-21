using Blog.Business.User;
using Blog.Entity.User;
using System.Web.Mvc;
using System.Web.Security;
using static Blog.Panel.FilterConfig;

namespace Hobby.Panel.Controllers
{
    [AllowAnonymous, RoutePrefix("account"), Compress]
    public class AccountController : Controller
    {
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl ?? "/");
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
                TempData["Message"] = null;
            }

            return View();
        }

        [Route("login"), HttpPost]
        public ActionResult Login(string AdminsMail, string AdminsPass, string returnUrl)
        {
            Users user = new UserManager().CheckAuth(AdminsMail, AdminsPass);
            if (user != null)
            {
                string AuthCookie = "{ Mail: '" + user.Mail + "', ID: '" + user.ID + "', Name: '" + user.Name +  "', Password: '" + user.Password + "' }";

                FormsAuthentication.SetAuthCookie(AuthCookie, true);
                return Redirect("/");
            }
            else
            {
                //TempData["Message"] = new { title = "Giriş Başarısız!", type = "orange", content = "Mail and Password doesn't match!", icon = "fa fa-times-circle fa-thin" };
            }

            return Redirect(Request.UrlReferrer.LocalPath);
        }

        [Route("logout"), Authorize, HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}