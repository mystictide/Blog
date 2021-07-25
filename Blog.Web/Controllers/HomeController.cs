using System.Web.Mvc;
using static Blog.Web.FilterConfig;

namespace Blog.Web.Controllers
{
    [AllowAnonymous, RoutePrefix(""), Compress]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
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

        [Route("post")]
        public ActionResult Post()
        {
            return View();
        }

        //[Route("post/{Title}/{ID}")]
        //public ActionResult Post()
        //{
        //    return View();
        //}
    }
}