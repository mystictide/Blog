using Dapper.Contrib.Extensions;
using System;

namespace Blog.Entity
{
    [Table("Logs")]
    public class Logs
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public int Line { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
