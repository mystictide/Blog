using Blog.Business.Blog;
using Blog.Business.User;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Blog.Entity.User;
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
        private Settings settings;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            settings = new SettingsManager().Get(0);
        }

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
            ViewBag.Categories = new CategoryManager().GetAll();
            ViewBag.Settings = settings;
            return View(result);
        }

        [Route("c/{CategoryID}")]
        public ActionResult Categorized(Entity.Helpers.Filter filter, PostCategoryJunk filterModel, int CategoryID)
        {
            if (Request.Cookies["culture"] != null)
            {
                ViewBag.Culture = Request.Cookies["culture"].Value;
            }
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 6;
            filter.isDetailSearch = false;
            FilteredList<PostCategoryJunk> request = new FilteredList<PostCategoryJunk>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<PostCategoryJunk> result = new PostManager().PostsbyCategory(request, CategoryID);
            ViewBag.Categories = new CategoryManager().GetAll();
            ViewBag.Settings = settings;
            return View(result);
        }

        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Settings = settings;
            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            ViewBag.Settings = settings;
            var model = new ContactView();
            return View(model);
        }

        [Route("contact"), HttpPost]
        public ActionResult Contact(ContactView model)
        {
            ViewBag.Settings = settings;
            var entity = new Contact();
            entity.Name = model.Name;
            entity.Mail = model.Mail;
            entity.Message = model.Message;
            entity.Phone = model.Phone.Replace("(", "").Replace(")", "").Replace(" ", "");
            entity.Date = DateTime.Now;
            entity.IsActive = true;
            ProcessResult result = new ContactManager().Add(entity);
            if (result.State == ProcessState.Success)
            {
                TempData["state"] = 1;
            }
            else
            {
                TempData["state"] = 2;
            }
            return Redirect("/contact");
        }

        [Route("post/{Title}/{ID}")]
        public ActionResult Post(int ID)
        {
            ViewBag.Categories = new CategoryManager().GetAll();
            ViewBag.Settings = settings;
            var model = new PostManager().GetPostforView(ID);
            return View(model);
        }
    }
}