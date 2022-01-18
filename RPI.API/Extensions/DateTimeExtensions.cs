using System;

namespace RPI.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertTime(this double timestampLinux)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestampLinux).ToLocalTime();

            return dateTime;
        }

    }
}
