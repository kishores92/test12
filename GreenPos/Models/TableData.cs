using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class TableData
    {
        
        public TableData()
        {
            CssClass = "header-text";
        }
        public string RowNumber { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public decimal Cost { get; set; }

        public string CssClass { get; set; }

        public string SubTotalCss { get; set; }


    }
}
