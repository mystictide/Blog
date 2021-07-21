using Blog.Core.Interface.Blog;
using Blog.Core.Repository.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using System.Collections.Generic;

namespace Blog.Business.Blog
{
    public class CategoryManager : ICategories
    {
        private readonly ICategories _repo;
        public CategoryManager()
        {
            _repo = new CategoryRepository();
        }
        public ProcessResult Add(Categories entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Categories> FilteredList(FilteredList<Categories> request)
        {
            return _repo.FilteredList(request);
        }

        public Categories Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Categories> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Categories entity)
        {
            return _repo.Update(entity);
        }
    }
}
