using Dapper.Contrib.Extensions;

namespace Blog.Entity.Blog
{
    [Table("PostCategoryJunk")]
    public class PostCategoryJunk
    {
        [Key]
        public int ID { get; set; }
        public int PostID { get; set; }
        public int CategoryID { get; set; }

        [Write(false)]
        public Posts Post { get; set; }
    }
}
