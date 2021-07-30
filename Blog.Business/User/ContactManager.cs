using Blog.Core.Interface.User;
using Blog.Core.Repository.User;
using Blog.Entity.Helpers;
using Blog.Entity.User;
using System.Collections.Generic;

namespace Blog.Business.User
{
    public class ContactManager : IContact
    {
        private readonly IContact _repo;
        public ContactManager()
        {
            _repo = new ContactRepository();
        }
        public ProcessResult Add(Contact entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Contact> FilteredList(FilteredList<Contact> request)
        {
            return _repo.FilteredList(request);
        }

        public Contact Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Contact> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Contact entity)
        {
            return _repo.Update(entity);
        }
    }
}
