/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework.Infrastructure.Utils
{
    public static class ValidationUtils
    {
        public static readonly Regex AddressRegex = new Regex(@"^[A-Za-z0-9\s\.,#\-\/]+$");
        public static readonly Regex USAPhoneNumberRegex = new Regex(@"^(\(?\d{3}\)?) ?-?\d{3}-?\d{4}$");

        public static readonly Regex IndianPhoneNumberRegex = new Regex(@"^(\+?91|0091|0)?[-\s]?(\d{2,3})?[-\s]?(\d{2,10})[-\s]?(\d{0,10})[-\s]?(\d{0,10})$");

        public static readonly Regex NameAndSpecialCharacterRegex = new Regex(@"^(?=.*[a-zA-Z])[\w\s\-\'\.\,\&\@\#\!\$\%\^\*\(\)\+\=\[\]\{\}\|\:\;\<\>\?\/\\]+$");

        public static readonly Regex USAZipcodeRegex = new Regex(@"^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$");

        public static readonly Regex NameRegex = new Regex(@"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$");

        public static readonly Regex URLRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$");

        public static readonly Regex IndianZipCodeRegex = new Regex(@"^\d{6}$");

        public static bool IsNumeric(this string expression)
        {
            return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double _);
        }

        public static bool IsNullOrFalse(this bool? value)
        {
            if (value == true)
                return false;

            return true;
        }

        public static bool IsTrue(this bool? value)
        {
            if (value == true)
                return true;

            return false;
        }

        public static bool IsLongValid(long? value)
        {
            if (value == null)
                return false;

            return value > 0;
        }

        public static bool IsWithinLongRange(long? value)
        {
            if (value == null)
                return false;

            return value > long.MinValue && value < long.MaxValue;
        }

        public static bool IsWithinDecimalRange(decimal? value)
        {
            if (value == null)
                return false;

            const decimal maxValue = 99999999999999999m;
            if (value > maxValue || value < 0)
            {
                return false;
            }

            return true;
        }

        public static bool IsGuidValid(object guid)
        {
            var g = SafeUtils.Guid(guid, Guid.Empty);
            return g != Guid.Empty;
        }

        public static bool IsEmail(string str)
        {
            return new EmailAddressAttribute().IsValid(str);
        }

        public static bool IsEnumValid<T>(int enumValue)
        {
            var typeValue = SafeUtils.Enum<T>(enumValue, null);
            return Enum.IsDefined(typeof(T), typeValue);
        }

        public static bool IsEnumValid<T>(string enumValue)
        {
            var typeValue = SafeUtils.Enum<T>(enumValue, null);
            return Enum.IsDefined(typeof(T), typeValue);
        }

        public static bool IsEnumValid<T, TValue>(TValue enumValue)
            where T : Enum
        {
            var typeValue = SafeUtils.Enum<T, TValue>(enumValue, null);
            return Enum.IsDefined(typeof(T), typeValue);
        }

        public static bool IsEnumValid<T>(List<int> enumValues)
        {
            var res = enumValues.SelectToList(x => Enum.IsDefined(typeof(T), SafeUtils.Enum<T>(x, null)));
            if (res.Contains(false))
                return false;
            return true;
        }

        public static bool IsEnumValid<T>(List<string> enumValues)
        {
            var res = enumValues.SelectToList(x => Enum.IsDefined(typeof(T), SafeUtils.Enum<T>(x, null)));
            if (res.Contains(false))
                return false;
            return true;
        }

        public static bool IsDoubleSameAsZero(double d, int numerOdDigits = 2)
        {
            long v = (long)(d * Math.Pow(10, numerOdDigits) / Math.Pow(10, numerOdDigits));
            return v == 0;
        }

        public static bool IsFloatSameAsZero(float d, int numerOdDigits = 2)
        {
            long v = (long)(d * Math.Pow(10, numerOdDigits) / Math.Pow(10, numerOdDigits));
            return v == 0;
        }

        public static bool IsGuidListValid(List<Guid> guidList)
        {
            foreach (var guid in guidList)
            {
                if (!Guid.TryParse(guid.ToString(), out Guid _))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsValidRgbComponents(long? red, long? green, long? blue)
        {
            return red >= 0 && red <= 255 &&
                   green >= 0 && green <= 255 &&
                   blue >= 0 && blue <= 255;
        }

        public static bool IsInteger(string numStr)
        {
            return long.TryParse(numStr, out long _);
        }

        public static bool IsPositiveInteger(string numStr)
        {
            if (!long.TryParse(numStr, out long num))
                return false;
            return num > 0;
        }

        public static bool IsPositiveIntegerOrZero(string numStr)
        {
            if (!long.TryParse(numStr, out long num))
                return false;
            return num >= 0;
        }

        public static bool IsNegativeInteger(string numStr)
        {
            if (!long.TryParse(numStr, out long num))
                return false;
            return num < 0;
        }

        public static bool IsDouble(string numStr)
        {
            return double.TryParse(numStr, out _);
        }

        public static bool IsBoolean(string numStr)
        {
            if (numStr.IsTrimmedStringNullOrEmpty())
                return false;

            var u = numStr.ToUpper();
            if (u == "T" || u == "TRUE" || u == "1" || u == "Y" || u == "YES" || u == "F" || u == "FALSE" || u == "0" || u == "N" || u == "NO")
                return true;

            return bool.TryParse(numStr, out _);
        }

        public static bool IsPositiveDouble(string numStr)
        {
            if (!double.TryParse(numStr, out double num))
                return false;
            return num >= 0;
        }

        public static bool IsNegativeDouble(string numStr)
        {
            if (!double.TryParse(numStr, out double num))
                return false;
            return num < 0;
        }

        public static bool IsUrlValid(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }

        public static bool IsUrlValidRegex(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return Match(url, URLRegex);
        }

        public static bool IsUSAPhoneNumber(string value)
        {
            return Match(value, USAPhoneNumberRegex);
        }

        public static bool IsUSAZipcode(string value)
        {
            return Match(value, USAZipcodeRegex);
        }

        public static bool IsIndianPhoneNumber(string value)
        {
            return Match(value, IndianPhoneNumberRegex);
        }

        public static bool IsValidName(string name)
        {
            return Match(name, NameRegex);
        }

        public static bool Match(string value, Regex regex)
        {
            return value.IsNotNullOrEmpty() && regex.IsMatch(value);
        }
    }
}
