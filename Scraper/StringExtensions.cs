using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public static class StringExtensions
    {
        public static string StripDiacritics(this string InputString)
        {
            if (InputString == null) return null;

            InputString = InputString.Normalize(NormalizationForm.FormD);
            StringBuilder outputString = new StringBuilder(InputString.Length);

            for (int i = 0; i < InputString.Length; ++i)
            {
                if (char.GetUnicodeCategory(InputString, i) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    if (!char.IsSurrogatePair(InputString, i))
                    {
                        outputString.Append(InputString[i]);
                    }
                    else
                    {
                        outputString.Append(InputString, i, 2);
                        ++i;
                    }
                }
            }
            return outputString.ToString().Normalize();
        }
    }
}
