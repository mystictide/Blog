using Blog.Business.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Blog.Web.Models;
using System;
using System.Web;
using System.Web.Mvc;
using static Blog.Web.FilterConfig;

namespace Blog.Web.Controllers
{
    [AllowAnonymous, RoutePrefix(""), Compress]
    public class HomeController : Controller
    {
        [Route("lang/{lang}")]
        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageManager().SetLanguage(lang);
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = LanguageManager.GetDefaultLanguage();
                }
            }
            new LanguageManager().SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }

        [Route("")]
        public ActionResult Index(Entity.Helpers.Filter filter, Posts filterModel)
        {
            if (Request.Cookies["culture"] != null)
            {
                ViewBag.Culture = Request.Cookies["culture"].Value;
            }
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 6;
            filter.isDetailSearch = false;
            FilteredList<Posts> request = new FilteredList<Posts>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Posts> result = new PostManager().FilteredList(request);
            return View(result);
        }

        [Route("about")]
        public ActionResult About()
        {
            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("post/{Title}/{ID}")]
        public ActionResult Post(int ID)
        {
            var model = new PostManager().Get(ID);
            return View(model);
        }
    }
}