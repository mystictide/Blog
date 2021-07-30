using Dapper.Contrib.Extensions;

namespace Blog.Entity.Blog
{
    [Table("Settings")]
    public class Settings
    {
        public string TitleTUR { get; set; }
        public string TitleENG { get; set; }
        public string DescriptionTUR { get; set; }
        public string DescriptionENG { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string YouTube { get; set; }
        public string LinkedIn { get; set; }
        [Write(false)]
        public string Banner { get; set; }
    }
}
