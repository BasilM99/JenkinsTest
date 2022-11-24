using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities
{
    public static class StringUtilities
    {
        public static string SubStringWithFullWord(string current, int length)
        {
            if (string.IsNullOrEmpty(current) || current.Length <= length)
                return current;

            int i = 0;
            current = current.Substring(0, length);

            for (int len = current.Length - 1; len >= 0; len--)
            {
                if (char.IsWhiteSpace(current[len]))
                {
                    break;
                }

                i++;
            }

            return current.Substring(0, length - i);
        }
    }
}
