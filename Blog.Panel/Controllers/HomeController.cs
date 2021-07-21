using Blog.Entity.User;
using Blog.Panel.App_Start;
using System.Web.Mvc;
using static Blog.Panel.FilterConfig;

namespace Blog.Panel.Controllers
{
    [Authorize, RoutePrefix(""), Compress]
    public class HomeController : Controller
    {
        private Users user;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            user = UserAuthorize.GetAuthCookie();
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}