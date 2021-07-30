using Blog.Core.Interface.Blog;
using Blog.Core.Repository.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using System.Collections.Generic;

namespace Blog.Business.Blog
{
    public class SettingsManager : ISettings
    {
        private readonly ISettings _repo;
        public SettingsManager()
        {
            _repo = new SettingsRepository();
        }
        public ProcessResult Add(Settings entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Settings> FilteredList(FilteredList<Settings> request)
        {
            return _repo.FilteredList(request);
        }

        public Settings Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Settings> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Settings entity)
        {
            return _repo.Update(entity);
        }
    }
}
