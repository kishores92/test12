using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class Promotion : BaseEntity<int>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Cost { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}
