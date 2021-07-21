using Dapper.Contrib.Extensions;

namespace Blog.Entity.User
{

    [Table("Users")]
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }
    }
}
