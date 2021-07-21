using Blog.Entity.User;

namespace Blog.Core.Interface.User
{
    public interface IUsers : IBaseInterface<Users>
    {
        bool CheckMail(string Mail);
        Users CheckAuth(string Mail, string Password);
    }
}
