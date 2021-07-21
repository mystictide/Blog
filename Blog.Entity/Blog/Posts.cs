using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Blog.Entity.Blog
{
    [Table("Posts")]
    public class Posts
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string Banner { get; set; }
        public DateTime Date { get; set; }

        [Write(false)]
        public List<int> Categories { get; set; }
    }
}
