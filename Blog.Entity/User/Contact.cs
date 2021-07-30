using Dapper.Contrib.Extensions;
using System;

namespace Blog.Entity.User
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
