using Blog.Entity.Helpers;
using Blog.Entity.User;
using Blog.Core.Interface.User;
using Blog.Core.Repository.User;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System;

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
            var model = new UserManager().Get(ID);
            try
            {
                if (File.Exists(Path.Combine(HostingEnvironment.MapPath("~/images/authors/"), model.Picture)))
                {
                    File.Delete(Path.Combine(HostingEnvironment.MapPath("~/images/authors/"), model.Picture));
                }
            }
            catch (Exception)
            {
            }
            return _repo.Delete(ID);       
        }

        public bool DeletePicture(string Picture)
        {
            try
            {
                if (File.Exists(Path.Combine(HostingEnvironment.MapPath("~/images/authors/"), Picture)))
                {
                    File.Delete(Path.Combine(HostingEnvironment.MapPath("~/images/authors/"), Picture));
                }
            }
            catch (Exception)
            {
            }
            return true;
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
