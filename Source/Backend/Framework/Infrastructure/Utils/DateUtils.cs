/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Framework.Infrastructure.Utils
{
    public static class DateUtils
    {
        public const int SECOND = 1;
        public const int MINUTE = 60 * SECOND;
        public const int HOUR = 60 * MINUTE;
        public const int DAY = 24 * HOUR;
        public const int MONTH = 30 * DAY;

        public static readonly DateTime InvalidDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime PastDate = new DateTime(1977, 1, 23, 0, 0, 0, DateTimeKind.Local);
        public static readonly TimeZoneInfo IndianZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");

        public static DateTime GetIndianDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, IndianZone);
        }

        public static DateTime ToIndianTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, IndianZone);
        }

        public static DateTime ThisMonthStart()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local);
        }

        public static DateTime ThisMonthEnd()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, ThisMonthStart().AddMonths(1).AddDays(-1).Day, 0, 0, 0, DateTimeKind.Local);
        }

        public static bool IsInvalidDate(this string dtStr)
        {
            var dt = SafeUtils.DateTime(dtStr, DateTime.MinValue);
            return IsInvalidDate(dt);
        }

        public static bool IsInvalidDate(this DateTime? dt)
        {
            return (dt == null) || (dt == InvalidDate) || (dt == DateTime.MinValue) || (dt == DateTime.MaxValue);
        }

        public static bool IsInvalidDate(this DateTime dt)
        {
            return (dt == InvalidDate) || (dt == DateTime.MinValue) || (dt == DateTime.MaxValue);
        }

        public static bool IsValidDate(this DateTime? dt)
        {
            return !IsInvalidDate(dt);
        }

        public static bool IsValidDate(this DateTime dt)
        {
            return !IsInvalidDate(dt);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, DateTimeKind.Local);
        }

        public static string SecondsToHour(double value)
        {
            if (ValidationUtils.IsDoubleSameAsZero(value))
            {
                return "";
            }

            double hours = Math.Floor(value / 3600);
            double minutes = Math.Floor((value % 3600) / 60);
            string period = hours >= 12 ? "PM" : "AM";
            // Convert hour from 24-hour format to 12-hour format
            double displayHour = (long)hours % 12 == 0 ? 12 : (long)hours % 12;
            string hourFormat = $"{displayHour}".PadLeft(2, '0');
            string minFormat = $"{minutes}".PadLeft(2, '0');
            // Format hours, minutes into a string
            string formattedTime = $"{hourFormat}:{minFormat} {period}";
            return formattedTime;
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, DateTimeKind.Local);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            var dt = new DateTime(date.Year, date.Month + 1, 1, 23, 59, 59, 999, DateTimeKind.Local);
            return dt.AddDays(-1);
        }

        public static DateTime? EndOfMonth(this DateTime? date)
        {
            if (date == null)
                return date;
            var dt = EndOfMonth(date.Value);
            return dt.AddDays(-1);
        }

        public static DateTime EndOfYear(this DateTime date)
        {
            var dt = new DateTime(date.Year, 12, 1, 23, 59, 59, 999, DateTimeKind.Local);
            return dt;
        }

        public static DateTime? EndOfYear(this DateTime? date)
        {
            if (date == null)
                return date;
            return EndOfYear(date.Value);
        }

        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, DateTimeKind.Local);
        }

        public static DateTime? StartOfMonth(this DateTime? date)
        {
            if (date == null)
                return date;
            return StartOfMonth(date.Value);
        }

        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        }

        public static DateTime? StartOfYear(this DateTime? date)
        {
            if (date == null)
                return date;
            return StartOfYear(date.Value);
        }

        public static bool IsSameDay(this DateTime? src, DateTime? des)
        {
            return src.Value.StartOfDay() == des.Value.StartOfDay();
        }

        public static DateTime UpdateDatePart(this DateTime date, DateTime newDatePart)
        {
            return new DateTime(newDatePart.Year, newDatePart.Month, newDatePart.Day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Local);
        }

        public static DateTime UpdateDatePart(this DateTime date, int year, int month, int day)
        {
            return new DateTime(year, month, day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Local);
        }

        public static string ToUtcDateFullInfoStringWithDash()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        }

        public static string ToFullInfoStringWithDash(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        }

        public static string ToFullInfoStringWithoutDash(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssfff");
        }

        public static string ToReadableString(this TimeSpan span)
        {
            var dur = span.Duration();
            var sb = new StringBuilder();
            if (dur.Days > 0)
                sb.AppendFormat("{0:0} day{1}, ", span.Days, span.Days == 1 ? "" : "s");
            if (dur.Hours > 0)
                sb.AppendFormat("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? "" : "s");
            if (dur.Minutes > 0)
                sb.AppendFormat("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? "" : "s");
            if (dur.Seconds > 0)
                sb.AppendFormat("{0:0} second{1}, ", span.Seconds, span.Seconds == 1 ? "" : "s");
            if (sb.Length >= 2)
                sb.Length -= 2;

            var result = sb.ToString();
            if (result.IsNullOrEmpty())
                return "0 seconds";
            else
                return result;
        }

        public static string FriendlyRelativeTime(this DateTime date)
        {
            if (date > DateTime.UtcNow)
            {
                return "in the future";
            }

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
            {
                return "not yet";
            }

            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }

            if (delta < 2 * MINUTE)
            {
                return "a minute ago";
            }

            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }

            if (delta < 90 * MINUTE)
            {
                return "an hour ago";
            }

            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }

            if (delta < 48 * HOUR)
            {
                return "yesterday";
            }

            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }

            if (delta < 12 * MONTH)
            {
                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }

            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

        public static bool IsBefore(this DateTime source, DateTime other)
        {
            return source.CompareTo(other) < 0;
        }

        public static bool IsBefore(this DateTime? source, DateTime other)
        {
            if (source == null)
                return false;
            return IsBefore(source.Value, other);
        }

        public static bool IsAfter(this DateTime source, DateTime other)
        {
            return source.CompareTo(other) > 0;
        }

        public static bool IsAfter(this DateTime? source, DateTime other)
        {
            return IsAfter(source.Value, other);
        }

        public static DateTime AddDaysExcludingWeekends(this DateTime source, long noOfDays)
        {
            var noOfDaysWithWeekends = noOfDays + (Math.Floor(noOfDays / 5d) * 2);

            if (source.DayOfWeek == DayOfWeek.Saturday)
            {
                noOfDaysWithWeekends += 2;
            }
            else if (source.DayOfWeek == DayOfWeek.Sunday)
            {
                noOfDaysWithWeekends += 1;
            }

            var newDate = source.AddDays(noOfDaysWithWeekends);

            if (newDate.DayOfWeek == DayOfWeek.Saturday)
            {
                newDate.AddDays(2);
            }
            else if (newDate.DayOfWeek == DayOfWeek.Sunday)
            {
                newDate.AddDays(1);
            }

            return newDate;
        }

        public static DateTime WeekStart(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            var diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return date.AddDays(-1 * diff).Date;
        }

        public static DateTime WeekEnd(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            return date.WeekStart(startOfWeek).AddDays(7);
        }

        public static DateTime CurrentWeekStart(DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            return DateTime.Now.WeekStart(startOfWeek);
        }

        public static DateTime CurrentWeekEnd(DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            return DateTime.Now.WeekEnd(startOfWeek);
        }

        public static int Age(DateTime date)
        {
            if (date.IsInvalidDate())
            {
                return -1;
            }

            var today = DateTime.Today;
            return today.Year - date.Year;
        }

        public static DateTime? ParseDateTime(string dateTimeString)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParse(dateTimeString, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDateTime))
            {
                return parsedDateTime;
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static bool IsFutureDate(DateTime date)
        {
            return date > DateTime.Now;
        }

        public static bool OverlapsWith(DateTime firstStartDate, DateTime firstEndDate, DateTime secondStartDate, DateTime secondEndDate)
        {
            return firstStartDate <= secondEndDate && secondStartDate <= firstEndDate;
        }

        public static DateTime GetFirstDateOfWeek(int year, int week)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var firstDayOfWeek = firstDayOfYear.AddDays(week * 7);
            var dayOfWeek = (int)firstDayOfWeek.DayOfWeek;
            var startOfWeek = firstDayOfWeek.AddDays(dayOfWeek == 0 ? -6 : 1 - dayOfWeek);

            return startOfWeek;
        }

        public static int ConvertTimeToSeconds(this DateTime dateTime)
        {
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;

            var totalSeconds = (hours * 3600) + (minutes * 60) + seconds;

            return totalSeconds;
        }
    }
}
