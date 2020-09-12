using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class QuoteViewModel : PageModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        public string FileNameOfAttachment { get; set; }

        public List<FacadeViewModel> Facades { get; set; }

        public List<PackageViewModel> Packages { get; set; }
        public List<InclusionViewModel> Inclusions { get; set; }
        public List<DesignViewModel> Designs { get; set; }
        public List<PromotionViewModel> Promotions { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        public string Address { get; set; }
    }
}
