using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
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

namespace ArabyAds.AdFalcon.Persistence.ReportsGP.MinHashEstimator
{
    public class CounterCode
    {
         public const string CountryCode = "country";
         public const string OperatorCode = "operator";
         public const string PlatformCode = "platform";
         public const string AdFormatCode = "adtypegroup";
         public const string GenderTypeCode = "gender";
         public const string AgeGroupCode = "agegroup";
         public const string LanguageCode = "language";
         public const string DeviceTypeCode = "devicetype";
         public const string AdSizeCode = "creativeunits";
         public const string AppSiteCode = "appsite";
        public const string SubAppSiteCode = "subappsite";
        public const string environmenttypeCode = "environmenttype";
        public const string providerSegmentsCode = "provider-segments";


    }

    public class MinHashEstimatorProcessing
    {
        public const string fact_publisherCounter = "select * from fact_publisher_counters where dateid={0} {1} {2} ";

        public const string CacheMINEstimator_m = "MIN_Hash_{0}_{1}_{2}_m";
        public const string CacheMINEstimator_r = "MIN_Hash_{0}_{1}_{2}_r";
        private TimeSpan time6HoursPeriod = new TimeSpan(6, 0, 0);
        private TimeSpan time1HoursPeriod = new TimeSpan(1, 0, 0);
      
        public int weekno;
    
    
  
        private string cacheStore = CacheStores.Redis;
        private string cacheName = string.Empty;
        private TrafficPlannerCriteriaDto Criteria;
        private CombinedEstimator combinedEstimator;
        private string CounterCodeStr = string.Empty;

