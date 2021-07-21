using Blog.Entity.Helpers;
using Blog.Entity.User;
using Blog.Core.Interface.User;
using Blog.Core.Repository.User;
using System.Collections.Generic;

namespace Blog.Business.User
{
    public class UserManager : IUsers
    {
        private readonly IUsers _repo;
        public UserManager()
        {
            _repo = new UserRepository();
        }
        public ProcessResult Add(Users entity)
        {
            return _repo.Add(entity);
        }

        public Users CheckAuth(string Mail, string Password)
        {
            return _repo.CheckAuth(Mail, Password);
        }

        public bool CheckMail(string Mail)
        {
            return _repo.CheckMail(Mail);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Users> FilteredList(FilteredList<Users> request)
        {
            return _repo.FilteredList(request);
        }

        public Users Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Users> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Users entity)
        {
            return _repo.Update(entity);
        }
    }
}
