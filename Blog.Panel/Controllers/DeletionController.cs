using Blog.Business.Blog;
using Blog.Business.User;
using Blog.Entity.User;
using Blog.Panel.App_Start;
using System.Web.Mvc;
using static Blog.Panel.FilterConfig;

namespace Blog.Panel.Controllers
{
    [Authorize, RoutePrefix("delete"), Compress]
    public class DeletionController : Controller
    {
        private Users user;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            user = UserAuthorize.GetAuthCookie();
        }

        [Route("user"), HttpPost]
        public ActionResult DeleteUser(int modelID)
        {
            new UserManager().Delete(modelID);
            return Redirect("/users");
        }

        [Route("post"), HttpPost]
        public ActionResult DeletePost(int modelID)
        {
            new PostManager().Delete(modelID);
            return Redirect("/posts");
        }

        [Route("category"), HttpPost]
        public ActionResult DeleteCategory(int modelID)
        {
            new CategoryManager().Delete(modelID);
            return Redirect("/categories");
        }

        [Route("message"), HttpPost]
        public ActionResult DeleteMessage(int modelID)
        {
            new ContactManager().Delete(modelID);
            return Redirect("/messages");
        }
    }
}