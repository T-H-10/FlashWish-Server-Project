using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.DTOs
{
    public class GreetingMessageDTO
    {
        public int TextID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Signature { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
