using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GHUtils.Services
{
    public class Wildcard : Regex
    {
        public Wildcard(string pattern)
         : base(WildcardToRegex(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase)
        {
        }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
             Replace("\\*", ".*").
             Replace("\\?", ".") + "$";
        }
    }
}
