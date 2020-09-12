using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Service
{
    public static class TokenHelper
    {
        public static string ReplaceTokens(string html, Dictionary<string,string> tokens)
        {
            foreach(var token in tokens)
            {
                html = html.Replace($"{{" + "token.Key" + "}", token.Value);

            }
            return html;
        }
    }
}
