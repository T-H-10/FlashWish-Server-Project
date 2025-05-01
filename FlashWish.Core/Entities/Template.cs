using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.Entities
{
    [Table("Templates")]
    public class Template
    {
        [Key]
        public int TemplateID { get; set; }

        [StringLength(100, ErrorMessage = "Template name cannot exceed 100 characters.")]
        public string TemplateName { get; set; }

        //[StringLength(500, ErrorMessage = "Template description cannot exceed 500 characters.")]
        //public string TemplateDescription { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; } = 0; //---
        public bool MarkedForDeletion { get; set; } = false;

        [Url(ErrorMessage = "Invalid URL format.")]
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
