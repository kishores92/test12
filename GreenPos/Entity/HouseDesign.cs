using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class HouseDesign : BaseEntity<int>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public long Cost { get; set; }

        public int? Bed { get; set; }

        public int? Bath { get; set; }

        public int? Parking { get; set; }

        public decimal? MinBlockWidth { get; set; }

    }
}
