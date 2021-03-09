using System;

namespace LabCenter.Shared.Utils
{
    public static class DateUtils
    {
        public static long GetDayIndex(this DateTimeOffset date) => date.Date.Ticks;
    }
}
