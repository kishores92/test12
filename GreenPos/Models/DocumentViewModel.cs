using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class DocumentViewModel : BaseViewModel<long>
    {
        private const string baseUrl = @"https://dhdocgen.blob.core.windows.net/quotes/";
        public string Title { get; set; }

        public string Name { get; set; }

        public long QuoteId { get; set; }

        public string Url
        {
            get
            {
                return baseUrl + Name;
            }
        }
    }
}
