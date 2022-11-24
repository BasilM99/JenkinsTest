using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    /// <summary>
    /// Contains methods to help generate script for query execution
    /// </summary>
    internal static class RepositoryScriptHelper
    {
        public static List<DateTime> GetDates(DateTime dateFrom, DateTime dateTo, SummaryBy summaryBy)
        {
            var result = new List<DateTime>();
            switch (summaryBy)
            {
                case SummaryBy.Hour:
                    {
                        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
                        for (var date = dateFrom; date <= dateTo; date = date.AddDays(1))
                        {
                            result.Add(date);
                        }
                        break;
                    }
                case SummaryBy.Day:
                    {
                        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
                        for (var date = dateFrom; date <= dateTo; date = date.AddDays(1))
                        {
                            //result.Add(Convert.ToInt32(date.ToString("yyyyMMdd")));
                            result.Add(date);
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
                        for (var date = dateFrom; date <= dateTo; date = date.AddDays(1))
                        {
                            //this is start week
                            if (date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                //result.Add(Convert.ToInt32(date.ToString("yyyyMMdd")));
                                result.Add(date);
                            }
                        }
                        break;
                    }
                case SummaryBy.Accumulated:
                case SummaryBy.Month:
                    {
                        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
                        for (var date = dateFrom; date <= dateTo; date = date.AddDays(1))
                        {
                            //this is start month
                            if (date.Day == 1)
                            {
                                // result.Add(Convert.ToInt32(date.ToString("yyyyMMdd")));
                                result.Add(date);
                            }
                        }
                        break;
                    }
            }
            return result;
        }

        public static List<DateTime> GetPartialDates(DateTime fromDate, DateTime toDate, SummaryBy summaryBy, out DateTime newDateFrom, out DateTime newDateTo)
        {
            var result = new List<DateTime>();
            var dateFrom = fromDate;
            var dateTo = toDate;
            newDateFrom = dateFrom;
            newDateTo = dateTo;
            if (dateTo == dateFrom)
            {
                result.Add(newDateTo);
            }
            else
            {
                switch (summaryBy)
                {
                    case SummaryBy.Week:
                        {
                            for (; newDateFrom <= dateTo; newDateFrom = newDateFrom.AddDays(1))
                            {
                                //this is start week
                                if (newDateFrom.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    break;
                                }
                                else
                                {
                                    result.Add(newDateFrom);
                                }
                            }
                            for (; newDateTo > dateFrom; newDateTo = newDateTo.AddDays(-1))
                            {
                                //this is end week
                                if (newDateTo.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    break;
                                }
                                else
                                {
                                    result.Add(newDateTo);
                                }
                            }
                            break;
                        }
                    case SummaryBy.Accumulated:
                    case SummaryBy.Month:
                        {
                            var endMonth = dateTo.Month;
                            if (newDateFrom.Month == dateTo.Month)
                            {
                                for (; newDateFrom <= dateTo; newDateFrom = newDateFrom.AddDays(1))
                                {
                                    result.Add(newDateFrom);
                                }
                            }
                            else
                            {
                                for (; newDateFrom <= dateTo; newDateFrom = newDateFrom.AddDays(1))
                                {
                                    //this is start week
                                    if (newDateFrom.Day == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        result.Add(newDateFrom);
                                    }
                                }
                                var lastDay = new DateTime(dateTo.Year, dateTo.Month, 1).AddMonths(1).AddDays(-1);
                                if (lastDay.CompareTo(dateTo) != 0)
                                {
                                    if (dateFrom.Month != dateTo.Month)
                                    {
                                        for (;
                                            (endMonth == newDateTo.Month && newDateTo > dateFrom);
                                            newDateTo = newDateTo.AddDays(-1))
                                        {
                                            //this is end week
                                            result.Add(newDateTo);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            return result;
        }

        public static Dictionary<string, string> GetDateTables(BaseGeneratorArgs args, List<DateTime> dates, SummaryBy summaryBy)
        {
            var result = new Dictionary<string, string>();
            var tableName = args.DayFactStatTable;
            foreach (var date in dates)
            {
                switch (summaryBy)
                {
                    case SummaryBy.Hour:
                        {
                            var dateWeekStart = StartOfWeek(date, DayOfWeek.Sunday);
                            tableName = args.FactStatTable + dateWeekStart.ToString("_yyyy_MM_dd");
                            break;
                        }
                    case SummaryBy.Day:
                        {
                            var dateMonthStart = new DateTime(date.Year, date.Month, 1);
                            tableName = args.DayFactStatTable + dateMonthStart.ToString("_yyyy_MM_dd");
                            break;
                        }
                    case SummaryBy.Week:
                        {
                            tableName = args.WeekFactStatTable;
                            break;
                        }
                    case SummaryBy.Accumulated:
                    case SummaryBy.Month:
                        {
                            tableName = args.MonthFactStatTable;
                            break;
                        }
                }
                if (result.ContainsKey(tableName))
                {
                    result[tableName] += date.ToString("yyyyMMdd") + ",";
                }
                else
               {
                    result.Add(tableName, date.ToString("yyyyMMdd") + ",");
                }
            }
            foreach (var key in result.Keys.ToList())
            {
                result[key] = result[key].Trim(',');
            }
            return result;
        }

        private static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

    }
}
