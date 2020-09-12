using GreenPOS.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Models
{
    public class RoleViewModel : BaseViewModel<long>
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public string PermissionIds { get; set; }

        public string[] PermissionRoleIds { get; set; }

        public string PermissionNames { get; set; }

        public IEnumerable<ScreenPermission> Permissions { get; set; }
    }
}
