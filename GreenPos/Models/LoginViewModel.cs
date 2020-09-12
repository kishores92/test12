using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class LoginViewModel
    {
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

    }
}
