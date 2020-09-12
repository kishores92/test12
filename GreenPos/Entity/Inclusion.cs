using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class Inclusion : BaseEntity<long>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Cost { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? SequenceNumber { get; set; }

        public bool? IsSummary { get; set; }

        public string Package { get; set; }

        public string FileUrl { get; set; }

    }
}
