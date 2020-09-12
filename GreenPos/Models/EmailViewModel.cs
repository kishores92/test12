using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class EmailViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        public string FileNameOfAttachment { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [EmailAddress]
        public string EmailCC { get; set; }
    }
}
