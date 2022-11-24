using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.BigData.CardinalityEstimation;
using ArabyAds.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.ReportsGP.CardinalityEstimator
{
    public enum EstimatorCalculationPeriodType
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Accumulated = 4
    }
    public enum GetMethod
    {
        cache = 0,
        database = 1
    }

    public enum EstimatorCalculationType
    {
        Campaign = 0,
        AdGroup = 1

    }
    public class AdvertisorEstimatorCalculation
    {
        public const string CampCountSQL_d = "SELECT DISTINCT dateid AS Date,campaignid,  unique_impressions, unique_clicks FROM fact_campaigns_unique_counters_d where accountid={3} and adgroupid is null and ({0}) and dateid between {1} and {2};";
        public const string AdGoupCountSQL_d = "SELECT DISTINCT dateid AS Date, campaignid, adgroupid, unique_impressions, unique_clicks FROM fact_campaigns_unique_counters_d  where accountid={3} and ({0}) and dateid between {1} And {2};";
        public const string CampCountSQL_m = "SELECT DISTINCT monthid AS Date,campaignid,   unique_impressions, unique_clicks FROM fact_campaigns_unique_counters_m where accountid={2} and adgroupid is null and ({0}) and monthid in ({1});";
        public const string AdGoupCountSQL_m = "SELECT DISTINCT monthid AS Date, campaignid, adgroupid, unique_impressions, unique_clicks FROM fact_campaigns_unique_counters_m  where accountid={2} and ({0})  and monthid in ({1});";


        public const string CampEsimatorSQL_d = "SELECT DISTINCT dateid AS Date,campaignid,   unique_impressions, unique_clicks, impressions_estimator, clicks_estimator FROM fact_campaigns_unique_counters_d where accountid={2} and adgroupid is null and ({0}) and {1};";
        public const string AdGoupEsimatorSQL_d = "SELECT DISTINCT dateid AS Date, campaignid, adgroupid, unique_impressions, unique_clicks, impressions_estimator, clicks_estimator FROM fact_campaigns_unique_counters_d  where accountid={2} and ({0})  and {1};";
        public const string CampEsimatorSQL_m = "SELECT DISTINCT monthid AS Date,campaignid,   unique_impressions, unique_clicks, impressions_estimator, clicks_estimator FROM fact_campaigns_unique_counters_m where accountid={2} and adgroupid is null and ({0}) and {1};";
        public const string AdGoupEsimatorSQL_m = "SELECT DISTINCT monthid AS Date, campaignid, adgroupid, unique_impressions, unique_clicks, impressions_estimator, clicks_estimator FROM fact_campaigns_unique_counters_m  where accountid={2} and ({0})  and {1};";


        public const string CampCacheHYPL = "HYP_Camp_{0}_{1}";
        public const string AdGroupCacheHYPL = "HYP_AdGroup_{0}_{1}";
        public const string CampCacheHYPL_m = "HYP_Camp_{0}_{1}_m";
        public const string AdGroupCacheHYPL_m = "HYP_AdGroup_{0}_{1}_m";
        public const string CampCacheHYPLClick = "HYP_Camp_{0}_{1}Click";
        public const string AdGroupCacheHYPLClick = "HYP_AdGroup_{0}_{1}Click";
        public const string CampCacheHYPL_mClick = "HYP_Camp_{0}_{1}_mClick";
        public const string AdGroupCacheHYPL_mClick = "HYP_AdGroup_{0}_{1}_mClick";
        public const string CampCacheCount = "Count_Camp_{0}_{1}";
        public const string AdGroupCacheCount = "Count_AdGroup_{0}_{1}";
        public const string CampCacheCount_m = "Count_Camp_{0}_{1}_m";
        public const string AdGroupCacheCount_m = "Count_AdGroup_{0}_{1}_m";
        private TimeSpan time6HoursPeriod = new TimeSpan(6, 0, 0);
        private TimeSpan time1HoursPeriod = new TimeSpan(1, 0, 0);
        public DateTime DateFrom;
        public DateTime DateTo;
        public EstimatorCalculationPeriodType PeriodType;
        public EstimatorCalculationType CalculationType;
        public int AccountId;
        public Dictionary<int, DateEstimatorRange> IdPerRange;
        private string cacheStore = CacheStores.Redis;
        private string cacheName = string.Empty;
   


        public AdvertisorEstimatorCalculation(DateTime DateFromvar, DateTime DateTovar, EstimatorCalculationPeriodType PeriodTypevar, EstimatorCalculationType CalculationTypevar, int AccountIdvar)
        {

            PeriodType = PeriodTypevar;

            CalculationType = CalculationTypevar;


            DateTo = new DateTime(DateTovar.Year, DateTovar.Month, DateTovar.Day);

            DateFrom = new DateTime(DateFromvar.Year, DateFromvar.Month, DateFromvar.Day);
            AccountId = AccountIdvar;
        }
        private void StopWatch(string msg)
        {
            //global.Stop();
            //long elapsedMs = global.ElapsedMilliseconds / 1000;
            //string text = string.Format("seconds are \t {1} \t Milliseconds are \t {2} \t ::: End OP {0} ::: ", msg, elapsedMs.ToString(), global.ElapsedMilliseconds);
            //WriteWatch(text);

        }
        private void StartWatch(string msg)
        {

            //long elapsedMs = global.ElapsedMilliseconds / 1000;
            //string text = string.Format("seconds are \t {1} \t Milliseconds are \t {2} \t ::: Start OP {0} ::: ", msg, elapsedMs.ToString(), global.ElapsedMilliseconds);
            //WriteWatch(text);
            //global.Start();
        }
        private void WriteWatch(string msg)
        {
            //try
            //{
               
            //    Framework.ApplicationContext.Instance.Logger.Warn(msg);

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception: " + e.Message);
            //}
            //finally
            //{
            //    Console.WriteLine("Executing finally block.");
            //}

        }

        public IList<CampaignCardinalityEstimatorDto> GetCardinalityEsitimator(string Ids, bool isAdGroup = false)
        {
          

            StartWatch("distributed Ids to results objects");

            List<CampaignCardinalityEstimatorDto> results = null;
            if (isAdGroup)
            {
                results = Ids.Split(',').Select(x => new CampaignCardinalityEstimatorDto { AdGroupId = Convert.ToInt32(x) }).Distinct().ToList();
            }
            else
            {
                results = Ids.Split(',').Select(x => new CampaignCardinalityEstimatorDto { CampaignId = Convert.ToInt32(x) }).Distinct().ToList();

            }
            StopWatch("distributed Ids to results objects");

            if (PeriodType == EstimatorCalculationPeriodType.Day)
            {

                return GetCardinalityEsitimatorDaily(Ids);
            }

            if ((DateTo - DateFrom).TotalDays == 0)
            {
                return GetCardinalityEsitimatorDaily(Ids);
            }


            if ((DateFrom.Day == 1 && DateTo.Month == DateFrom.Month && DateTo.Year == DateFrom.Year) && (DateTo.Day == DateTime.DaysInMonth(DateTo.Year, DateTo.Month) || (DateTo.Month == Framework.Utilities.Environment.GetServerTime().Month && DateTo.Day == Framework.Utilities.Environment.GetServerTime().Day)))
            {
                return GetCardinalityEsitimatorOneMonthly(Ids);
            }
            DateEstimatorRange range = new DateEstimatorRange(DateFrom, DateTo);

            var aggImpression = new CardinalityEstimatorAggregator();
            var aggClicksDirectCount = new CardinalityEstimatorAggregator();

           
            StartWatch("GetStringTimeDes");
            var ranges = range.GetStringTimeDes();
            StopWatch("GetStringTimeDes");

            
            //StartWatch("rangesLoop");
            foreach (var Range in ranges)
            {
                
                // StartWatch("getRangeDayHyperLogFunc" + Range.PeriodType.ToString());

                if (!string.IsNullOrEmpty(Range.Ranges))
                {
                    getRangeDayHyperLog(Ids, Range, ref results);
                }


                // StopWatch("getRangeDayHyperLogFunc" + Range.PeriodType.ToString());

                //foreach (CampaignCardinalityEstimatorDto item in results)
                //{
                //    ReleaseMemory(item);

                //}
            }
            //StopWatch("rangesLoop");
            //sw.Close();

            return results;
        }
        private void ReleaseMemory(CampaignCardinalityEstimatorDto item)
        {
            //bitem.clicks_estimator.
            item.impressions_estimator = null;
            item.clicks_estimator = null;
            item = null;
            //    System.GC.Collect();

        }

        async Task<int> MainAsyncPutInCache(List<CampaignCardinalityEstimatorDto> results)
        {
            foreach (CampaignCardinalityEstimatorDto res in results)
            {
                PutInCache(res.impressions_estimator, res.ImpCacheKey, getDatestring(res.Date.ToString()));
                PutInCache(res.clicks_estimator, res.ClickCacheKey, getDatestring(res.Date.ToString()));
                ReleaseMemory(res);
            }
          //  await Task.Delay(10000);
            return 1;
        }
        async Task<int> MainAsyncPutInCache(string cacheKey, string cacheKeyClicks, string date)
        {

            PutInCache(new byte[0], cacheKey, getDatestring(date));
            PutInCache(new byte[0], cacheKeyClicks, getDatestring(date));
            //  ReleaseMemory(res);

           // await Task.Delay(10000);
            return 1;
        }


        public void getRangeDayHyperLog(string ids, DateEstimatorRangeResult dateRange, ref List<CampaignCardinalityEstimatorDto> results)
        {
            var aggImpression = new CardinalityEstimatorAggregator();
            var aggClicksDirectCount = new CardinalityEstimatorAggregator();
            CampaignCardinalityEstimatorDto resultd = new CampaignCardinalityEstimatorDto();
            List<CampaignCardinalityEstimatorDto> resultList = new List<CampaignCardinalityEstimatorDto>();
            List<string> dates = dateRange.Ranges.Split(',').ToList();
            string NotCachedDates = "";
            string NotCachedIds = "";
            // StartWatch("getRangeDayHyperLogCheckCache");
            foreach (string id in ids.Split(','))
            {
                aggImpression = new CardinalityEstimatorAggregator();
                aggClicksDirectCount = new CardinalityEstimatorAggregator();
                bool idCached = false;
                foreach (string date in dates)
                {
                    if (IdPerRange != null)
                    {
                        DateTime corectTime = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);


                        if (!(corectTime.Date >= IdPerRange[Convert.ToInt32(id)].DateFrom.Date && corectTime.Date <= IdPerRange[Convert.ToInt32(id)].DateTo.Date))
                        {
                            continue;
                        }

                    }
                    resultd = new CampaignCardinalityEstimatorDto();
                    StartWatch("GetCache1");
                    string cacheKey = GetCacheKey(Convert.ToInt32(id), date, dateRange.PeriodType);
                    StopWatch("GetCache1");
                    StartWatch("GetCache2");
                    string cacheKeyClicks = GetCacheKeyClicks(Convert.ToInt32(id), date, dateRange.PeriodType);
                    StopWatch("GetCache2");
                    byte[] result = CacheManager.Current[cacheName, cacheStore].Get<byte[]>(cacheKey);
                    byte[] resultClicks = CacheManager.Current[cacheName, cacheStore].Get<byte[]>(cacheKeyClicks);
                    //if (!NotCachedDates.Contains(date))
                    //{
                    //    NotCachedDates += date + ",";
                    //}
                    //idCached = true;

                    if (result == null || resultClicks == null)
                    {
                        if (!NotCachedDates.Contains(date))
                        {
                            NotCachedDates += date + ",";
                        }
                        idCached = true;
                    }
                    else
                    {
                        resultd.clicks_estimator = resultClicks;
                        resultd.impressions_estimator = result;
                        // AggregateFromStream(resultClicks, result, ref aggImpression, ref aggClicksDirectCount);


                        if (CalculationType == EstimatorCalculationType.Campaign)
                        {

                            resultd.CampaignId = Convert.ToInt32(id);
                        }
                        if (CalculationType == EstimatorCalculationType.AdGroup)
                        {
                            resultd.AdGroupId = Convert.ToInt32(id); ;

                        }
                        resultd.Date = Convert.ToInt32(date);
                        resultd.FromCache = true;
                        resultList.Add(resultd);
                    }

                }
                if (idCached)
                {
                    NotCachedIds += id + ",";
                }
            }

            // StopWatch("getRangeDayHyperLogCheckCache");
            //if (CalculationType == EstimatorCalculationType.Campaign)
            //    res.CampaignId = Convert.ToInt32(id);
            //else
            //    res.AdGroupId = Convert.ToInt32(id);
            NotCachedDates = NotCachedDates.TrimEnd(',');
            NotCachedIds = NotCachedIds.TrimEnd(',');
            List<CampaignCardinalityEstimatorDto> resultList2 = new List<CampaignCardinalityEstimatorDto>();
            List<CampaignCardinalityEstimatorDto> PutinCache = new List<CampaignCardinalityEstimatorDto>();
            if (NotCachedDates.Length > 0 && NotCachedDates.Count() > 0 && NotCachedIds.Length > 0 && NotCachedIds.Split(',').Count() > 0)
            {

                string sqlScript = GetSQLScript(NotCachedIds, NotCachedDates, dateRange.PeriodType);
           
                StartWatch("GetResultFunc");
                resultList2 = GetResult<CampaignCardinalityEstimatorDto>(sqlScript);
                StopWatch("GetResultFunc");
                foreach (string ResId in NotCachedIds.Split(','))
                {
                    foreach (string ResDate in NotCachedDates.Split(','))
                    {
                        var temp = resultList2.Where(x => x.Date.ToString() == ResDate);
                        if (CalculationType == EstimatorCalculationType.Campaign)
                        {
                            temp = temp.Where(x => x.CampaignId.ToString() == ResId);
                        }
                        if (CalculationType == EstimatorCalculationType.AdGroup)
                        {
                            temp = temp.Where(x => x.AdGroupId.ToString() == ResId);
                        }

                        if (temp.FirstOrDefault() == null)
                        {
                            string cacheKey = GetCacheKey(Convert.ToInt32(ResId), ResDate, dateRange.PeriodType);
                            string cacheKeyClicks = GetCacheKeyClicks(Convert.ToInt32(ResId), ResDate, dateRange.PeriodType);

                           
                                var t = Task.Run(() => MainAsyncPutInCache(cacheKey, cacheKeyClicks, ResDate));
                                t.ConfigureAwait(false);
                                //  t.Start();
                            
                            //PutInCache(new byte[0], cacheKey, getDatestring(ResDate));
                            //PutInCache(new byte[0], cacheKeyClicks, getDatestring(ResDate));

                        }
                    }
                }

            }
            List<CampaignCardinalityEstimatorDto> resultListfinal = null;
            resultList2.AddRange(resultList);

        
            // StartWatch("idsLoop");
            foreach (string id in ids.Split(','))
            {
                if (CalculationType == EstimatorCalculationType.Campaign)
                {
                    resultListfinal = resultList2.Where(M => M.CampaignId == Convert.ToInt32(id)).ToList();
                }
                else if (CalculationType == EstimatorCalculationType.AdGroup)
                {
                    resultListfinal = resultList2.Where(M => M.AdGroupId == Convert.ToInt32(id)).ToList();
                }
                aggImpression = new CardinalityEstimatorAggregator();
                aggClicksDirectCount = new CardinalityEstimatorAggregator();
                int i = 0;

                foreach (CampaignCardinalityEstimatorDto res in resultListfinal)
                {

                    if (res.impressions_estimator != null && !res.FromCache)
                    {
                       
                        StartWatch("GetCacheKeyFunc" + i.ToString());
                        string cacheKey = GetCacheKey(res.CampaignId, res.Date.ToString(), dateRange.PeriodType);
                        string cacheKeyClicks = GetCacheKeyClicks(res.CampaignId, res.Date.ToString(), dateRange.PeriodType);

                        res.ImpCacheKey = cacheKey;
                        res.ClickCacheKey = cacheKeyClicks;
                        StopWatch("GetCacheKeyFunc" + i.ToString());

                     
                        StartWatch("PutInCacheFunc" + i.ToString());
                        // PutInCache(res.impressions_estimator, cacheKey, getDatestring(res.Date.ToString()));
                        //    PutInCache(res.clicks_estimator, cacheKeyClicks, getDatestring(res.Date.ToString()));



                        AggregateFromStream(res.clicks_estimator, res.impressions_estimator, ref aggImpression, ref aggClicksDirectCount);
                        PutinCache.Add(res);

                        StopWatch("PutInCacheFunc" + i.ToString());
                        continue;

                    }
                    else if (res.FromCache)
                    {
                       
                        StartWatch("AggregateFromStreamFunc" + i.ToString());

                        AggregateFromStream(res.clicks_estimator, res.impressions_estimator, ref aggImpression, ref aggClicksDirectCount);
                        StopWatch("AggregateFromStreamFunc" + i.ToString());

                    }
                    i++;
                   
                    StartWatch("ReleaseMemoryFunc");
                    ReleaseMemory(res);
                    StopWatch("ReleaseMemoryFunc");


                }
             
                StartWatch("AssignFunc");
                if (CalculationType == EstimatorCalculationType.Campaign)
                {
                    results.Where(x => x.CampaignId == Convert.ToInt32(id)).FirstOrDefault().unique_clicks = (long)aggClicksDirectCount.Count();
                    results.Where(x => x.CampaignId == Convert.ToInt32(id)).FirstOrDefault().unique_impressions = (long)aggImpression.Count();
                }
                if (CalculationType == EstimatorCalculationType.AdGroup)
                {
                    results.Where(x => x.AdGroupId == Convert.ToInt32(id)).FirstOrDefault().unique_clicks = (long)aggClicksDirectCount.Count();
                    results.Where(x => x.AdGroupId == Convert.ToInt32(id)).FirstOrDefault().unique_impressions = (long)aggImpression.Count();
                }
                if (PutinCache != null && PutinCache.Count > 0)
                {
                    var t = Task.Run(() => MainAsyncPutInCache(PutinCache));
                    t.ConfigureAwait(false);
                    //  t.Start();
                }
                StopWatch("AssignFunc");

            }
            // StopWatch("idsLoop");



        }

        public void AggregateFromStream(byte[] clicks_estimator, byte[] impressions_estimator, ref CardinalityEstimatorAggregator aggImpression, ref CardinalityEstimatorAggregator aggClicksDirectCount)
        {
            if (impressions_estimator != null && impressions_estimator.Count() > 0)
                aggImpression.AggregateFromStream(new MemoryStream(impressions_estimator));

            if (clicks_estimator != null && clicks_estimator.Count() > 0)
                aggClicksDirectCount.AggregateFromStream(new MemoryStream(clicks_estimator));
        }
        private string GenerateBetweenDates(string dates, string dateField, EstimatorCalculationPeriodType summaryBy)
        {
            IList<int> datesNo = (dates ?? "").Split(',').Select<string, int>(int.Parse).Distinct().ToList();
            datesNo = datesNo.OrderBy(M => M).ToList();
            IDictionary<int, int> dicgouping = new Dictionary<int, int>();
            int groupIndex = 0;
            int diffDay = 0;
            int IncreasedgroupIndex = 0;
            if (summaryBy == EstimatorCalculationPeriodType.Day)
            {
                groupIndex = 1;
                diffDay = 1;
                IncreasedgroupIndex = 1;
            }

            if (summaryBy == EstimatorCalculationPeriodType.Month)
            {
                diffDay = 28;
                groupIndex = 100;
                IncreasedgroupIndex = 100;
            }

            dicgouping.Add(datesNo[0], IncreasedgroupIndex);

            DateTime firstDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime secondDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 1; i < datesNo.Count; i++)

            {

                firstDate = DateTime.ParseExact(datesNo[i - 1].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                secondDate = DateTime.ParseExact(datesNo[i].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                var diff = (secondDate - firstDate).TotalDays;

                if (diff == diffDay)

                {
                    dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                }
                else
                {
                    if (summaryBy == EstimatorCalculationPeriodType.Month)
                    {
                        if (diff >= 28 && diff <= 31)
                        {

                            dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                        }
                        else
                        {
                            IncreasedgroupIndex = IncreasedgroupIndex + groupIndex;
                            dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                        }
                    }
                    else
                    {
                        IncreasedgroupIndex = IncreasedgroupIndex + groupIndex;
                        dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                    }
                }
            }
            var results = dicgouping.GroupBy(n => n.Value).Select(g => g.Count() >= 2 ?
       dateField + " BETWEEN " + g.First().Key + " AND " + g.Last().Key :
       dateField + "=" + g.Select(x => x.Key).First()).ToList();


            return "(" + String.Join(" oR ", results) + ")";
        }
        public bool isCurrentDay(DateTime Date)
        {
            return DateTo == Framework.Utilities.Environment.GetServerDate();
        }

        public bool isInCurrentMonth(DateTime date)
        {
            return (date.Year == Framework.Utilities.Environment.GetServerTime().Year && date.Month == Framework.Utilities.Environment.GetServerTime().Month);
        }

        public void PutInCache(byte[] val, string key, DateTime date)
        {
            if (isInCurrentMonth(date) || isCurrentDay(date))
            {
                CacheManager.Current[cacheName, cacheStore].Put<byte[]>(key, val, time1HoursPeriod);
            }
            CacheManager.Current[cacheName, cacheStore].Put<byte[]>(key, val, time6HoursPeriod);

        }
        public IList<CampaignCardinalityEstimatorDto> GetCardinalityEsitimatorDaily(string Ids)
        {

            StartWatch("GetCardinalityEsitimatorDaily");
            string dateFrom = DateFrom.ToString("yyyyMMdd");
            string dateTo = DateTo.ToString("yyyyMMdd");
            string sqlScript = string.Empty;
            string idsModified = Ids;
            if (CalculationType == EstimatorCalculationType.AdGroup)
            {
                idsModified = idsModified.Replace(",", " OR adgroupid = ");
                idsModified = "adgroupid = " + idsModified;
                sqlScript = GetAdGroupSQLScriptDayFull(idsModified, dateFrom, dateTo);
            }
            else
            {
                idsModified = idsModified.Replace(",", " OR campaignid = ");
                idsModified = "campaignid = " + idsModified;
                sqlScript = GetCampSQLScriptDayFull(idsModified, dateFrom, dateTo);



            }

            StopWatch("GetCardinalityEsitimatorDaily");
            //sw.Close();

            return GetResult<CampaignCardinalityEstimatorDto>(sqlScript);
        }
        public IList<CampaignCardinalityEstimatorDto> GetCardinalityEsitimatorOneMonthly(string Ids)
        {
            StartWatch("GetCardinalityEsitimatorOneMonthly");
            string dateFrom = DateFrom.ToString("yyyyMMdd");
            string dateTo = DateTo.ToString("yyyyMMdd");
            string sqlScript = string.Empty;

            string idsModified = Ids;
            if (CalculationType == EstimatorCalculationType.AdGroup)
            {
                idsModified = idsModified.Replace(",", " OR adgroupid = ");
                idsModified = "adgroupid = " + idsModified;

                sqlScript = GetAdGroupSQLScriptMonthFull(idsModified, dateFrom);
            }
            else
            {
                idsModified = idsModified.Replace(",", " OR campaignid = ");
                idsModified = "campaignid = " + idsModified;
                sqlScript = GetCampSQLScriptMonthFull(idsModified, dateFrom);



            }
            StopWatch("GetCardinalityEsitimatorOneMonthly");
          //  sw.Close();

            return GetResult<CampaignCardinalityEstimatorDto>(sqlScript);
        }
        protected List<T> GetResult<T>(string script, string optoionalDrop = "")
        {
            // Create a PetaPoco database object

            Framework.ApplicationContext.Instance.Logger.Info(script);
            using (var db = new ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP.PetaPoco.Database("ReportingGPDB"))
            {
                //make the timeout 5 minutes
                db.CommandTimeout = 300;
                db.EnableAutoSelect = false;

                //return db.Fetch<T>("call executeScript(@0);", script.Replace("'","''")).ToList();
                // db.BeginTransaction();
                var results = db.Fetch<T>(script).ToList();

                // db.CompleteTransaction();
                if (!string.IsNullOrEmpty(optoionalDrop))
                    db.Execute(optoionalDrop);
                return results;
            }
            //return db.Fetch<T>(script,0).ToList();
            //return new List<T>();
        }
        public string GetCampSQLScriptMonthFull(string campIds, string dayFrom)
        {

            return string.Format(CampCountSQL_m, campIds, dayFrom,AccountId);

        }
        public string GetAdGroupSQLScriptMonthFull(string AdGroupIds, string dayFrom)
        {

            return string.Format(AdGoupCountSQL_m, AdGroupIds, dayFrom,AccountId);

        }
        public string GetCampSQLScriptDayFull(string campIds, string dayFrom, string dayTo)
        {

            return string.Format(CampCountSQL_d, campIds, dayFrom, dayTo,AccountId);

        }
        public string GetAdGroupSQLScriptDayFull(string AdGroupIds, string dayFrom, string dayTo)
        {

            return string.Format(AdGoupCountSQL_d, AdGroupIds, dayFrom, dayTo,AccountId);

        }

        public string GetCampSQLScriptDay(string campId, string day)
        {

            return string.Format(CampEsimatorSQL_d, campId, GenerateBetweenDates(day, "dateid", EstimatorCalculationPeriodType.Day),AccountId);

        }
        public string GetAdGroupSQLScriptDay(string AdGroupId, string day)
        {

            return string.Format(AdGoupEsimatorSQL_d, AdGroupId, GenerateBetweenDates(day, "dateid", EstimatorCalculationPeriodType.Day),AccountId);

        }

        public string GetCampSQLScriptMonth(string campId, string day)
        {

            return string.Format(CampEsimatorSQL_m, campId, GenerateBetweenDates(day, "monthid", EstimatorCalculationPeriodType.Month),AccountId);

        }
        public string GetAdGroupSQLScriptMonth(string AdGroupId, string day)
        {

            return string.Format(AdGoupEsimatorSQL_m, AdGroupId, GenerateBetweenDates(day, "monthid", EstimatorCalculationPeriodType.Month), AccountId);

        }
        public string GetCampHyperLogCachKeyClick(int campId, string day)
        {

            return string.Format(CampCacheHYPLClick, campId, day);

        }
        public string GetAdGroupHyperLogCachKeyClick(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheHYPLClick, AdGroupId, day);

        }


        public string GetCampMHyperLogCachKeyClick(int campId, string day)
        {

            return string.Format(CampCacheHYPL_mClick, campId, day);

        }
        public string GetAdGroupMHyperLogCachKeyClick(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheHYPL_mClick, AdGroupId, day);

        }


        public string GetCampHyperLogCachKey(int campId, string day)
        {

            return string.Format(CampCacheHYPL, campId, day);

        }
        public string GetAdGroupHyperLogCachKey(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheHYPL, AdGroupId, day);

        }
        public string GetCampCountCachKey(int campId, string day)
        {

            return string.Format(CampCacheCount, campId, day);

        }
        public string GetAdGroupCountCachKey(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheCount, AdGroupId, day);

        }

        public string GetCacheKeyClicks(int id, string day, EstimatorCalculationPeriodType PeriodType)
        {
            bool isAdGroup = false;

            if (CalculationType == EstimatorCalculationType.AdGroup)
            {
                isAdGroup = true;
            }

            if (PeriodType == EstimatorCalculationPeriodType.Day)
            {
                if (isAdGroup)
                {
                    return GetAdGroupHyperLogCachKeyClick(id, day);
                }
                return GetCampHyperLogCachKeyClick(id, day);
            }
            if (PeriodType == EstimatorCalculationPeriodType.Month)
            {
                if (isAdGroup)
                {
                    return GetAdGroupMHyperLogCachKeyClick(id, day);
                }

                return GetCampMHyperLogCachKeyClick(id, day);
            }
            return string.Empty;
        }
        public string GetCacheKey(int id, string day, EstimatorCalculationPeriodType PeriodType)
        {
            bool isAdGroup = false;

            if (CalculationType == EstimatorCalculationType.AdGroup)
            {
                isAdGroup = true;
            }

            if (PeriodType == EstimatorCalculationPeriodType.Day)
            {
                if (isAdGroup)
                {
                    return GetAdGroupHyperLogCachKey(id, day);
                }
                return GetCampHyperLogCachKey(id, day);
            }
            if (PeriodType == EstimatorCalculationPeriodType.Month)
            {
                if (isAdGroup)
                {
                    return GetAdGroupMHyperLogCachKey(id, day);
                }

                return GetCampMHyperLogCachKey(id, day);
            }
            return string.Empty;
        }

        public string GetSQLScript(string id, string day, EstimatorCalculationPeriodType PeriodType)
        {
            bool isAdGroup = false;
            string idsModified = id;
            if (CalculationType == EstimatorCalculationType.AdGroup)
            {
                isAdGroup = true;
                idsModified = idsModified.Replace(",", " OR adgroupid = ");
                idsModified = " adgroupid = " + idsModified;
            }
            else
            {
                idsModified = idsModified.Replace(",", " OR campaignid = ");
                idsModified = " campaignid = " + idsModified;


            }
            if (PeriodType == EstimatorCalculationPeriodType.Day)
            {
                if (isAdGroup)
                {
                    return GetAdGroupSQLScriptDay(idsModified, day);
                }
                return GetCampSQLScriptDay(idsModified, day);
            }
            if (PeriodType == EstimatorCalculationPeriodType.Month)
            {
                if (isAdGroup)
                {
                    return GetAdGroupSQLScriptMonth(idsModified, day);
                }

                return GetCampSQLScriptMonth(idsModified, day);
            }
            return string.Empty;
        }
        public DateTime getDatestring(string date)
        {
            DateTime theTime = DateTime.ParseExact(date,
                                    "yyyyMMdd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);

            return theTime;

        }

        public string GetCampMHyperLogCachKey(int campId, string day)
        {

            return string.Format(CampCacheHYPL_m, campId, day);

        }
        public string GetAdGroupMHyperLogCachKey(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheHYPL_m, AdGroupId, day);

        }
        public string GetCampMCountCachKey(int campId, string day)
        {

            return string.Format(CampCacheCount_m, campId, day);

        }
        public string GetAdGroupMCountCachKey(int AdGroupId, string day)
        {

            return string.Format(AdGroupCacheCount_m, AdGroupId, day);

        }

    }


    public class DateEstimatorRange
    {
        public DateEstimatorRange()
        {

        }
        public DateEstimatorRange(DateTime DateFrom, DateTime DateTo)
        {
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;

        }
        public DateTime DateFrom, DateTo;
        public EstimatorCalculationPeriodType type;

        public List<DateEstimatorRange> GetTimeDes()
        {
            List<DateEstimatorRange> results = new List<DateEstimatorRange>();
            DateTime tfrom = DateFrom;

            while (tfrom <= DateTo)
            {
                if (tfrom == new DateTime(tfrom.Year, tfrom.Month, 1))
                {
                    DateTime monthEnd = new DateTime(tfrom.Year, tfrom.Month, DateTime.DaysInMonth(tfrom.Year, tfrom.Month));
                    if (DateTo > monthEnd)
                    {
                        results.Add(new DateEstimatorRange { type = EstimatorCalculationPeriodType.Month, DateFrom = tfrom, DateTo = monthEnd });
                        tfrom = monthEnd.AddDays(1);
                    }
                    else if (DateTo < monthEnd)
                    {

                        if ((tfrom.Day == 1 && DateTo.Month == tfrom.Month && DateTo.Year == tfrom.Year) && ((DateTo.Month == Framework.Utilities.Environment.GetServerTime().Month && DateTo.Day == Framework.Utilities.Environment.GetServerTime().Day)))
                        {

                            results.Add(new DateEstimatorRange { type = EstimatorCalculationPeriodType.Month, DateFrom = tfrom, DateTo = monthEnd });
                            tfrom = monthEnd.AddDays(1);

                        }
                        else
                        {
                            while (tfrom <= DateTo)
                            {
                                results.Add(new DateEstimatorRange { type = EstimatorCalculationPeriodType.Day, DateFrom = tfrom, DateTo = tfrom });
                                tfrom = tfrom.AddDays(1);
                            }
                        }


                    }
                    else
                    {
                        results.Add(new DateEstimatorRange { type = EstimatorCalculationPeriodType.Month, DateFrom = tfrom, DateTo = DateTo });
                        tfrom = DateTo.AddDays(1);
                    }
                }
                else
                {
                    DateTime TempFrom = tfrom;
                    while (tfrom.Day <= DateTime.DaysInMonth(tfrom.Year, tfrom.Month) && TempFrom.Month == tfrom.Month && tfrom <= DateTo)
                    {
                        results.Add(new DateEstimatorRange { type = EstimatorCalculationPeriodType.Day, DateFrom = tfrom, DateTo = tfrom });

                        tfrom = tfrom.AddDays(1);

                    }
                }

            }

            return results;
        }

        public IList<DateEstimatorRangeResult> GetStringTimeDes()
        {
            IList<DateEstimatorRangeResult> result = new List<DateEstimatorRangeResult>();
            string DayString = "";
            string MonthString = "";

            DateTime tfrom = DateFrom;
            while (tfrom <= DateTo)
            {
                if (tfrom == new DateTime(tfrom.Year, tfrom.Month, 1))
                {
                    DateTime monthEnd = new DateTime(tfrom.Year, tfrom.Month, DateTime.DaysInMonth(tfrom.Year, tfrom.Month));
                    if (DateTo > monthEnd)
                    {
                        MonthString += tfrom.ToString("yyyyMMdd") + ",";
                        tfrom = monthEnd.AddDays(1);
                    }
                    else if (DateTo < monthEnd)
                    {

                        if ((tfrom.Day == 1 && DateTo.Month == tfrom.Month && DateTo.Year == tfrom.Year) && ((DateTo.Month == Framework.Utilities.Environment.GetServerTime().Month && DateTo.Day == Framework.Utilities.Environment.GetServerTime().Day)))
                        {

                            MonthString += tfrom.ToString("yyyyMMdd") + ",";
                            tfrom = monthEnd.AddDays(1);

                        }
                        else
                        {
                            while (tfrom <= DateTo)
                            {
                                DayString += tfrom.ToString("yyyyMMdd") + ",";
                                tfrom = tfrom.AddDays(1);
                            }
                        }


                    }
                    else
                    {
                        MonthString += tfrom.ToString("yyyyMMdd") + ",";
                        tfrom = DateTo.AddDays(1);
                    }
                }
                else
                {
                    DateTime TempFrom = tfrom;
                    while (tfrom.Day <= DateTime.DaysInMonth(tfrom.Year, tfrom.Month) && TempFrom.Month == tfrom.Month && tfrom <= DateTo)
                    {
                        DayString += tfrom.ToString("yyyyMMdd") + ",";

                        tfrom = tfrom.AddDays(1);

                    }
                }

            }

            DayString = DayString.TrimEnd(',');
            MonthString = MonthString.TrimEnd(',');


            result.Add(new DateEstimatorRangeResult
            {
                Ranges = DayString,
                PeriodType = EstimatorCalculationPeriodType.Day,
            });
            result.Add(new DateEstimatorRangeResult
            {
                Ranges = MonthString,
                PeriodType = EstimatorCalculationPeriodType.Month,
            });

            return result;
        }
    }

    public class DateEstimatorRangeResult
    {
        public string Ranges { get; set; }

        public EstimatorCalculationPeriodType PeriodType { get; set; }

    }
}
