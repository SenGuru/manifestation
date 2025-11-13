/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;

namespace Framework.Infrastructure.Utils
{
    public static class CurrencyUtils
    {
        public const double MaxValue = 1000000000000;

        public static readonly string[] Units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        public static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static bool IsCurrencyExceededMaxValue(double amt)
        {
            return amt > MaxValue;
        }

        public static string ConvertToWords(double number)
        {
            if (ValidationUtils.IsDoubleSameAsZero(number))
                return "Zero Rupees";

            int intPart = (int)number;
            int fractionalPart = (int)((number - intPart) * 100);

            string words = Convert(intPart) + " Rupees";

            if (fractionalPart > 0)
            {
                words += " and " + Convert(fractionalPart) + " Paise";
            }

            return words;
        }

        private static string Convert(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + Convert(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += Convert(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += Convert(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += Convert(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += Convert(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                {
                    words += Units[number];
                }
                else
                {
                    words += Tens[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + Units[number % 10];
                }
            }

            return words.Trim();
        }
    }
}
