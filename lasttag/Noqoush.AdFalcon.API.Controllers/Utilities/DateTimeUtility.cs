using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.API.Controllers.Utilities
{
    public class DateTimeUtility
    {
        public static string FixDate(DateTime date,int? timeId, DateTime criteriaFromDate,int summaryBy)
        {
            var apiDateFormat = Config.APIDateTimeFormat;

            var DateRange = string.Empty;
            switch (summaryBy)
            {
                case 0:
                    DateRange = (new DateTime(date.Year, date.Month, date.Day, timeId.Value, 0, 0)).ToString(apiDateFormat);
                    break;
                case 1:
                    {
                        DateRange = date.ToString(apiDateFormat);
                        break;
                    }
                case 2:
                    {
                        DateTime weekStart;
                        GetWorkWeekDates(date, out weekStart, criteriaFromDate);
                        DateRange = weekStart.ToString(apiDateFormat);
                        break;
                    }
                case 3:
                    {
                        DateTime monthStart;
                        GetWorkMonthDates(date, out monthStart, criteriaFromDate);
                        DateRange = monthStart.ToString(apiDateFormat);
                        break;
                    }
            }
            return DateRange;
        }
        // ---- GetWorkWeekDates -----------------------
        private static void GetWorkWeekDates(DateTime Input, out DateTime Start, DateTime criteriaFromDate)
        {
            while (Input.Date.DayOfWeek != DayOfWeek.Sunday)
                Input = Input.Date.AddDays(-1);

            Start = Input;

            if (criteriaFromDate.CompareTo(Start) > 0)
            {
                Start = criteriaFromDate;
            }
        }
        private static void GetWorkMonthDates(DateTime Input, out DateTime Start, DateTime criteriaFromDate)
        {
            Start = new DateTime(Input.Year, Input.Month, 1);

            if (criteriaFromDate.CompareTo(Start) > 0)
            {
                Start = criteriaFromDate;
            }
        }
    }
}
