using System;
using System.Security.Cryptography;
using System.Text;
using BtcMarkets.Core.Api;

namespace BtcMarkets.Core.Helpers
{
    public static class ApiHelper
    {
        public static TimeZoneInfo LocalTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
        public static string SignData(string data, string secret)
        {
            var encoding = Encoding.UTF8;
            using (var hash = new HMACSHA512(Convert.FromBase64String(secret)))
            {
                return Convert.ToBase64String(hash.ComputeHash(encoding.GetBytes(data)));
            }
        }

        public static string GetTimeStamp(DateTime? date = null)
        {
            var nonce = GetTimeStampLong(date).ToString();
            return nonce;
        }

        public static long GetTimeStampLong(DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.UtcNow;
            }
            else
            if (date.Value.Kind != DateTimeKind.Utc)
            {
                date = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
            }

            var unixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondsSinceEpoch = (long)(date.Value - unixTime).TotalMilliseconds;
            return secondsSinceEpoch;

        }

        public static DateTime ToLocalTime(long utcTimestamp)
        {
            DateTime dt = GetDate(utcTimestamp);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(dt, LocalTimeZone);
            return localTime;
        }
        public static DateTime GetDate(string milliseconds)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(Convert.ToDouble(milliseconds));
        }
        public static DateTime GetDate(long milliseconds)
        {
            milliseconds = FromLongTimeStamp(milliseconds);

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(Convert.ToDouble(milliseconds));
        }

        public static long ToLongValue(double value)
        {
            return (long)(value * ApiConstants.CurrencyDecimal);
        }

        public static double ToDoubleValue(long value)
        {
            return value / ApiConstants.CurrencyDecimal;
        }
        public static long ToLongTimeStamp(long value)
        {
            if (Math.Floor(Math.Log10(value) + 1) <= 10)
            {
                value *= ApiConstants.TimestampDenom;
            }

            return value;
        }

        public static long FromLongTimeStamp(long value)
        {
            long val = value;
            if (Math.Floor(Math.Log10(val) + 1) > 10)
            {
                val /= ApiConstants.TimestampDenom;
            }

            return val;
        }
        public static long? FromLongTimeStamp(long? value)
        {
            if (!value.HasValue)
                return value;

            long val = value.Value;
            if (Math.Floor(Math.Log10(val) + 1) > 10)
            {
                val /= ApiConstants.TimestampDenom;
            }

            return val;
        }
        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }

        public static string ToRelativeTime(DateTime date)
        {
            var dt = date;
            var to = DateTime.UtcNow;

            var ts = to - dt;
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * ApiConstants.MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * ApiConstants.MINUTE)
                return "a minute ago";

            if (delta < 45 * ApiConstants.MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * ApiConstants.MINUTE)
                return "an hour ago";

            if (delta < 24 * ApiConstants.HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * ApiConstants.HOUR)
                return "yesterday";

            if (delta < 30 * ApiConstants.DAY)
                return ts.Days + " days ago";

            if (delta < 12 * ApiConstants.MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        public static string ToRelativeTime(long? timestamp)
        {
            if (!timestamp.HasValue)
                return string.Empty;

            var val = timestamp.Value;

            var dt = GetDate(val);

            var str = ToRelativeTime(dt);

            return str;
        }
    }
}
