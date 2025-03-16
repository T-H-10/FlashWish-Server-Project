using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.Entities
{
    [Table("GreetingCards")]
    public class GreetingCard
    {
        [Key]
        public int CardID { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }

        [ForeignKey(nameof(Template))]
        public int TemplateID { get; set; }

        [ForeignKey(nameof(GreetingMessage))]
        public int TextID { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
