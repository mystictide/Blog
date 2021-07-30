using Blog.Core.Interface.Blog;
using Blog.Core.Repository.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Blog.Business.Blog
{
    public class PostManager : IPosts
    {
        private readonly IPosts _repo;
        public PostManager()
        {
            _repo = new PostRepository();
        }
        public ProcessResult Add(Posts entity)
        {
            return _repo.Add(entity);
        }
        public ProcessResult ProcessPost(Posts entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                if (entity.ID > 0)
                {
                    result = Update(entity);
                }
                else
                {
                    entity.Date = DateTime.Now;
                    result = Add(entity);
                }

                List<PostCategoryJunk> categories = new List<PostCategoryJunk>();
                if (entity.Categories != null)
                {
                    entity.Categories.ForEach(m => { PostCategoryJunk category = new PostCategoryJunk(); category.CategoryID = m; category.PostID = result.ReturnID; categories.Add(category); });
                }
                new PostCategoryJunkManager().DeleteAllbyPost(result.ReturnID); 
                new PostCategoryJunkManager().Add(categories);

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
                return result;
            }
        }

        public ProcessResult Delete(int ID)
        {
            var model = new PostManager().Get(ID);
            new PostCategoryJunkManager().DeleteAllbyPost(ID);
            try
            {
                if (File.Exists(Path.Combine(HostingEnvironment.MapPath("~/images/posts/"), model.Banner)))
                {
                    File.Delete(Path.Combine(HostingEnvironment.MapPath("~/images/posts/"), model.Banner));
                }
            }
            catch (Exception)
            {
            }
            return _repo.Delete(ID);
        }

        public FilteredList<Posts> FilteredList(FilteredList<Posts> request)
        {
            return _repo.FilteredList(request);
        }

        public Posts Get(int ID)
        {
            return _repo.Get(ID);
        }

        public bool DeleteBanner(string Banner)
        {
            try
            {
                if (File.Exists(Path.Combine(HostingEnvironment.MapPath("~/images/posts/"), Banner)))
                {
                    File.Delete(Path.Combine(HostingEnvironment.MapPath("~/images/posts/"), Banner));
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        public IEnumerable<Posts> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Posts entity)
        {
            return _repo.Update(entity);
        }

        public FilteredList<PostCategoryJunk> PostsbyCategory(FilteredList<PostCategoryJunk> request, int CategoryID)
        {
            return _repo.PostsbyCategory(request, CategoryID);
        }

        public Posts GetPostforView(int ID)
        {
            return _repo.GetPostforView(ID);
        }
    }
}
