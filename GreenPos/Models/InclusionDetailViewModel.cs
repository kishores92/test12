using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class InclusionDetailViewModel
    {
        public int Id { get; set; }
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

        public int? InclusionId { get; set; }

        public bool IsIncluded { get; set; }
        public bool IsNoteOnly { get; set; }

        public int? QuoteId { get; set; }
    }
}
