﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.Entities
{
    [Table("GreetingMessages")]
    public class GreetingMessage
    {
        [Key]
        public int TextID { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        [StringLength(300, ErrorMessage = "Title cannot exceed 300 characters.")]
        public string Title { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; } = 0;//---

        public string Content { get; set; }

        public string Signature { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
