using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class DesignViewModel : BaseViewModel<int>
    {        
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Cost { get; set; }

        public int? Bed { get; set; }

        public int? Bath { get; set; }

        public int? Parking { get; set; }

        public decimal? MinBlockWidth { get; set; }

        public string DisplayPrice
        {
            get
            {
                //if (Cost == 0)
                //    return "Included";
                //return $"${Cost.T}";
                return Cost.ToString("C0");
            }
        }
        public bool IsSelected { get; set; }
    }
}
