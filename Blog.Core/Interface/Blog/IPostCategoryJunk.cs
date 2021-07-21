using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using System.Collections.Generic;

namespace Blog.Core.Interface.Blog
{
    public interface IPostCategoryJunk : IBaseInterface<PostCategoryJunk>
    {
        ProcessResult Add(List<PostCategoryJunk> InsertList); 
        ProcessResult DeleteAllbyPost(int PostID);
    }
}
