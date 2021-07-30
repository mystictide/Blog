using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models
{
    public class ContactView
    {
        public int ID { get; set; }
        [Required(ErrorMessageResourceName = nameof(Lang.Global.Required), ErrorMessageResourceType = typeof(Lang.Global))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = nameof(Lang.Global.Required), ErrorMessageResourceType = typeof(Lang.Global))]
        [RegularExpression(@"^([\w.-]+)@([\w-]+)((.(\w){2,3})+)$", ErrorMessageResourceName = nameof(Lang.Global.EmailFormat), ErrorMessageResourceType = typeof(Lang.Global))]
        public string Mail { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessageResourceName = nameof(Lang.Global.Required), ErrorMessageResourceType = typeof(Lang.Global))]
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}