        public long GetUniqueCountForByCounterId(int CounterId)
        {
            //if (CounterId == 0)
            //    return 0;
            Byte[] estimator=null;
            long requests = 0;
            if (!CheckIfExistInCache(GetMinHashCache(CounterCodeStr, CounterId)))
            {

                var result = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCodeStr, CounterId)).ToList();
                if (result!=null && result.Count>0)
                {
                    estimator = result[0].estimator;
                    PutInCache(estimator, GetMinHashCache(CounterCodeStr, CounterId));
                    requests = result[0].unique_requests;
                    PutInCache(result[0].unique_requests, GetMinHashRequestsCache(CounterCodeStr, CounterId));
                }
            }
            else
            {
                estimator = GetFromCache(GetMinHashCache(CounterCodeStr, CounterId));
                requests = GetFromCacheCount(GetMinHashRequestsCache(CounterCodeStr, CounterId));
            }
            if (estimator != null)
            {
                var CardinalityMinHashCombined = new CardinalityMinHashCombined();
                CardinalityMinHashCombined.Deserialize(new MemoryStream(estimator));
                return combinedEstimator.GetTotalCombined(CardinalityMinHashCombined.hll, CardinalityMinHashCombined.minHashSet, requests);
            }
            return 0;
        }
        public void BuildTrafficPlannerCriteriaCahing()
        {
            IList<MinHashEstimatorDto> CachedResult = new List<MinHashEstimatorDto>();
            IList<MinHashEstimatorDto> TotalResult = new List<MinHashEstimatorDto>();
            string DrillDownDataFilter = string.Empty;
            //if (TrafficPlannerCrt.Continents != null && TrafficPlannerCrt.Continents.Length > 0)
            //{
            //    DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} EXISTS (Select loc.id  from dim_locations loc  where parentid IN (0)  and loc.id= countryid )", string.Join(",", TrafficPlannerCrt.Continents), countAnd > 0 ? " And " : string.Empty);
            //    countAnd++;
            //}
            if (Criteria.Countries != null && Criteria.Countries.Length > 0)
            {
                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.Countries)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.CountryCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator= GetFromCache(GetMinHashCache(CounterCode.CountryCode, c)),CacheKey= GetMinHashCache(CounterCode.CountryCode, c) });
                            
                            
                            }
                }
                if (queryResult.Count > 0)
                { var countriesResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.CountryCode, queryResult.ToArray()));

                    foreach (var item in countriesResult)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.CountryCode, item.Id);
                        TotalResult.Add(item);
                    }
                }
            }

            if (Criteria.Segments != null && Criteria.Segments.Length > 0)
            {
                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.Segments)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.providerSegmentsCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.providerSegmentsCode, c)), CacheKey = GetMinHashCache(CounterCode.providerSegmentsCode, c) });


                    }
                }
                if (queryResult.Count > 0)
                {
                    var segResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.providerSegmentsCode, queryResult.ToArray()));

                    foreach (var item in segResult)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.providerSegmentsCode, item.Id);
                        TotalResult.Add(item);
                    }
                }
            }


            if (Criteria.Operators != null && Criteria.Operators.Length > 0)
            {

                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.Operators)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.OperatorCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.OperatorCode, c)), CacheKey = GetMinHashCache(CounterCode.OperatorCode, c) });


                    }
                }

                if (queryResult.Count > 0)
                {
                    var operatorCodeResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.OperatorCode, queryResult.ToArray()));
                    foreach (var item in operatorCodeResult)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.OperatorCode, item.Id);
                        TotalResult.Add(item);
                    }
                }
            }

            if (Criteria.Platforms != null && Criteria.Platforms.Length > 0)
            {

                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.Platforms)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.PlatformCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.PlatformCode, c)), CacheKey = GetMinHashCache(CounterCode.PlatformCode, c) });


                    }
                }

               
                if (queryResult.Count > 0)
                {
                    var PlatformCodeResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.PlatformCode, queryResult.ToArray()));
                    foreach (var item in PlatformCodeResult)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.PlatformCode, item.Id);
                        TotalResult.Add(item);
                    }
                }

            }
            if (Criteria.AdFormats != null && Criteria.AdFormats.Length > 0)
            {
                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.AdFormats)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.AdFormatCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.AdFormatCode, c)), CacheKey = GetMinHashCache(CounterCode.AdFormatCode, c) });


                    }
                }

                
                if (queryResult.Count > 0)
                {
                    var AdFormatsResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.AdFormatCode, queryResult.ToArray()));
                    foreach (var item in AdFormatsResult)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.AdFormatCode, item.Id);
                        TotalResult.Add(item);
                    }
                }
            }
            if (Criteria.GenderType > 0)
            {
                if (!CheckIfExistInCache(GetMinHashCache(CounterCode.GenderTypeCode, Criteria.GenderType)))
                {
                    var GendersResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.GenderTypeCode, Criteria.GenderType));
                    if (GendersResult != null && GendersResult.Count > 0)
                    {
                        GendersResult[0].CacheKey = GetMinHashCache(CounterCode.GenderTypeCode, Criteria.GenderType);
                        TotalResult.Union(GendersResult);
                    }
                }
                else
                {

                    CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.GenderTypeCode, Criteria.GenderType)), CacheKey = GetMinHashCache(CounterCode.GenderTypeCode, Criteria.GenderType) });


                }
              
            }
            if (Criteria.EnvironmentType > 0)
            {
                if (!CheckIfExistInCache(GetMinHashCache(CounterCode.environmenttypeCode, Criteria.EnvironmentType)))
                {
                    var GendersResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.environmenttypeCode, Criteria.EnvironmentType));
                    if (GendersResult != null && GendersResult.Count > 0)
                    {
                        GendersResult[0].CacheKey = GetMinHashCache(CounterCode.environmenttypeCode, Criteria.EnvironmentType);
                        TotalResult.Union(GendersResult);
                    }

                }
                else
                {

                    CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.environmenttypeCode, Criteria.EnvironmentType)), CacheKey = GetMinHashCache(CounterCode.environmenttypeCode, Criteria.EnvironmentType) });


                }

            }
            if (Criteria.AgeGroup > 0)
            {
                if (!CheckIfExistInCache(GetMinHashCache(CounterCode.AgeGroupCode, Criteria.AgeGroup)))
                {
                    var GendersResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.AgeGroupCode, Criteria.AgeGroup));
                    if (GendersResult != null && GendersResult.Count > 0)
                    {
                        GendersResult[0].CacheKey = GetMinHashCache(CounterCode.AgeGroupCode, Criteria.AgeGroup);
                        TotalResult.Union(GendersResult);
                    }

                }
                else
                {

                    CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.AgeGroupCode, Criteria.AgeGroup)), CacheKey = GetMinHashCache(CounterCode.AgeGroupCode, Criteria.AgeGroup) });


                }

            }

            if (Criteria.languages != null && Criteria.languages.Length > 0)
            {
                


                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.languages)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.LanguageCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.LanguageCode, c)), CacheKey = GetMinHashCache(CounterCode.LanguageCode, c) });


                    }
                }


                if (queryResult.Count > 0)
                {
                    var Results = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.LanguageCode, queryResult.ToArray()));
                    foreach (var item in Results)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.LanguageCode, item.Id);
                        TotalResult.Add(item);
                    }
                }

            }

            if (Criteria.DeviceTypeId > 0)
            {
                if (!CheckIfExistInCache(GetMinHashCache(CounterCode.DeviceTypeCode, Criteria.DeviceTypeId)))
                {
                    var GendersResult = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.DeviceTypeCode, Criteria.DeviceTypeId));
                    if (GendersResult != null && GendersResult.Count > 0)
                    {

                        GendersResult[0].CacheKey = GetMinHashCache(CounterCode.DeviceTypeCode, Criteria.DeviceTypeId);
                        TotalResult.Union(GendersResult);
                    }

                }
                else
                {

                    CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.DeviceTypeCode, Criteria.DeviceTypeId)), CacheKey = GetMinHashCache(CounterCode.DeviceTypeCode, Criteria.DeviceTypeId) });


                }
            }

            if (Criteria.AdSizes != null && Criteria.AdSizes.Length > 0)
            {
           

                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.AdSizes)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.AdSizeCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.AdSizeCode, c)), CacheKey = GetMinHashCache(CounterCode.AdSizeCode, c) });


                    }
                }


                if (queryResult.Count > 0)
                {
                    var Results = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.AdSizeCode, queryResult.ToArray()));
                    foreach (var item in Results)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.AdSizeCode, item.Id);
                        TotalResult.Add(item);
                    }
                }

            }
            if (Criteria.AppSites != null && Criteria.AppSites.Length > 0)
            {


                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.AppSites)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.AppSiteCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.AppSiteCode, c)), CacheKey = GetMinHashCache(CounterCode.AppSiteCode, c) });


                    }
                }


                if (queryResult.Count > 0)
                {
                    var Results = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.AppSiteCode, queryResult.ToArray()));
                    foreach (var item in Results)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.AppSiteCode, item.Id);
                        TotalResult.Add(item);
                    }
                }

            }


            if (Criteria.SubAppSites != null && Criteria.SubAppSites.Length > 0)
            {


                IList<int> queryResult = new List<int>();
                foreach (var c in Criteria.SubAppSites)
                {

                    if (!CheckIfExistInCache(GetMinHashCache(CounterCode.SubAppSiteCode, c)))
                        queryResult.Add(c);
                    else
                    {

                        CachedResult.Add(new MinHashEstimatorDto { estimator = GetFromCache(GetMinHashCache(CounterCode.SubAppSiteCode, c)), CacheKey = GetMinHashCache(CounterCode.SubAppSiteCode, c) });


                    }
                }


                if (queryResult.Count > 0)
                {
                    var Results = GetResult<MinHashEstimatorDto>(GetMinHashSQL(CounterCode.SubAppSiteCode, queryResult.ToArray()));
                    foreach (var item in Results)
                    {
                        item.CacheKey = GetMinHashCache(CounterCode.SubAppSiteCode, item.Id);
                        TotalResult.Add(item);
                    }
                }

            }
            combinedEstimator = new CombinedEstimator();
           var  CardinalityMinHashCombined = new CardinalityMinHashCombined();
            foreach (var item in TotalResult)
            {
                CardinalityMinHashCombined.Deserialize(new MemoryStream(item.estimator));

                combinedEstimator.AddToCardinlity(CardinalityMinHashCombined.hll);
                combinedEstimator.AddToMinHash(CardinalityMinHashCombined.minHashSet);


            }
            foreach (var item in CachedResult)
            {
                CardinalityMinHashCombined.Deserialize(new MemoryStream(item.estimator));

                combinedEstimator.AddToCardinlity(CardinalityMinHashCombined.hll);
                combinedEstimator.AddToMinHash(CardinalityMinHashCombined.minHashSet);


            }
            TotalResult.Union(CachedResult);
            var t = Task.Run(() => MainAsyncPutInCache(TotalResult.ToList()));
            t.ConfigureAwait(false);

        }
        async Task<int> MainAsyncPutInCache(List<MinHashEstimatorDto> results)
        {
            foreach (MinHashEstimatorDto res in results)
            {
                PutInCache(res.estimator, res.CacheKey);
               
                ReleaseMemory(res);
            }
            //  await Task.Delay(10000);
            return 1;
        }
        async Task<int> MainAsyncPutInCache(string cacheKey, string cacheKeyClicks, string date)
        {

            //PutInCache(new byte[0], cacheKey, getDatestring(date));
           
            //  ReleaseMemory(res);

            // await Task.Delay(10000);
            return 1;
        }

        private void ReleaseMemory(MinHashEstimatorDto item)
        {
            //bitem.clicks_estimator.
            item.estimator = null;
       
            item = null;
            //    System.GC.Collect();

        }
        public void PutInCache(byte[] val, string key)
        {
          
            CacheManager.Current[cacheName, cacheStore].Put<byte[]>(key, val, time6HoursPeriod);

        }
        public void PutInCache(long val, string key)
        {

            CacheManager.Current[cacheName, cacheStore].Put<long>(key, val, time6HoursPeriod);

        }
        public MinHashEstimatorProcessing( string CounterCode, TrafficPlannerCriteriaDto CR)
        {

            weekno = CR.Weekid;
            this.Criteria = CR;
            this.CounterCodeStr = CounterCode;




        }
        public string GetMinHashCache(string counterCode, int CounterId)
        {

            return string.Format(CacheMINEstimator_m, weekno, counterCode, CounterId);

        }
        public string GetMinHashRequestsCache(string counterCode, int CounterId)
        {

            return string.Format(CacheMINEstimator_r ,weekno, counterCode, CounterId);

        }

        public string GetMinHashSQL(string counterCode, int CounterId)
        {

            return string.Format(fact_publisherCounter, weekno, " And counter_code='"+ counterCode+"'", " And counter_value_id=" +  CounterId);

        }

        public string GetMinHashSQL(string counterCode, int[] CounterId)
        {

            return string.Format(fact_publisherCounter, weekno, " And counter_code='" + counterCode + "'", " And counter_value_id IN (" + string.Join(",", CounterId) +")");

        }

        public bool CheckIfExistInCache(string cacheKey)
        {

            byte[] result = CacheManager.Current[cacheName, cacheStore].Get<byte[]>(cacheKey);
           return  result != null;
                
        }
        public long GetFromCacheCount(string cacheKey)
        {

            long result = CacheManager.Current[cacheName, cacheStore].Get<long>(cacheKey);
            return result;

        }
        public byte[] GetFromCache(string cacheKey)
        {

            byte[] result = CacheManager.Current[cacheName, cacheStore].Get<byte[]>(cacheKey);
            return result ;

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

              
                var results = db.Fetch<T>(script).ToList();

               
                if (!string.IsNullOrEmpty(optoionalDrop))
                    db.Execute(optoionalDrop);
                return results;
            }
          
        }
    }
}
