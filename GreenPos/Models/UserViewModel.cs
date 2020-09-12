using Google.Apis.Docs.v1.Data;
using GreenPOS.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Models
{
    public class UserViewModel : BaseViewModel<long>
    {
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

        [Required]
        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public string Token { get; set; }
        //public List<Role> Roles { get; set; }
    }
}
