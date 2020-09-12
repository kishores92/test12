using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Common
{
    public static class CommonHelper
    {
        public static DateTime CurrentDateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
