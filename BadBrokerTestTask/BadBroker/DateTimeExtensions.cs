using System;

namespace BadBroker
{
    public static class DateTimeExtensions
    {
        public static double GetBusinessDaysCount(this DateTime startDay, DateTime endDay)
        {
            var calcBusinessDays =
                1 + ((endDay - startDay).TotalDays * 5 -
                     (startDay.DayOfWeek - endDay.DayOfWeek) * 2) / 7;

            if (endDay.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startDay.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays;
        }
    }
}