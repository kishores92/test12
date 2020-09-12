using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class Facade : BaseEntity<int>
    {

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public long Cost { get; set; }
    }
}
