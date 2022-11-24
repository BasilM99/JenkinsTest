using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    public class BaseReportRepository : RepositoryBase<ChartDto, int> 
    {
        protected ArabyAds.Framework.ConfigurationSetting.IConfigurationManager ConfigurationManager = null;
        static readonly object LockObj = new object();


        private string LongDateFormat
        {
            get
            {
                var key = "LongDateFormat-CacheKey";
                var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    lock (LockObj)
                    {
                        value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = ConfigurationManager.GetConfigurationSetting(null, null, "LongDateFormat");
                            Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                        }
                    }
                }
                return value;
            }
        }

        private string ShortDateFormat
        {
            get
            {
                var key = "ShortDateFormat-CacheKey";
                var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    lock (LockObj)
                    {
                        value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = ConfigurationManager.GetConfigurationSetting(null, null, "ShortDateFormat");
                            Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                        }
                    }
                }
                return value;
            }
        }

        public BaseReportRepository(RepositoryImplBase<ChartDto, int> repository, ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager)
            : base(repository)
        {
            ConfigurationManager = configurationManager;
        }

        protected List<T> GetResult<T>(string script)
        {
            // Create a PetaPoco database object
            using (var db = new ArabyAds.AdFalcon.Persistence.Reports.Repositories.PetaPoco.Database("ReportingDB"))
            {
                //make the timeout 5 minutes
                db.CommandTimeout = 300;

                Framework.ApplicationContext.Instance.Logger.Info(script);
                //return db.Fetch<T>("call executeScript(@0);", script.Replace("'","''")).ToList();
                return db.Fetch<T>("call executeScript(@0);", script).ToList();
            }
            //return db.Fetch<T>(script,0).ToList();
            //return new List<T>();
        }

        protected string GetDateRange(DateTime Date, ReportCriteriaDto criteriaDto)
        {
            var shortDateFormat = ShortDateFormat;
            var longDateFormat = LongDateFormat;

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "ar-JO")
            {
                var arr = shortDateFormat.ToCharArray();
                Array.Reverse(arr);
                shortDateFormat = new string(arr);
            }
            var DateRange = string.Empty;
            switch (criteriaDto.SummaryBy)
            {
                case 0:
                    DateRange = Date.ToString(longDateFormat);
                    break;
                case 1:
                    {
                        DateRange = Date.ToString(shortDateFormat);
                        break;
                    }
                case 2:
                    {
                        DateTime weekStart, weekEnd;
                        GetWorkWeekDates(Date, out weekStart, out weekEnd, criteriaDto);
                        DateRange = weekStart.ToString(shortDateFormat) + " - " + weekEnd.ToString(shortDateFormat);
                        break;
                    }
                case 4:
                case 3:
                    {
                        DateTime monthStart, monthEnd;
                        GetWorkMonthDates(Date, out monthStart, out monthEnd, criteriaDto);
                        DateRange = monthStart.ToString(shortDateFormat) + " - " + monthEnd.ToString(shortDateFormat);
                        break;
                    }
            }
            return DateRange;
        }

        protected DateTime getDate(int date, int? timeId)
        {
            if (!timeId.HasValue)
                return DateTime.ParseExact(date.ToString(), "yyyyMMdd", null);
            else
                return DateTime.ParseExact(string.Format("{0} {1}:00:00", date.ToString(), timeId.Value.ToString()), "yyyyMMdd hh:mm:ss", null);
        }


        // ---- GetWorkWeekDates -----------------------
        private void GetWorkWeekDates(DateTime Input, out DateTime Start, out DateTime End, ReportCriteriaDto criteriaDto)
        {
            while (Input.Date.DayOfWeek != DayOfWeek.Sunday)
                Input = Input.Date.AddDays(-1);

            Start = Input;
            End = Input.AddDays(6);

            if (criteriaDto.FromDate.CompareTo(Start) > 0)
            {
                Start = criteriaDto.FromDate;
            }
            if (criteriaDto.ToDate.CompareTo(End) < 0)
            {
                End = criteriaDto.ToDate;
            }
        }
        private void GetWorkMonthDates(DateTime Input, out DateTime Start, out DateTime End, ReportCriteriaDto criteriaDto)
        {
            Start = new DateTime(Input.Year, Input.Month, 1);
            End = Start.AddMonths(1).AddDays(-1);

            if (criteriaDto.FromDate.CompareTo(Start) > 0)
            {
                Start = criteriaDto.FromDate;
            }
            if (criteriaDto.ToDate.CompareTo(End) < 0)
            {
                End = criteriaDto.ToDate;
            }
        }
    }
}
