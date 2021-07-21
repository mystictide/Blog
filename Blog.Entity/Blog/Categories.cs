using Dapper.Contrib.Extensions;

namespace Blog.Entity.Blog
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
