﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.DTOs
{
    public class GreetingCardDTO
    {
        public int CardID { get; set; }
        public int UserID { get; set; }
        public int TemplateID { get; set; }
        public int TextID { get; set; }
        public int CategoryID { get; set; }
        public string CanvasStyle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
