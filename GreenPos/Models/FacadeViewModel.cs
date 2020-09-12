using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class FacadeViewModel : BaseViewModel<int>
    {
       
        public string Name { get; set; }

        public string ImageUrl { get; set; }


        public string RelativeImageUrl { get; set; }
        public long Cost { get; set; }

        public string DisplayPrice
        {
            get
            {
                if (Cost == 0)
                    return "Included";
                return Cost.ToString("C0");
            }
        }

        public bool? IsSelected { get; set; }
    }
}
