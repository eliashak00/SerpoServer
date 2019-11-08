﻿using System;
using System.Globalization;

namespace SerpoServer
{
    public static class DateTime
    {
        public static int GetIso8601WeekOfYear(this System.DateTime time)
        {

            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}