using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using NHibernate;
using System.Collections;
using System.Text.RegularExpressions;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{

    public class KPIConfigRepository : RepositoryBase<KPIConfig, int>, IKPIConfigRepository
    {
        public KPIConfigRepository(RepositoryImplBase<KPIConfig, int> repository)
          : base(repository)
        { }
        private readonly static string METRIC_ALIAS_PLACEHOLDER = " @@METRIC_ALIAS@@ ", 
                                       METRICS_DB_FIELDS_PLACEHOLDER = " @@METRICS_DB_FIELDS@@ ",
                                       METRICS_DB_TABLE_PLACEHOLDER = " @@METRICS_DB_TABLE@@ ",
                                       METRICS_EXTRA_FILTERS_PLACEHOLDER = " @@METRICS_EXTRA_FILTERS@@ ";
        private readonly static string METRIC_GROWTH_ALIAS = $" GR_OverTime_{METRIC_ALIAS_PLACEHOLDER} ";
        private readonly static string INPUT_VIEW_ALIAS = "input", PRESENT_VIEW_ALIAS = "present", PAST_VIEW_ALIAS = "past", TOTALMETRIC_VIEW_ALIAS="totalMetric";
        private readonly static string METRIC_COLUMNS_TEMPLATE = @$" {TOTALMETRIC_VIEW_ALIAS}.{METRIC_ALIAS_PLACEHOLDER},  common_schema.gr_overtime({PAST_VIEW_ALIAS}.{METRIC_ALIAS_PLACEHOLDER}, {PRESENT_VIEW_ALIAS}.{METRIC_ALIAS_PLACEHOLDER}, {INPUT_VIEW_ALIAS}.duration)  {METRIC_GROWTH_ALIAS} ";
        private readonly static string INPUT_VIEW_QUERY = @$" (SELECT MIN(Day) min_day, MAX(Day) max_day, COUNT(DISTINCT (Day)) duration, {METRICS_DB_FIELDS_PLACEHOLDER}  
                                                             FROM {METRICS_DB_TABLE_PLACEHOLDER}
                                                             WHERE Day BETWEEN :past_day AND :present_day and Day < :today {METRICS_EXTRA_FILTERS_PLACEHOLDER}) {INPUT_VIEW_ALIAS} ";
        private readonly static string PRESENT_VIEW_QUERY = @$" (SELECT Day, {METRICS_DB_FIELDS_PLACEHOLDER}
	                                                         FROM {METRICS_DB_TABLE_PLACEHOLDER} 
	                                                         WHERE Day BETWEEN :past_day AND :present_day {METRICS_EXTRA_FILTERS_PLACEHOLDER} GROUP BY Day) 
                                                             {PRESENT_VIEW_ALIAS} ON ({PRESENT_VIEW_ALIAS}.Day = {INPUT_VIEW_ALIAS}.max_day ) ";

        private readonly static string PAST_VIEW_QUERY = @$" (SELECT Day, {METRICS_DB_FIELDS_PLACEHOLDER}
	                                                         FROM {METRICS_DB_TABLE_PLACEHOLDER} 
	                                                         WHERE Day BETWEEN :past_day AND :present_day {METRICS_EXTRA_FILTERS_PLACEHOLDER} GROUP BY Day) 
                                                             {PAST_VIEW_ALIAS} ON ({PAST_VIEW_ALIAS}.Day = {INPUT_VIEW_ALIAS}.min_day ) ";





        private readonly static string TOTALMETRIC_VIEW_QUERY = @$" ,(SELECT  {METRICS_DB_FIELDS_PLACEHOLDER}
	                                                         FROM {METRICS_DB_TABLE_PLACEHOLDER} 
	                                                         WHERE Day BETWEEN :past_day AND :present_day {METRICS_EXTRA_FILTERS_PLACEHOLDER} ) 
                                                             {TOTALMETRIC_VIEW_ALIAS} ";

        private readonly static string PAST_DAY_PARAM = "past_day", PRESENT_DAY_PARAM = "present_day", TODAY_PARAM = "today", METRIC_ALIAS_PREFIX = "Metric_";


        //private string GetDBFieldSummation(string dbField) 
        //{
        //    if (dbField.Contains('/'))
        //    {
        //        Regex.Matches(

            
        //    }
        //    return dbField;
        //}
        private string BuildQuery(string tableName, IList<string> dataBaseFields) {

            StringBuilder query = new StringBuilder("SELECT");

            StringBuilder metricsDBFieldsSelectList = new StringBuilder($" {dataBaseFields[0]} {METRIC_ALIAS_PREFIX}0");
            query.Append(METRIC_COLUMNS_TEMPLATE.Replace(METRIC_ALIAS_PLACEHOLDER, METRIC_ALIAS_PREFIX + "0"));
            for (int i=1;i< dataBaseFields.Count; i++)
            {
                query.Append(',');
                query.Append(METRIC_COLUMNS_TEMPLATE.Replace(METRIC_ALIAS_PLACEHOLDER, METRIC_ALIAS_PREFIX + i));
                query.Append(' ');
                metricsDBFieldsSelectList.Append(',');
                metricsDBFieldsSelectList.Append($" {dataBaseFields[i]} {METRIC_ALIAS_PREFIX}{i} ");
            }
            query.Append("FROM ");
            query.Append(INPUT_VIEW_QUERY);
            query.Append(" INNER JOIN ");
            query.Append(PRESENT_VIEW_QUERY);
            query.Append(" INNER JOIN ");
            query.Append(PAST_VIEW_QUERY);
            query.Append(TOTALMETRIC_VIEW_QUERY);

            query = query.Replace(METRICS_DB_TABLE_PLACEHOLDER, tableName);
            query = query.Replace(METRICS_DB_FIELDS_PLACEHOLDER, metricsDBFieldsSelectList.ToString());

            return " SET SQL_BIG_SELECTS=1 ; " + query.ToString();
        }
        public IDictionary<string,(decimal?, double?)> GetKPIsForCampaigns(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            var today = int.Parse (DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("campaignsperformance", dataBaseFields);
            if (ids?.Length > 0) 
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and CampaignId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();
            // Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}", "Event Broker", args.EventName, args.InstanceId);

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }


        public IDictionary<string, (decimal?, double?)> GetKPIsForAdGroups(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("adgroupsperformance", dataBaseFields);
            if (ids?.Length > 0)
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and AdGroupId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();
            // Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}", "Event Broker", args.EventName, args.InstanceId);

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }
        public IDictionary<string, (decimal?, double?)> GetKPIsForAds(IList<string> dataBaseFields, DateTime from, DateTime to,  int? accountId,params int[] ids)
        {
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("adsperformance", dataBaseFields);
            if (ids?.Length > 0)
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and AdId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();
            // Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}", "Event Broker", args.EventName, args.InstanceId);

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }


        public IDictionary<string, (decimal?, double?)> GetKPIsForAdvertisers(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("advertisersperformance", dataBaseFields);
            if (accountId.HasValue)
            {
                

                if (ids?.Length > 0)
                {
                   // sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and AdvertiserAssociationId in (:ids)");
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM advertiser_account
                WHERE advertiser_account.accountid = :accountid and advertiser_account.id=advertisersperformance.AdvertiserAssociationId  )  and  AdvertiserAssociationId in (:ids)");


                }
                else
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM advertiser_account
                WHERE advertiser_account.accountid = :accountid and advertiser_account.id=advertisersperformance.AdvertiserAssociationId) ");

                }
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (accountId.HasValue)
            {
                query.SetInt32("accountid", accountId.Value);
            }

            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }
           int fromInt= int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
              
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();
           // Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}", "Event Broker", args.EventName, args.InstanceId);

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }

        private IDictionary<string, (decimal?, double?)> GetDBFieldToValuesMAp(IList dbResults, IList<string> dataBaseFields,int fromInt = 0, int toInt = 0)
        {
            var dic = new Dictionary<string, (decimal?, double?)>();
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            if (dbResults.Count == 0) {

                for (int i = 0; i < dataBaseFields.Count; i++)
                {
                   // var metricAlias = METRIC_ALIAS_PREFIX + i;
                   // var metricGrowthAlias = METRIC_GROWTH_ALIAS.Replace(METRIC_ALIAS_PLACEHOLDER, metricAlias);
                    dic.Add(dataBaseFields[i],( null, null));
                }
                return dic;
            };
            var resultsHash = dbResults[0] as Hashtable;
            for (int i = 0; i < dataBaseFields.Count; i++)
            {
                var metricAlias = METRIC_ALIAS_PREFIX + i;
                var metricGrowthAlias = METRIC_GROWTH_ALIAS.Replace(METRIC_ALIAS_PLACEHOLDER, metricAlias);
                if (!string.IsNullOrEmpty(metricGrowthAlias))
                    metricGrowthAlias = metricGrowthAlias.Trim();
                if (!((toInt - fromInt) <= 0))
                {
                    //today = 0;

                    dic.Add(dataBaseFields[i], ((decimal?)resultsHash[metricAlias], (double?)resultsHash[metricGrowthAlias]));
                }
                else if (toInt == today)
                {
                    dic.Add(dataBaseFields[i], ((decimal?)resultsHash[metricAlias], null));


                }
                else
                {
                    dic.Add(dataBaseFields[i], ((decimal?)resultsHash[metricAlias], (double?)resultsHash[metricGrowthAlias]));

                }
            }
            return dic;
        }

        public IDictionary<string, (decimal?, double?)> GetKPIsForDeals(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("dealsperformance", dataBaseFields);


            if (accountId.HasValue)
            {


                if (ids?.Length > 0)
                {
                   
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM buyer_deals
                WHERE buyer_deals.AccountId = :accountid and buyer_deals.id=dealsperformance.DealId  )  and  DealId in (:ids)");


                }
                else
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM buyer_deals
                WHERE buyer_deals.AccountId = :accountid and buyer_deals.Id=dealsperformance.DealId) ");

                }
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }
            /*if (ids?.Length > 0)
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and DealId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }*/
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }

            if (accountId.HasValue)
            {
                query.SetInt32("accountid", accountId.Value);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }

        public IDictionary<string, (decimal?, double?)> GetKPIsForPublishers(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("appsitesperformance", dataBaseFields);
            /*if (ids?.Length > 0)
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and PublisherId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }*/

            if (accountId.HasValue)
            {


                if (ids?.Length > 0)
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM appsite
                WHERE  appsite.Id=appsitesperformance.AppSiteId and appsite.AccountId=:accountid) and AppSiteId in (:ids) ");


                }
                else
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM appsite
                WHERE  appsite.Id=appsitesperformance.AppSiteId and appsite.AccountId=:accountid) ");

                }
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }

            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }

            if (accountId.HasValue)
            {
                query.SetInt32("accountid", accountId.Value);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }

        public IDictionary<string, (decimal?, double?)> GetKPIsForDataProviders(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids)
        {
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            var sql = BuildQuery("adfalcon_hdp.data_provider_performance", dataBaseFields);



            if (accountId.HasValue)
            {


                if (ids?.Length > 0)
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM business_partners
                WHERE  business_partners.id=data_provider_performance.DataProviderId and business_partners.AccountId=:accountid) and DataProviderId in (:ids) ");


                }
                else
                {
                    sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, @$"and EXISTS (SELECT 1 FROM business_partners
                WHERE  business_partners.id=data_provider_performance.DataProviderId and business_partners.AccountId=:accountid) ");

                }
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }

          
            
    


            /*if (ids?.Length > 0)
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, $"and DataProviderId in (:ids)");
            }
            else
            {
                sql = sql.Replace(METRICS_EXTRA_FILTERS_PLACEHOLDER, "");

            }*/
            var query = nhibernateSession.CreateSQLQuery(sql);
            if (ids?.Length > 0)
            {
                query.SetParameterList("ids", ids);
            }
            if (accountId.HasValue)
            {
                query.SetInt32("accountid", accountId.Value);
            }
            int fromInt = int.Parse(from.ToString("yyyyMMdd"));
            int toInt = int.Parse(to.ToString("yyyyMMdd"));
            if ((toInt - fromInt) <= 0 && toInt == today)
            {
                today = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            }
            query.SetInt32(PAST_DAY_PARAM, fromInt);
            query.SetInt32(PRESENT_DAY_PARAM, toInt);
            query.SetInt32(TODAY_PARAM, today);
            query.SetResultTransformer(new NHibernate.Transform.AliasToEntityMapResultTransformer());
            var results = query.List();

            return GetDBFieldToValuesMAp(results, dataBaseFields, fromInt, toInt);
        }


    }
}
