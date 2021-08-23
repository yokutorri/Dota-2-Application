using System;
using System.Collections.Generic;
using System.Text;

namespace Dota_2_Application.Utils
{
    static class StringUtils
    {
        public static string TruncateLongString(this string inputString, int maxChars, string postfix = "...")
        {
            if (maxChars <= 0)
                throw new ArgumentOutOfRangeException("maxChars");
            if (inputString == null || inputString.Length < maxChars)
                return inputString;

            var truncatedString = inputString.Substring(0, maxChars) + postfix;

            return truncatedString;

        }
    }
}
