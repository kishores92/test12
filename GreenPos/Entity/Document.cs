using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class Document : BaseEntity<long>
    {
       
        public string Title { get; set; }
              
        public string Name { get; set; }

        public long QuoteId { get; set; }
    }
}
