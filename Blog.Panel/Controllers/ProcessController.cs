using Blog.Business.Blog;
using Blog.Business.User;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Blog.Entity.User;
using Blog.Panel.App_Start;
using System.Collections.Generic;
using System.IO;
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

            try
            {
                foreach (string file in Request.Files)
                {
                    var FileData = Request.Files[file];
                    if (FileData.ContentLength > 0)
                    {
                        string _Ext = Path.GetExtension(FileData.FileName);
                        string _path = Path.Combine(Server.MapPath("~/images/posts/"));
                        if (!Directory.Exists(_path))
                        {
                            Directory.CreateDirectory(_path);
                        }
                        FileData.SaveAs(_path + result.ReturnID + _Ext);
                        model.ID = result.ReturnID;
                        model.Banner = result.ReturnID + _Ext;
                        new PostManager().Update(model);
                    }
                }
            }
            catch
            {

            }

            return Redirect("~/post/" + result.ReturnID);
        }

        [Route("post/delbanner/")]
        public JsonResult DeleteBanner(int ID)
        {
            var model = new PostManager().Get(ID);
            new PostManager().DeleteBanner(model.Banner);
            model.Banner = null;
            new PostManager().Update(model);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

        [Route("settings")]
        public ActionResult ProcessSettings()
        {
            var model = new SettingsManager().Get(0);
            return View(model);
        }

        [Route("settings"), HttpPost]
        public ActionResult ProcessSettings(Settings model)
        {
            ProcessResult result = new ProcessResult();

            result = new SettingsManager().Update(model);

            try
            {
                foreach (string file in Request.Files)
                {
                    var FileData = Request.Files[file];
                    if (FileData.ContentLength > 0)
                    {
                        string _Ext = Path.GetExtension(FileData.FileName);
                        string _path = Path.Combine(Server.MapPath("~/images/banner/"));
                        if (!Directory.Exists(_path))
                        {
                            Directory.CreateDirectory(_path);
                        }
                        FileData.SaveAs(_path + "banner" + _Ext);
                    }
                }
            }
            catch
            {

            }

            return Redirect("~/settings/");
        }

        [Route("message/{ID}")]
        public ActionResult ProcessMessage(int ID)
        {
            var model = new ContactManager().Get(ID);
            return View(model);
        }

        [Route("message"), HttpPost]
        public ActionResult ProcessMessage(Contact model, string msg)
        {
            ProcessResult result = new ProcessResult();
            model.IsActive = false;
            //mail gönderme metodu
            return Redirect("~/messages/");
        }
    }
}