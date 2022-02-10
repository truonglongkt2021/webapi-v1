using Invedia.Core.DateTimeUtils;
using Invedia.Core.StringUtils;
using Microsoft.AspNetCore.Http;
using StreamFile.Core.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace StreamFile.Core.Utils
{
    public static class CoreHelper
    {

        public static HttpContext CurrentHttpContext =>
            Invedia.Web.Middlewares.HttpContextMiddleware.HttpContext.Current;

        public static TimeZoneInfo SystemTimeZoneInfo => DateTimeHelper.GetTimeZoneInfo(Formattings.TimeZone);

        public static DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

        public static DateTimeOffset UtcTimeNow => DateTimeOffset.Now;

        public static DateTime UtcToSystemTime(this DateTimeOffset dateTimeOffsetUtc)
        {
            return dateTimeOffsetUtc.UtcDateTime.UtcToSystemTime();
        }

        public static DateTime UtcToSystemTime(this DateTime dateTimeUtc)
        {
            var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, SystemTimeZoneInfo);

            return dateTimeWithTimeZone;
        }

        public static string GetIpClient()
        {
            return CurrentHttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string RandomTokenString()
        {
            var result = StringHelper.Generate(6, false, false, true, false);
            return result;
        }

        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            GregorianCalendar _gc = new GregorianCalendar();

            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}