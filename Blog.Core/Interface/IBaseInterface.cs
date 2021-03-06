using Blog.Entity.Helpers;
using System.Collections.Generic;

namespace Blog.Core.Interface
{
    public interface IBaseInterface<T> where T : class
    {
        ProcessResult Add(T entity);
        ProcessResult Update(T entity);
        ProcessResult Delete(int ID);
        T Get(int ID);
        IEnumerable<T> GetAll();
        FilteredList<T> FilteredList(FilteredList<T> request);
    }
}
