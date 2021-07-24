using Blog.Business.Blog;
using Blog.Business.User;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Blog.Entity.User;
using Blog.Panel.App_Start;
using System.Collections.Generic;
using System.Web.Mvc;
using static Blog.Panel.FilterConfig;

namespace Blog.Panel.Controllers
{

    [Authorize, RoutePrefix(""), Compress]
    public class ProcessController : Controller
    {
        private Users user;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            user = UserAuthorize.GetAuthCookie();
        }

        [Route("user/{ID?}")]
        public ActionResult ProcessUser(int? ID)
        {
            var model = new Users();
            if (ID.HasValue)
            {
                model = new UserManager().Get(ID.Value);
            }
            return View(model);
        }

        [Route("user"), HttpPost]
        public ActionResult ProcessUser(Users model)
        {
            ProcessResult result = new ProcessResult();
            if (model != null)
            {
                result = new UserManager().Update(model);
            }
            return Redirect("~/users");
        }

        [Route("post/{ID?}")]
        public ActionResult ProcessPost(int? ID)
        {
            var model = new Posts();
            ViewBag.Categories = new CategoryManager().GetAll();
            if (ID.HasValue)
            {
                model = new PostManager().Get(ID.Value);
            }
            return View(model);
        }

        [Route("post"), HttpPost]
        public ActionResult ProcessPost(Posts model, List<int> drop)
        {
            model.UserID = user.ID;
            model.Categories = drop;
            ProcessResult result = new ProcessResult();

            result = new PostManager().ProcessPost(model);
            return Redirect("~/post/" + result.ReturnID);
        }

        [Route("category/{ID?}")]
        public ActionResult ProcessCategory(int? ID)
        {
            var model = new Categories();
            if (ID.HasValue)
            {
                model = new CategoryManager().Get(ID.Value);
            }
            return View(model);
        }

        [Route("category"), HttpPost]
        public ActionResult ProcessCategory(Categories model)
        {
            ProcessResult result = new ProcessResult();
            if (model.ID > 0)
            {
                result = new CategoryManager().Update(model);
            }
            else
            {
                result = new CategoryManager().Add(model);
            }
            return Redirect("~/categories/");
        }
    }
}