using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.DTOs
{
    public class UserWithRolesDTO : UserResponseDTO
    { 
        public List<string> Roles { get; set; }
    }
}
