using Blog.Entity.Helpers;
using Blog.Panel.App_Start;
using System.Web.Mvc;
using static Blog.Panel.FilterConfig;
using Blog.Entity.User;
using Blog.Business.User;
using Blog.Entity.Blog;
using Blog.Business.Blog;

namespace Blog.Panel.Controllers
{
    [Authorize, RoutePrefix(""), Compress]
    public class ListController : Controller
    {
        private Users user;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            user = UserAuthorize.GetAuthCookie();
        }

        [Route("users")]
        public ActionResult UserList(Entity.Helpers.Filter filter, Users filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 15;
            filter.isDetailSearch = false;
            FilteredList<Users> request = new FilteredList<Users>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Users> result = new UserManager().FilteredList(request);
            return View(result);
        }

        [Route("posts")]
        public ActionResult PostList(Entity.Helpers.Filter filter, Posts filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 15;
            filter.isDetailSearch = false;
            FilteredList<Posts> request = new FilteredList<Posts>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Posts> result = new PostManager().FilteredList(request);
            return View(result);
        }

        [Route("categories")]
        public ActionResult CategoryList(Entity.Helpers.Filter filter, Categories filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 20;
            filter.isDetailSearch = false;
            FilteredList<Categories> request = new FilteredList<Categories>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Categories> result = new CategoryManager().FilteredList(request);
            return View(result);
        }

        [Route("messages")]
        public ActionResult MessageList(Entity.Helpers.Filter filter, Contact filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 15;
            filter.isDetailSearch = false;
            FilteredList<Contact> request = new FilteredList<Contact>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Contact> result = new ContactManager().FilteredList(request);
            return View(result);
        }
    }
}