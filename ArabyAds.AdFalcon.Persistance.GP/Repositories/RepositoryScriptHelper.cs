using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP
{
    /// <summary>
    /// Contains methods to help generate script for query execution
    /// </summary>
    public static class RepositoryScriptHelper
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
           // System.Globalization.GregorianCalendar Cal;

         
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
        /// <summary>
        /// Calculates ISO week and year number (ISO year can differ from calendar year)
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="year">ISO year</param>
        /// <param name="wk">ISO week</param>
        /// <seealso cref="http://codebetter.com/petervanooijen/2005/09/26/iso-weeknumbers-of-a-date-a-c-implementation/"/>
        public static void GetWeekNumber(DateTime date, out int year, out int wk)
        {
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            year = 0;

            wk = myCal.GetWeekOfYear(date, myCWR, myFirstDOW);
            //year = date.Year;
            //// Get jan 1st of the year
            //DateTime startOfYear = new DateTime(year, 1, 1);
            //// Get dec 31st of the year
            //DateTime endOfYear = new DateTime(year, 12, 31);
            //// ISO 8601 weeks start with Monday 
            //// The first week of a year includes the first Thursday 
            //// DayOfWeek returns 0 for sunday up to 6 for saterday
            //int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            //int nds = date.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            //wk = nds / 7;
            //switch (wk)
            //{
            //    case 0:
            //        // Return weeknumber of dec 31st of the previous year
            //        GetWeekNumber(startOfYear.AddDays(-1), out year, out wk);
            //        break;
            //    case 53:
            //        // If dec 31st falls before thursday it is week 01 of next year
            //        if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
            //        {
            //            wk = 1; year += 1;
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }
        public static int GetIso8601WeekNumber(this DateTime date)
        {
            var thursday = date.AddDays(3 - ((int)date.DayOfWeek + 6) % 7);
            return 1 + (thursday.DayOfYear - 1) / 7;
        }
        public static Dictionary<string, string> GetDateTables(BaseGeneratorArgs args, List<DateTime> dates, SummaryBy summaryBy,   List<int> weeknumber=null)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.InvariantInfo;
            weeknumber = new List<int>();
               Calendar cal = dfi.Calendar;

            var result = new Dictionary<string, string>();
            var tableName = args.DayFactStatTable;
            switch (summaryBy)
            {
                case SummaryBy.Hour:
                    {
                        //var dateWeekStart = StartOfWeek(date, DayOfWeek.Sunday);
                        tableName = args.FactStatTable;
                        break;
                    }
                case SummaryBy.Day:
                    {
                        //var dateMonthStart = new DateTime(date.Year, date.Month, 1);
                        tableName = args.DayFactStatTable;
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
            string FormatDate = string.Empty;
            int w = 0;
            int y = 0;
            foreach (var date in dates)
            {
               // if (summaryBy == SummaryBy.Day || summaryBy == SummaryBy.Hour)
                    FormatDate = date.ToString("yyyyMMdd") + ",";
                //else
                //{
                //    FormatDate = date.ToString("yyyyMM01") + ",";

                //    if (summaryBy == SummaryBy.Week)
                //    {

                //       // GetWeekNumber(date, out y, out w);
                //        //weeknumber.Add(w);

                //        date.StartOfWeek(DayOfWeek.Sunday);
                //        FormatDate = date.ToString("yyyyMMdd") + ",";
                //    }
                //    else
                //    {

                //        FormatDate = date.ToString("yyyyMM01") + ",";
                //    }


                //}
                if (result.ContainsKey(tableName))
                {
                    result[tableName] += FormatDate;
                }
                else
               {
                    result.Add(tableName, FormatDate);
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
