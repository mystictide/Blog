using Blog.Core.Interface.Blog;
using Blog.Core.Repository.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using System.Collections.Generic;

namespace Blog.Business.Blog
{
    public class PostCategoryJunkManager : IPostCategoryJunk
    {
        private readonly IPostCategoryJunk _repo;
        public PostCategoryJunkManager()
        {
            _repo = new PostCategoryJunkRepository();
        }
        public ProcessResult Add(PostCategoryJunk entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Add(List<PostCategoryJunk> InsertList)
        {
            return _repo.Add(InsertList);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public ProcessResult DeleteAllbyPost(int PostID)
        {
            return _repo.DeleteAllbyPost(PostID);
        }

        public FilteredList<PostCategoryJunk> FilteredList(FilteredList<PostCategoryJunk> request)
        {
            return _repo.FilteredList(request);
        }

        public PostCategoryJunk Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<PostCategoryJunk> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(PostCategoryJunk entity)
        {
            return _repo.Update(entity);
        }
    }
}
