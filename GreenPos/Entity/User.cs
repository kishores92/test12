using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Entity
{
    public class User : BaseEntity<long>
    {
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
    }
}
