using Blog.Entity.Blog;
using Blog.Entity.Helpers;

namespace Blog.Core.Interface.Blog
{
    public interface IPosts : IBaseInterface<Posts>
    {
        FilteredList<PostCategoryJunk> PostsbyCategory(FilteredList<PostCategoryJunk> request, int? CategoryID);
    }
}
