using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Models
{
    public class UserRoleViewModel : BaseViewModel<long>
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long RoleId { get; set; }

        [MaxLength(100)]
        public string RoleName { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }
    }
}
