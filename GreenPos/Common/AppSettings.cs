using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Common
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public string SendGridKey { get; set; }

        public string IronPdfKey { get; set; }
        public string AzureDataStorageConnection { get; set; }
        
    }
}
