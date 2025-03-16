using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "PermissionName is required.")]
        public string PermissionName { get; set; }

        public string PermissionDescription { get; set; }

        public ICollection<Role> Roles { get; set; }

    }
}
