using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(80, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 80 characters.")]
        public string CategoryName { get; set; }

        public List<Template> Templates { get; set; } = new List<Template>();
        public List<GreetingMessage> GreetingMessages { get; set; } = new List<GreetingMessage>();

    }
}
