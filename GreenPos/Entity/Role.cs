using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Entity
{
    public class Role : BaseEntity<long>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string PermissionIds { get; set; }
    }
}
