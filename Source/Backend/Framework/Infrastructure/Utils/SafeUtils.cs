/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
#pragma warning disable CA1031

namespace Framework.Infrastructure.Utils
{
    public static class SafeUtils
    {
        public static int Int(this string obj, int defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                if (int.TryParse(obj, out int result))
                {
                    return result;
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static int Int(this string obj)
        {
            return Int(obj, 0);
        }

        public static int Int(object obj, int defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static List<int> Int(List<string> objList, int defaultValue = 0)
        {
            List<int> result = new List<int>();
            foreach (var obj in objList)
            {
                if (obj == null)
                {
                    result.Add(defaultValue);
                    continue;
                }

                try
                {
                    if (int.TryParse(obj, out int res))
                    {
                        result.Add(res);
                        continue;
                    }

                    result.Add(defaultValue);
                }
                catch (Exception)
                {
                    result.Add(defaultValue);
#pragma warning disable S3626 // Jump statements should not be redundant
                    continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                }
            }

            return result;
        }

        public static bool Bool(string obj, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }

            var bstr = obj.Trim().ToUpper();
            if ((bstr == "ON") || (bstr == "T") || (bstr == "TRUE") || (bstr == "Y") || (bstr == "YES") || (bstr == "1") || (Int(bstr) > 0))
            {
                return true;
            }

            return false;
        }

        public static bool Bool(this object obj, bool defaultValue = false)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                if (obj is string s)
                {
                    return Bool(s, defaultValue);
                }

                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static ushort UShort(this string obj, ushort defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                if (ushort.TryParse(obj, out ushort result))
                {
                    return result;
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static short Short(this object obj, short defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt16(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static long Long(this object obj, long defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static decimal Decimal(this string obj, decimal defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                if (decimal.TryParse(obj, out decimal result))
                {
                    return result;
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static float Float(this object obj, float defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSingle(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static double Double(this object obj, double defaultValue = 0.0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T Enum<T, TValue>(TValue enumValueToConvert, object defaultValue)
        {
            var enumValue = default(T);
            try
            {
                var str = enumValueToConvert.ToString();
                return Enum<T>(str, defaultValue);
            }
            catch (Exception)
            {
                return enumValue;
            }
        }

        public static T Enum<T>(string enumString, object defaultValue)
        {
            T enumValue = default;

            if (!string.IsNullOrEmpty(enumString))
            {
                try
                {
                    enumValue = (T)System.Enum.Parse(typeof(T), enumString, true);
                }
                catch (Exception)
                {
                    if (defaultValue != null)
                    {
                        enumValue = (T)System.Enum.ToObject(typeof(T), defaultValue);
                    }
                }
            }
            else
            {
                if (defaultValue != null)
                {
                    enumValue = (T)System.Enum.ToObject(typeof(T), defaultValue);
                }
            }

            return enumValue;
        }

        public static T Enum<T>(int enumValue, object defaultValue)
        {
            T enumRet;
            try
            {
                enumRet = (T)System.Enum.ToObject(typeof(T), enumValue);
            }
            catch (Exception)
            {
                enumRet = (T)System.Enum.ToObject(typeof(T), defaultValue);
            }

            return enumRet;
        }

        public static object Enum(Type enumType, string enumString, object defaultValue)
        {
            object enumValue = null;

            if (!string.IsNullOrEmpty(enumString))
            {
                try
                {
                    enumValue = System.Enum.Parse(enumType, enumString);
                }
                catch (Exception)
                {
                    if (defaultValue != null)
                    {
                        enumValue = System.Enum.ToObject(enumType, defaultValue);
                    }
                }
            }
            else
            {
                if (defaultValue != null)
                {
                    enumValue = System.Enum.ToObject(enumType, defaultValue);
                }
            }

            return enumValue;
        }

        public static object Enum(Type enumType, int enumValue, object defaultValue)
        {
            try
            {
                return System.Enum.ToObject(enumType, enumValue);
            }
            catch (Exception)
            {
                return System.Enum.ToObject(enumType, defaultValue);
            }
        }

        public static TimeSpan Timespan(object objValue)
        {
            var strValue = objValue as string;
            return Timespan(strValue);
        }

        public static TimeSpan Timespan(string strValue)
        {
            if (TimeSpan.TryParse(strValue, CultureInfo.CurrentCulture, out TimeSpan tValue))
                return tValue;

            return TimeSpan.MinValue;
        }

        public static Guid Guid(string value, Guid? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = System.Guid.Empty;

            try
            {
                if (System.Guid.TryParse(value, out Guid result))
                {
                    return result;
                }

                return defaultValue.Value;
            }
            catch (Exception)
            {
                return defaultValue.Value;
            }
        }

        public static Guid Guid(object value, System.Guid? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = System.Guid.Empty;

            if (value == null)
                return defaultValue.Value;

            return Guid(value.ToString(), defaultValue);
        }

        public static DateTime DateTime(string obj, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }

            try
            {
                if (System.DateTime.TryParse(obj, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime DateTime(DateTime? obj, DateTime defaultValue)
        {
            if (!DateUtils.IsValidDate(obj))
            {
                return defaultValue;
            }

            try
            {
                return obj.Value;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime DateTime(string obj)
        {
            return DateTime(obj, System.DateTime.MinValue);
        }

        public static T To<T>(string v, T defaultValue = default(T))
        {
            try
            {
                var t = typeof(T);
                var tc = TypeDescriptor.GetConverter(t);
                return (T)tc.ConvertFrom(v);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
#pragma warning restore CA1031