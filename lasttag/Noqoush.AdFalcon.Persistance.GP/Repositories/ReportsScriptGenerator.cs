using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Services;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.ConfigurationSetting;

namespace Noqoush.AdFalcon.Persistence.Reports.RepositoriesGP
{
    public enum ChartCase
    {
        Hours, SixHours, Day, Week, Month

    }
  
   
    public enum SubType
    {
        None = 0,
        Operator = 1,
        Platform = 2,
        Manufacturer = 3,
        GeoLocation = 4,
        Campaign = 5,
        AdGroup = 6,

        SubSite = 7,
        AppSite = 8,
        Audiance = 9

    }
    public class ReportGeneratorArgs : BaseGeneratorArgs
    {
        private static IConfigurationManager configurationManager;
        private static IAccountRepository accountRepository;
        private static IAccountStatistic accountStatistic;
        private static IAdvertiserAccountRepository advertiserAccountRepository;
        public const string DateField = "dateid";
        public const string HourDateField = "hourid";
        public const string MonthDateField = "monthid";
        public const string WeekDateField = "weekid";
        public static string DataBase = Noqoush.AdFalcon.Domain.Configuration.DB;
        public TrafficPlannerCriteriaDto TrafficPlannerCrt { get; set; }
        public ReportCriteriaDto Criteria { get; set; }
        public ChartCase ChartCase { get; set; }
        public string reportDataTableName { get; set; }
        public string groupDataTableName { get; set; }
        public SubType SubType { get; set; }
        public SubType SecondSubType { get; set; }
        public int Threshold { get; set; }
        public int Count { get; set; }

        //this data should be changed 
        public string AccountIdFieldName { get; set; }
        public string AdvertiserIdFieldName { get; set; }
        public string AdvertiserIdEqualFormat { get; set; }
        public int AccountId { get; set; }
        public bool IsAccumulated { get; set; }//"OperatorID"
        public string IdFieldName { get; set; }//"OperatorID"
        public string Fieldname { get; set; }//"OperatorID"
        public string RealFieldname { get; set; }//"OperatorID"
        public string TableIdName { get; set; }//"operators"

        public int AdvertiserId { get; set; }
        public string SecondIdFieldName { get; set; }//"OperatorID"
        public string SecondFieldname { get; set; }//"OperatorID"
        public string SecondTableIdName { get; set; }//"operators"

        public bool UseForDrillDownData { get; set; }
        public string DrillDownDataFilter { get; set; }
        public string SecondLocalizedStringFieldName { get; set; }
        public string LocalizedStringFieldName { get; set; }//"OperatorNameId"
        public Dictionary<string, string> FieldNames;

        public string DataTableName { get; set; }//"appsite"
        public string FilterDataFieldId { get; set; }//"AppSiteID"

        public string FilledRequestsId { get; set; }//"AppSiteID"
        public string DataFieldId { get; set; }//"AppSiteID"
        public string SelectDataIds { get; set; }//"AppSiteID"
        //public string SelectAllDataIds { get; set; }//"AppSiteID"
        public string DataFieldName { get; set; }//"Name"
        public string DataFieldNameToLower { get; set; }//"Name"
        public string DataFieldNameAlias { get; set; }//"SubName"
        public string SecondDataFieldNameAlias { get; set; }//" SecondSubName"
        public string SubQueryStr { get; set; }
        public Dictionary<string, string> DataFieldNames;

        static ReportGeneratorArgs()
        {
            configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>();
            accountRepository = Framework.IoC.Instance.Resolve<IAccountRepository>();
            accountStatistic = Framework.IoC.Instance.Resolve<IAccountStatistic>();
            advertiserAccountRepository = Framework.IoC.Instance.Resolve<IAdvertiserAccountRepository>();
        }
        //TODO:OSaleh to move this function to domain service 
        private static int GetAccountEntityCount(EntityType entityType, int accountId)
        {
            var count = 0;
            // get account
            var account = accountRepository.Get(accountId);
            if (account == null)
            {
                throw new DataNotFoundException();
            }
            switch (entityType)
            {
                
                case EntityType.App:
                    {
                        count = accountStatistic.GetAppCount(account);
                        break;
            }
                     case EntityType.Deal:
            accountStatistic.GetDealCount(account);
            break;
                case EntityType.Audiances:
                    accountStatistic.GetAudienceSegmentCount(account);
                    break;
                case EntityType.AudianceSegmentsForAdvertiser:
                case EntityType.Campaign:
                case EntityType.AdGroup:
                case EntityType.Ad:
                    {
                        count = accountStatistic.GetAdCount(account);
                        break;
                    }
            }
            return count;
        }
        private static string GetAccountEntityIds(EntityType entityType, int accountId)
        {
            IList<int> Ids = new List<int>();
            // get account
            var account = accountRepository.Get(accountId);
            if (account == null)
            {
                throw new DataNotFoundException();
            }
            switch (entityType)
            {
                case EntityType.Deal:

                    Ids = accountStatistic.GeDealsIds(account);
                    break;
                case EntityType.Audiances:

                    Ids = accountStatistic.GeAudienceSegmentsIds(account);
                    break;
                case EntityType.API:
                case EntityType.App:
                    {
                        Ids = accountStatistic.GetAppIds(account);
                        break;
                    }
                    case EntityType.AudianceSegmentsForAdvertiser:
                case EntityType.Campaign:
                case EntityType.AdGroup:
                case EntityType.Ad:
                    {
                        Ids = accountStatistic.GetAdIds(account);
                        break;
                    }
            }
            var idsStr = Ids.Aggregate(string.Empty, (current, c) => current + ("," + c.ToString()));
            return idsStr.Trim(',');
        }

        private static string GetAccountEntityIds(EntityType entityType, int accountId, int userId)
        {
            IList<int> Ids = new List<int>();
            // get account
            var account = accountRepository.Get(accountId);
            if (account == null)
            {
                throw new DataNotFoundException();
            }
            switch (entityType)
            {
                case EntityType.API:
                case EntityType.App:
                    {
                        Ids = accountStatistic.GetAppIdsPerUser(accountId, userId);
                        break;
                    }
                    case EntityType.AudianceSegmentsForAdvertiser:
                case EntityType.Campaign:
                    {
                        Ids = accountStatistic.GetCampaignIdsPerUser(accountId, userId);
                        break;
                    }
                case EntityType.Deal:
                    {
                        Ids = accountStatistic.GetDealIdsPerUser(accountId, userId);
                        break;

                    }
                case EntityType.Audiances:
                    {
                        Ids = accountStatistic.GetAudienceSegmentIdsPerUser(accountId, userId);
                        break;

                    }
                case EntityType.AdGroup:
                    {
                        Ids = accountStatistic.GetAdGroupIdsPerUser(accountId, userId);
                        break;

                    }

                case EntityType.Ad:
                    {
                        Ids = accountStatistic.GetAdIdsPerUser(accountId, userId);
                        break;
                    }
            }
            var idsStr = Ids.Aggregate(string.Empty, (current, c) => current + ("," + c.ToString()));
            return idsStr.Trim(',');
        }

        public static ReportGeneratorArgs GetInstance(ReportCriteriaDto criteria, int accountId, ReportType reportType, EntityType entityType, SubType subType, SubType SecondsubType= SubType.None, TrafficPlannerCriteriaDto TrafficPlannerCrt=null )
        {

            // Get Counts
            int threshold, count;
            int.TryParse(configurationManager.GetConfigurationSetting(null, null, "Threshold"), out threshold);
            count = GetAccountEntityCount(entityType, accountId);
          
            var instance = new ReportGeneratorArgs
            {
                Criteria = criteria,
                IsAccumulated=criteria.IsAccumulated,
              
                AccountId = accountId,
                ReportType = reportType,
                EntityType = entityType,
                SubType = subType,
               SecondSubType = SecondsubType,
                Count = count,
                Threshold = threshold,
                reportDataTableName= "report_data" + DateTime.Now.Ticks.ToString(),
                groupDataTableName= "grouped_data" + DateTime.Now.Ticks.ToString(),
                DataFieldNames = new Dictionary<string, string>(),
                FieldNames = new Dictionary<string, string>()
            };
            instance.DropStatements = "DROP TABLE IF EXISTS " + instance.reportDataTableName+";";
            instance.DropStatements += "DROP TABLE IF EXISTS " + instance.groupDataTableName + ";";
            instance.DropStatements += "DROP TABLE IF EXISTS " + instance.reportDataTableName + "_counts;";
      
           
            switch (entityType)
            {

                case EntityType.Deal:
                    {
                        instance.DataTableName = "dim_buyer_deals";
                        instance.DataFieldId = "DealId";
                        instance.FilterDataFieldId = "DealId";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "Name";
                        instance.SecondDataFieldNameAlias = "SecondSubName";
                        instance.FilledRequestsId = "requests_d";
                        instance.FactStatTable = "fact_deals_stat_h";
                        instance.DayFactStatTable = "fact_deals_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "appsite where AccountId={0}";
                        break;
                    }

                case EntityType.Audiances:
                    {
                        instance.DataTableName = "dim_business_partners";
                        instance.DataFieldId = "dataproviderid";
                        instance.FilterDataFieldId = "dataproviderid";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "DataProviderName";
                      

                        instance.FactStatTable = "fact_data_providers_d";
                        instance.DayFactStatTable = "fact_data_providers_d";
                        instance.WeekFactStatTable = "fact_data_providers_d";
                        instance.MonthFactStatTable = "fact_data_providers_d";
                        instance.AccountIdFieldName = "";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "appsite where AccountId={0}";
                        break;
                    }
                case EntityType.App:
                    {
                        instance.DataTableName = "dim_appsites";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "pubAccountID";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "appsite where AccountId={0}";
                        break;
                    }
                case EntityType.API:
                    {
                        instance.DataTableName = "dim_appsites";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldName = "PublisherId";
                        instance.DataFieldNameAlias = "aid";
                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "PubAccountID";
                        instance.SelectDataIds = "{0}";
                        instance.DataFieldNames.Add("Name", "an");
                        break;
                    }



                case EntityType.AudianceSegmentsForAdvertiser:
                    {
                        instance.DataTableName = "dim_campaigns";
                        instance.DataFieldId = "CampaignID";
                        instance.FilterDataFieldId = "CampaignID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_advertiser_segments_d";
                        instance.DayFactStatTable = "fact_advertiser_segments_d";
                        instance.WeekFactStatTable = "fact_advertiser_segments_w";
                        instance.MonthFactStatTable = "fact_advertiser_segments_m";
                        instance.AccountIdFieldName = "accountid";
                        instance.FilledRequestsId = "";

                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "campaigns where AccountId={0}";
                        break;
                    }
                case EntityType.Campaign:
                    {
                        instance.DataTableName ="dim_campaigns";
                        instance.DataFieldId = "CampaignID";
                        instance.FilterDataFieldId = "CampaignID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "advaccountId";
                        instance.FilledRequestsId = "requests_cr";

                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "campaigns where AccountId={0}";
                        break;
                    }
                case EntityType.AdGroup:
                    {

                        instance.DataTableName = "dim_adgroups";
                        instance.DataFieldId = "AdGroupID";
                        instance.FilterDataFieldId = "AdGroupID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldNameAlias = "SubName";
                 
                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "advaccountId";
                        instance.FilledRequestsId = "requests_ag";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = @"select " + DataBase + "adgroups.id FROM " + DataBase + "adgroups INNER JOIN " + DataBase + "campaigns ON " + DataBase + "adgroups.CampaignId = " + DataBase + "campaigns.Id where  AccountId={0}";
                        break;
                    }
                case EntityType.Ad:
                    {
                        instance.DataTableName ="dim_ads";
                        instance.DataFieldId = "AdId";
                        instance.FilterDataFieldId = "AdId";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameToLower = "groupbyname";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";
                        instance.AccountIdFieldName = "advaccountId";
                        instance.FilledRequestsId = "requests_ad";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = @"SELECT " + DataBase + "ads.Id FROM " + DataBase + "ads INNER JOIN " + DataBase + "adgroups ON " + DataBase + "ads.AdGroupId = " + DataBase + "adgroups.Id INNER JOIN " + DataBase + "campaigns on " + DataBase + "adgroups.CampaignId = campaigns.Id WHERE " + DataBase + "campaigns.AccountId ={0}";
                        break;
                    }
            }
        
            switch (subType)
            {
                case SubType.Campaign:
                    {
                        instance.TableIdName = "dim_campaigns";
                        instance.Fieldname = ",CampID";
                        instance.RealFieldname = ",CampaignID";
                        instance.IdFieldName = "Id";
                        instance.FilledRequestsId = "requests_dca";
                        instance.LocalizedStringFieldName = "Name";
                        break;
                    }

                case SubType.Audiance:
                    {
                        instance.TableIdName = "dim_audience_segments";
                        instance.Fieldname = ",segmentid";
                        instance.RealFieldname = ",segmentid";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "name_";
                        break;
                    }
                case SubType.Operator:
                    {
                        instance.TableIdName ="dim_operators";
                        instance.Fieldname = ",mobileoperatorid";
                        instance.RealFieldname = ",mobileoperatorid";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "operator_";
                        break;
                    }
                case SubType.AppSite:
                    {
                        instance.TableIdName = "dim_appsites";
                        instance.Fieldname = ",appsiteid";
                        instance.RealFieldname = ",appsiteid";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "Name";
                        break;
                    }
                case SubType.Platform:
                    {
                        instance.TableIdName = "dim_platforms";
                        instance.Fieldname = ",deviceosid";
                        instance.RealFieldname = ",deviceosid";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "platform_";
                        break;
                    }
                case SubType.Manufacturer:
                    {
                        instance.TableIdName =  "dim_manufacturers";
                        instance.Fieldname = ",devicebrandid";
                        instance.RealFieldname = ",devicebrandid";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = " manufacturer_";
                        break;
                    }
                case SubType.GeoLocation:
                    {
                        instance.TableIdName =  "dim_locations";
                        instance.Fieldname = ",CountryID";
                        instance.RealFieldname = ",CountryID";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "location_";
                        switch (entityType)
                        {
                            case EntityType.API:
                                instance.FieldNames.Add("Code1", "cc");
                                break;
                            default:
                                break;
                        }
                        break;
                    }
            }
            switch (SecondsubType)
            {
                case SubType.AdGroup:
                    {
                        instance.SecondTableIdName = "dim_adgroups";
                        instance.SecondFieldname = ",AdGroupID";
                        instance.FilledRequestsId = "requests_dag";
                        instance.SecondIdFieldName = "Id";
                        instance.SecondLocalizedStringFieldName = "Name";
                        break;
                    }

                case SubType.SubSite:
                    {
                        instance.SecondTableIdName = "dim_sub_appsites";
                        instance.SecondFieldname = ",subappsiteid";
                        //instance.RealFieldname = ",subappsiteid";
                        instance.SecondIdFieldName = "Id";
                        instance.SecondLocalizedStringFieldName = "SubPublisherName";
                        break;
                    }
            }

           //if (TrafficPlannerCrt == null)
            //{
                if (criteria.AccountAdvertiserId > 0)
                {

                   // instance.AdvertiserIdFieldName = "advassociationid";
                    instance.AdvertiserIdEqualFormat = " and " + "   advassociationid" + "=" + instance.Criteria.AccountAdvertiserId + " ";
                    if (criteria.AdvertiserId > 0)
                    {
                        instance.AdvertiserIdFieldName = "advertiserid";
                        instance.AdvertiserIdEqualFormat = instance.AdvertiserIdEqualFormat + " and " + instance.AdvertiserIdFieldName + "=" + instance.Criteria.AdvertiserId + " ";
                    }

                }
                else if (entityType != EntityType.App && entityType != EntityType.API && !criteria.IsPrimaryUser && string.IsNullOrEmpty(instance.Criteria.ItemsList))
                {

                    var ids = accountStatistic.GetNotAllowedAdvertiserAsscoiation(accountId, criteria.userId);
                    if (ids != null && ids.Count > 0)
                    {
                        var idsStr = ids.Aggregate(string.Empty, (current, c) => current + ("," + c.ToString()));
                        var result = idsStr.Trim(',');

                        instance.AdvertiserIdFieldName = "advassociationid";
                        instance.AdvertiserIdEqualFormat = " and " + instance.AdvertiserIdFieldName + " Not In (" + result + " ) ";
                    }
                }
                if (!criteria.IsPrimaryUser && string.IsNullOrEmpty(instance.Criteria.ItemsList))
                { instance.Criteria.ItemsList = GetAccountEntityIds(entityType, accountId, criteria.userId); }
                //else if (criteria.AdvertiserId > 0)
                //{
                //    AdvertiserAccount advertiserAccount = advertiserAccountRepository.Query(x => x.Advertiser.ID == criteria.AdvertiserId && x.Account.ID ==criteria.AccountId).FirstOrDefault();
                //    if (advertiserAccount != null)
                //    {
                //        instance.AdvertiserIdFieldName = "advassociationid";
                //        instance.AdvertiserIdEqualFormat = " and " + instance.AdvertiserIdFieldName + "=" + instance.Criteria.AccountAdvertiserId + " ";
                //    }
                //    else
                //    {
                //        instance.AdvertiserIdFieldName = "AdvertiserId";
                //        instance.AdvertiserIdEqualFormat = " and " + instance.AdvertiserIdFieldName + "=" + instance.Criteria.AdvertiserId + " ";
                //    }
                //}
                if ((string.IsNullOrWhiteSpace(instance.Criteria.ItemsList)) && (instance.Count < instance.Threshold))
                {
                    // instance.Criteria.ItemsList = string.Format(instance.SelectAllDataIds, instance.AccountId.ToString());
                    instance.Criteria.ItemsList = GetAccountEntityIds(entityType, accountId);
                    instance.SelectDataIds = "{0}";
                }
            //}
            //if (TrafficPlannerCrt!=null)
            //{
            //    instance.AccountIdFieldName = string.Empty;
            //    instance.BuildTrafficPlannerCriteria();
           //}
            return instance;
        }

        //public  void BuildTrafficPlannerCriteria()
        //{
        //    var countAnd = 0;

        //    if (TrafficPlannerCrt.Countries != null && TrafficPlannerCrt.Countries.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format("  EXISTS (Select loc.id  from dim_locations loc  where parentid IN (0)  and loc.id= countryid )", string.Join(",", TrafficPlannerCrt.Countries));
        //        countAnd++;
        //    } 
        //    if (TrafficPlannerCrt.Countries != null && TrafficPlannerCrt.Countries.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  countryid IN({0}) ", string.Join(",", TrafficPlannerCrt.Countries),countAnd > 0 ? " And " : string.Empty);
        //        countAnd++;
        //    }


        //    if (TrafficPlannerCrt.Operators != null && TrafficPlannerCrt.Operators.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  mobileoperatorid IN({0}) ", string.Join(",", TrafficPlannerCrt.Operators), countAnd>0?  " And ": string.Empty);

        //        countAnd++;
        //    }

        //    if (TrafficPlannerCrt.Platforms != null && TrafficPlannerCrt.Platforms.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} deviceosid IN({0}) ", string.Join(",", TrafficPlannerCrt.Platforms), countAnd > 0 ? " And " : string.Empty);
        //        countAnd++;
        //    }

        //    if (TrafficPlannerCrt.GenderType > 0)
        //    {

        //        DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} genderid ={0} ", TrafficPlannerCrt.GenderType, countAnd > 0 ? " And " : string.Empty);


        //        countAnd++;
        //    }

        //    //if (TrafficPlannerCrt.AgeGroups != null && TrafficPlannerCrt.AgeGroups.Length > 0)
        //    //{
        //    //    DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} agegroupid IN({0}) ", string.Join(",", TrafficPlannerCrt.AgeGroups), countAnd > 0 ? " And " : string.Empty);
        //    //    countAnd++;

        //    //}

        //    if (TrafficPlannerCrt.languages != null && TrafficPlannerCrt.languages.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  languageid IN({0}) ", string.Join(",", TrafficPlannerCrt.languages), countAnd > 0 ? " And " : string.Empty);
        //        countAnd++;
        //    }

        //    if (TrafficPlannerCrt.DeviceTypeId > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format("  {1} EXISTS(Select id from dim_devices where devicetypeId={0}  and id = devicemodelid	)  ", TrafficPlannerCrt.DeviceTypeId, countAnd > 0 ? " And " : string.Empty);
        //        countAnd++;
        //    }

        //    if (TrafficPlannerCrt.AdSizes != null && TrafficPlannerCrt.AdSizes.Length > 0)
        //    {
        //        DrillDownDataFilter = DrillDownDataFilter + string.Format("  {1} EXISTS( Select crvGroup.creativeunitgroupid from creativeunitgroups_creativeunitids crvGroup  where creativeunitid IN ({0})  and crvGroup.creativeunitgroupid = creativeunitgroupid )  ", string.Join(",", TrafficPlannerCrt.AdSizes), countAnd > 0 ? " And " : string.Empty);
        //        countAnd++;
        //    }

         

           

			
        //}
    }
    public static class ScriptGenerator
    {
        public static string GenerateQueryScript(ReportGeneratorArgs args)
        {

            if (args.Criteria.SummaryBy==(int)SummaryBy.Accumulated)
            {
               return  GenerateQueryScriptUnion(args);
            }
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();
            //get generate temp table command
            reportCommand.AppendLine(GenerateTempTableCommand(args));
            List<DateTime> partialDates;
            switch (args.Criteria.SummaryBy)
            {
                case 0: //Hour
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.DayFactStatTable = tableName;

                        //}
                        if (tableInfo!=null && tableInfo.ContainsKey(args.FactStatTable))
                        {
                            reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.FactStatTable], SummaryBy.Hour, args));
                        }
                        break;
                    }
                case 1: //day
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Day);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.DayFactStatTable = tableName;
                           
                        //}
                        reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args));
                        break;
                    }

                case 2: //week
                    {
                        if ((args.Criteria.FromDate.DayOfWeek != DayOfWeek.Sunday) ||
                            (args.Criteria.ToDate.DayOfWeek != DayOfWeek.Saturday))
                        {
                            // Partial Dates
                            //get partial weeks dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate,args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                               out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;
                             
                            //}
                            reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }


                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            List<int> weekno = new List<int>();
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week, weekno);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.WeekFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                                reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.WeekFactStatTable], SummaryBy.Week, args));
                        }
                        break;
                    }
                case 4:
                    {


                        if ((args.Criteria.FromDate.Day != 1) ||
                       (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;
               
                            //}

                            reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                                reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.MonthFactStatTable], SummaryBy.Accumulated, args));
                        }
                        break;
                    }
                case 3: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate,args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;
                               
                            //}

                            reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;
                               
                            //}
                            if(tableInfo.Keys.Count>0)
                            reportCommand.AppendLine(GenerateSelectCommand(tableInfo[args.MonthFactStatTable], SummaryBy.Month, args));
                        }
                        break;
                    }
            }

            reportCommand.AppendLine(GenerateGroupByNameTapleCommand(args));

            reportCommand.AppendLine(GenerateGroupByNameInsertCommand(args));

            //reportCommand.AppendLine(GenerateTempTableCountCommand(GenerateSelectCountCommand(args)));
            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
           // reportCommand.AppendLine(GenerateDropGroupByNameTempTaple(args));
            //reportCommand.AppendLine(GenerateDropTempTaple());
            
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString().Trim('\n').Trim();
        }
        public static string GenerateQueryScriptUnion(ReportGeneratorArgs args)
        {
            string selectStatmen = string.Empty;
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();

            int countselect =0;
            //get generate temp table command
            reportCommand.AppendLine(GenerateTempTableCommand(args));
            string groupByVar = string.Empty;
                reportCommand.AppendLine(GenerateInsertSelectCommand((SummaryBy)args.Criteria.SummaryBy,args));
            reportCommand.AppendLine(" {0} from (  ");
            List<DateTime> partialDates;
            switch (args.Criteria.SummaryBy)
            {
                case 0: //Hour
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.DayFactStatTable = tableName;

                        //}
                        countselect++;
                        selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args, ref groupByVar, null);
                        reportCommand.AppendLine(selectStatmen);
                        break;
                    }
                case 1: //day
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Day);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.DayFactStatTable = tableName;

                        //}
                        countselect++;
                        selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args, ref groupByVar, null);
                       reportCommand.AppendLine(selectStatmen);
                        break;
                    }

                case 2: //week
                    {
                        if ((args.Criteria.FromDate.DayOfWeek != DayOfWeek.Sunday) ||
                            (args.Criteria.ToDate.DayOfWeek != DayOfWeek.Saturday))
                        {
                            // Partial Dates
                            //get partial weeks dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                               out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}
                            countselect++;
                            selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args, ref groupByVar, null);
                            reportCommand.AppendLine(selectStatmen);
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }


                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            List<int> weekno = new List<int>();
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week, weekno);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.WeekFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 0)
                                    reportCommand.AppendLine(" union All ");
                                countselect++;
                                selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.WeekFactStatTable], SummaryBy.Week, args, ref groupByVar, null);
                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
                case 4:
                    {


                        if ((args.Criteria.FromDate.Day != 1) ||
                       (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}
                            
                                countselect++;
                                selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args, ref groupByVar, null);

                                reportCommand.AppendLine(selectStatmen);
                                args.Criteria.FromDate = newDateFrom;
                                args.Criteria.ToDate = newDateTo;
                           
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;

                            //}

                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 0)
                                    reportCommand.AppendLine(" union All ");

                                selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.MonthFactStatTable], SummaryBy.Accumulated, args, ref groupByVar, null);
                                countselect++;
                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
                case 3: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}
                            countselect++;
                            selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args, ref groupByVar, null);
                            reportCommand.AppendLine(selectStatmen);
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 1)
                                    reportCommand.AppendLine(" union All ");
                                countselect++;
                                selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.MonthFactStatTable], SummaryBy.Month, args, ref groupByVar, null);

                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
            }
            if (countselect > 1)
            {
                if (string.IsNullOrEmpty(groupByVar.Trim()))
                {
                    groupByVar = "group by DateID";
                }
                reportCommand.AppendLine(") as tempUnion   " + groupByVar);

            }
            else
                reportCommand.AppendLine(";  ");
            selectStatmen = GenerateFinalSelectForUnionCommand((SummaryBy)args.Criteria.SummaryBy, args, null);
            //reportCommand = string.Format(reportCommand, selectStatmen);
            //selectStatmen = selectStatmen.Substring(0, selectStatmen.LastIndexOf("from"));

            reportCommand.AppendLine(GenerateGroupByNameTapleCommand(args));

            reportCommand.AppendLine(GenerateGroupByNameInsertCommand(args));

            //reportCommand.AppendLine(GenerateTempTableCountCommand(GenerateSelectCountCommand(args)));
            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            // reportCommand.AppendLine(GenerateDropGroupByNameTempTaple(args));
            //reportCommand.AppendLine(GenerateDropTempTaple());

            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            var resultstat= reportCommand.ToString().Trim('\n').Trim();

            if (countselect == 1)
            {
                resultstat= resultstat.Replace(" {0} from (  ", string.Empty);
            }
            selectStatmen = selectStatmen.Replace("dateid as newDateID", "newDateID as DateID");
            resultstat=string.Format(resultstat, selectStatmen);
            return resultstat;

        }
        public static string GenerateChartSelectScript(ReportGeneratorArgs args)
        {
            if (args.ReportType != ReportType.Chart)
            {
                throw new NotImplementedException();
            }
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();
            var dateDiff = args.Criteria.ToDate.Subtract(args.Criteria.FromDate).TotalDays;

            //get generate temp table command
            reportCommand.AppendLine(GenerateTempTableCommand(args));

            List<DateTime> partialDates;
            if (dateDiff <= 1)
            {
                args.ChartCase = ChartCase.Hours;
            }
            else if ((dateDiff > 1) && (dateDiff <= 7))
            {
                args.ChartCase = ChartCase.SixHours;
            }
            else if ((dateDiff > 7) && (dateDiff <= 122))
            {
                args.ChartCase = ChartCase.Day;
            }
            else if ((dateDiff > 122) && (dateDiff <= 180))
            {
                args.ChartCase = ChartCase.Week;
            }
            else if (dateDiff > 180)
            {
                args.ChartCase = ChartCase.Month;
            }
            if (args.EntityType == EntityType.Deal && args.SubType==SubType.AdGroup)
            {
                args.ChartCase = ChartCase.Day;
            }
            if (args.EntityType ==EntityType.Audiances || args.EntityType== EntityType.AudianceSegmentsForAdvertiser &&( args.ChartCase== ChartCase.Hours || args.ChartCase == ChartCase.SixHours))
            {
                args.ChartCase = ChartCase.Day;

            }
            switch (args.ChartCase)
            {
                case ChartCase.Hours: //Hours
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.FactStatTable = tableName;
                           
                        //}

                        reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.FactStatTable], args));
                        break;
                    }
                case ChartCase.SixHours: //day
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.FactStatTable = tableName;
                         
                        //}

                        reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.FactStatTable], args));
                        break;
                    }
                case ChartCase.Day: //Day
                    {

                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Day);
                        //foreach (var table in tableInfo)
                        //{
                        //    var tableName = table.Key;
                        //    var tableDays = table.Value;
                        //    args.DayFactStatTable = tableName;
                           
                        //}

                        reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.DayFactStatTable], args));
                        break;
                    }

                case ChartCase.Week: //week
                    {
                        if ((args.Criteria.FromDate.DayOfWeek != DayOfWeek.Sunday) ||
                            (args.Criteria.ToDate.DayOfWeek != DayOfWeek.Saturday))
                        {
                            // Partial Dates
                            //get partial weeks dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate,args.Criteria.ToDate, SummaryBy.Week, out newDateFrom, out newDateTo);

                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;
                               
                            //}
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.DayFactStatTable], args, SummaryBy.Day));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            List<int> weeknumber = null;
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week, weeknumber);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.WeekFactStatTable = tableName;
                        
                            //}

                            reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.WeekFactStatTable], args, SummaryBy.Week));
                        }
                        break;
                    }

                case ChartCase.Month: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            /*// Partial Dates
                            //get partial Month dates*/
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate,args.Criteria.ToDate, SummaryBy.Month,
                                                               out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;
                                
                            //}
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.DayFactStatTable], args, SummaryBy.Day));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                                
                            //}

                            //args.MonthFactStatTable = tableName;
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableInfo[args.MonthFactStatTable], args, SummaryBy.Month));
                        }
                        break;
                    }
            }


            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
          //  reportCommand.AppendLine(GenerateDropTempTaple());
            
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString().Trim('\n').Trim(); ;
        }

        private static string GenerateTempTableCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;
            string timeIdColumn = string.Empty;
            string timeIdIndex = string.Empty;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        var idColumn = string.Empty;
                        var key = string.Empty;
                        var filterByColumn = string.IsNullOrWhiteSpace(args.Fieldname)
                                         ? string.Empty
                                         : string.Format("{0} int  DEFAULT NULL,", args.Fieldname.Trim(','));
                        key = string.IsNullOrWhiteSpace(args.Fieldname)
                                         ? string.Empty
                                         : string.Format("{0}", args.Fieldname);

                         filterByColumn = string.IsNullOrWhiteSpace(args.SecondFieldname)
                                         ? filterByColumn
                                         : filterByColumn + string.Format("{0} int  DEFAULT NULL,", args.SecondFieldname.Trim(','));
                        key = string.IsNullOrWhiteSpace(args.SecondFieldname)
                                         ? key
                                         :  key+string.Format("{0}", args.SecondFieldname);


                        if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                        {
                            idColumn = string.Format("{0} int NOT NULL,", args.DataFieldId);
                           // if (args.Criteria.SummaryBy != (int)SummaryBy.Accumulated)
                                key = string.Format(",{0}", args.DataFieldId) + key;
                            //else
                            //{
                            //    key = string.Format("{0}", args.DataFieldId) + key;

                            //    if (string.IsNullOrEmpty(key))
                            //    {

                            //        idColumn = string.Format("{0} int NOT NULL", args.DataFieldId);
                            //    }

                            //}
                        }
                        //if (args.Criteria.SummaryBy != (int)SummaryBy.Accumulated)
                        //{
                        //    idColumn = "," + idColumn;

                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(idColumn))
                        //    {
                        //        idColumn = "," + idColumn;
                        //    }
                        //    else if(!string.IsNullOrEmpty(key))
                        //    {
                        //        idColumn = "," + idColumn;
                        //    }


                        //}
                        switch (args.Criteria.SummaryBy)
                        {
                           
                            case 0:
                                timeIdColumn = "TimeId int NULL,";
                                timeIdIndex = ",TimeId";
                                break;
                         
                              
                            default:
                                break;
                        }

                        switch (args.EntityType)
                        {
                            case EntityType.Deal:
                                str = string.Format(
   @"DROP TABLE IF EXISTS {5};DROP TABLE IF EXISTS {5}_counts;
create temporary table {5} (
DateID int NOT NULL,
{0}{1}

DisplayedImpressions bigint DEFAULT '0',

requests_dcr bigint DEFAULT '0',
unfilledrequests bigint DEFAULT '0',
WonImpressions bigint DEFAULT '0',

AvailableImpressions  bigint DEFAULT '0',{2}
 CONSTRAINT operator_by_day UNIQUE(DateID{3}{4})
);CREATE temporary TABLE {5}_counts (
   relname  text PRIMARY KEY,
   reltuples   numeric,TotalAvailableImpressions bigint); ", timeIdColumn, filterByColumn, idColumn, timeIdIndex, key , args.reportDataTableName);
                                args.OrderByStruct = string.Format("DateID{0}{1}", timeIdIndex, key);
                                break;

                            case EntityType.Audiances:
                                str = string.Format(
   @"DROP TABLE IF EXISTS {5};DROP TABLE IF EXISTS {5}_counts;
create temporary table {5} (
DateID int NOT NULL,
{0}{1}

Impressions bigint DEFAULT '0',
AdvertiserId int NOT NULL,
AccountId int NOT NULL,
billedsegment int NOT NULL,
Campaignid int NOT NULL,
discount decimal(21,12) DEFAULT '0.00000',
grossrevenue  decimal(21,12) DEFAULT '0.00000',
avrcost   decimal(21,12) DEFAULT '0.00000',
Revenue decimal(21,12) DEFAULT '0.00000'

,UsedSegmentsId  int NOT NULL,{2}
 CONSTRAINT operator_by_day UNIQUE(DateID{3}{4})
);CREATE temporary TABLE {5}_counts (
   relname  text PRIMARY KEY,
   reltuples   numeric,TotalAvailableImpressions bigint); ", timeIdColumn, filterByColumn, idColumn, timeIdIndex, key+",Campaignid, UsedSegmentsId,billedsegment", args.reportDataTableName);
                                args.OrderByStruct = string.Format("DateID DESC{0}{1}", timeIdIndex, key+",Campaignid, UsedSegmentsId,billedsegment");
                                break;
                            case EntityType.API:
                            case EntityType.App:
                                {

                                   
                                        str = string.Format(
    @"DROP TABLE IF EXISTS {5};DROP TABLE IF EXISTS {5}_counts;
create temporary table {5} (
DateID int NOT NULL,
{0}{1}
Requests bigint DEFAULT '0',
Impressions bigint DEFAULT '0',
Clicks bigint DEFAULT '0',

Revenue decimal(21,12) DEFAULT '0.00000',{2}
 CONSTRAINT operator_by_day UNIQUE(DateID{3}{4})
);CREATE temporary TABLE {5}_counts (
   relname  text PRIMARY KEY,
   reltuples   numeric); ", timeIdColumn, filterByColumn, idColumn, timeIdIndex, key, args.reportDataTableName);
                                    args.OrderByStruct = string.Format("DateID{0}{1}", timeIdIndex, key);



                                    break;
                                }

                            case EntityType.AudianceSegmentsForAdvertiser:
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                
                                        str = string.Format(
@"DROP TABLE IF EXISTS {5};DROP TABLE IF EXISTS {5}_counts;
create temporary table {5} (
DateID int NOT NULL,
{0}{1}
Impressions bigint DEFAULT '0',
Clicks bigint DEFAULT '0',
RequestsByType bigint DEFAULT '0',
WonImpressions bigint DEFAULT '0',

 conv_pr bigint DEFAULT '0',
conv_pr_ct bigint DEFAULT '0',
conv_pr_vt bigint DEFAULT '0',
 conv_ot bigint DEFAULT '0',
conv_ot_ct bigint DEFAULT '0',
conv_ot_vt bigint DEFAULT '0',
conv_pr_rev decimal(21,12) DEFAULT '0.00000',
conv_pr_ct_rev decimal(21,12)  DEFAULT '0.00000',
conv_pr_vt_rev decimal(21,12) DEFAULT '0.00000',
conv_ot_rev decimal(21,12)  DEFAULT '0.00000',
conv_ot_ct_rev decimal(21,12) DEFAULT '0.00000',
conv_ot_vt_rev decimal(21,12) DEFAULT '0.00000',

vcreativeviews bigint DEFAULT '0',
vstart bigint DEFAULT '0',
vfirstquartile bigint DEFAULT '0',
vmidpoint bigint DEFAULT '0',
vthirdquartile bigint DEFAULT '0',
vcomplete bigint DEFAULT '0',
custom_events bigint DEFAULT '0',
pageviews bigint DEFAULT '0',
NetCost  decimal(21,12) DEFAULT '0.00000',
AdjustedNetCost  decimal(21,12) DEFAULT '0.00000',
GrossCost   decimal(21,12) DEFAULT '0.00000',
platformfee  decimal(21,12) DEFAULT '0.00000',
DataFee  decimal(21,12) DEFAULT '0.00000',
thirdpartyfee  decimal(21,12) DEFAULT '0.00000',
AgencyRevenue   decimal(21,12) DEFAULT '0.00000',
BillableCost  decimal(21,12) DEFAULT '0.00000',{2}
CONSTRAINT operator_by_day UNIQUE(DateID{3}{4})
) ; CREATE temporary TABLE {5}_counts (
   relname  text PRIMARY KEY,
   reltuples   numeric);", timeIdColumn, filterByColumn, idColumn, timeIdIndex, key, args.reportDataTableName);
                                    args.OrderByStruct = string.Format("DateID{0}{1}", timeIdIndex, key);
                                    break;
                                }

                        }
                        break;
                    }
                case ReportType.Chart:
                    {

                        str = string.Format(
                           @"DROP TABLE IF EXISTS {0};
create temporary table {0} (
Id SERIAL,
Xaxis timestamp NOT NULL,
Yaxis decimal(21,12) ,
PRIMARY KEY (Id)
); ", args.reportDataTableName);


                        if (args.Criteria.MetricCode.ToLower() == "adrequests")
                        {
                            str = string.Format(
                            @"DROP TABLE IF EXISTS {0};
create temporary table {0} (
Id SERIAL,
Xaxis timestamp NOT NULL,
Yaxis decimal(28,12) ,
PRIMARY KEY (Id)
); ", args.reportDataTableName);
                        }
                        break;
                    }
            }
            // if (args.Criteria.SummaryBy == (int)SummaryBy.Accumulated)
            //    {
            //    str = str.Replace("CONSTRAINT operator_by_day UNIQUE(DateID,", "CONSTRAINT operator_by_day UNIQUE(");
            //    str = str.Replace("CONSTRAINT operator_by_day UNIQUE(DateID)", "");
            //    str = str.Replace("DateID int NOT NULL", "DateID int NOT NULL  DEFAULT '1'");
                
            //}
        
            return str;
        }

        private static string GenerateGroupByNameTapleCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;

            string timeIdColumn = string.Empty;
            string timeIdIndex = string.Empty;


            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                switch (args.ReportType)
                {
                    case ReportType.Report:

                        var idColumn = string.Empty;
                        var key = string.Empty;
                        var keylower = string.Empty;
                        idColumn = string.Format("{0} VARCHAR(255) NOT NULL,{1} VARCHAR(255) NOT NULL,", args.DataFieldNameAlias, args.DataFieldNameAlias+"lower");
                       // if (args.Criteria.SummaryBy != (int)SummaryBy.Accumulated)
                            key = string.Format(",{0}", args.DataFieldNameAlias) + key;
                        keylower = string.Format(",{0}", args.DataFieldNameAlias + "lower") + keylower;
                        //else
                        //    key = string.Format("{0}", args.DataFieldNameAlias) + key;

                        switch (args.Criteria.SummaryBy)
                        {
                      
                            case 0:
                                timeIdColumn = "TimeId int Default NULL,";
                                timeIdIndex = ",TimeId";
                                break;
                            default:
                                break;
                        }


                        switch (args.EntityType)
                        {
                            case EntityType.AudianceSegmentsForAdvertiser:
                            case EntityType.Audiances:
                            case EntityType.Deal:
                            case EntityType.API:
                            case EntityType.App:
                                break;
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                               // if (args.Criteria.SummaryBy != (int)SummaryBy.Accumulated)
                                    str = string.Format(@"DROP TABLE IF EXISTS {4};
                                create temporary table {4} (
                               
                                DateID int NOT NULL,
                                {0}
                                Impressions bigint DEFAULT '0',
                                Clicks bigint DEFAULT '0',
                                RequestsByType bigint DEFAULT '0',
                                WonImpressions bigint DEFAULT '0',

 conv_pr bigint DEFAULT '0',
conv_pr_ct bigint DEFAULT '0',
conv_pr_vt bigint DEFAULT '0',
 conv_ot bigint DEFAULT '0',
conv_ot_ct bigint DEFAULT '0',
conv_ot_vt bigint DEFAULT '0',
conv_pr_rev decimal(21,12) DEFAULT '0.00000',
conv_pr_ct_rev decimal(21,12)  DEFAULT '0.00000',
conv_pr_vt_rev decimal(21,12) DEFAULT '0.00000',
conv_ot_rev decimal(21,12)  DEFAULT '0.00000',
conv_ot_ct_rev decimal(21,12) DEFAULT '0.00000',
conv_ot_vt_rev decimal(21,12) DEFAULT '0.00000',
vcreativeviews bigint DEFAULT '0',
vstart bigint DEFAULT '0',
vfirstquartile bigint DEFAULT '0',
vmidpoint bigint DEFAULT '0',
vthirdquartile bigint DEFAULT '0',
vcomplete bigint DEFAULT '0',
custom_events bigint DEFAULT '0',
pageviews bigint DEFAULT '0',
      NetCost  decimal(21,12) DEFAULT '0.00000',
AdjustedNetCost  decimal(21,12) DEFAULT '0.00000',
GrossCost   decimal(21,12) DEFAULT '0.00000',
platformfee  decimal(21,12) DEFAULT '0.00000',
DataFee  decimal(21,12) DEFAULT '0.00000',
thirdpartyfee  decimal(21,12) DEFAULT '0.00000',
AgencyRevenue   decimal(21,12) DEFAULT '0.00000',
BillableCost  decimal(21,12) DEFAULT '0.00000',{1}
                                
                                CONSTRAINT grouped_index UNIQUE(DateID{2}{3})
                                ) ;", timeIdColumn, idColumn, timeIdIndex, key,args.groupDataTableName);
                                args.OrderByStruct = string.Format("DateID{0}{1}", timeIdIndex, keylower);
                                //else
                                //    str = string.Format(@"DROP TABLE IF EXISTS {4};
                                //create temporary table {4} (
                                //DateID int  NULL,
                                //{0}
                                //Impressions int DEFAULT '0',
                                //Clicks int DEFAULT '0',
                                //Spend decimal(17,5) DEFAULT '0.00000',{1}
                                //CONSTRAINT grouped_index UNIQUE({3})
                                //) ;", timeIdColumn, idColumn, timeIdIndex, key, args.groupDataTableName);
                                break;
                            default:
                                break;
                        }
                        break;
                    case ReportType.Chart:
                        break;
                    default:
                        break;
                }
            }

            return str;
        }

        private static string GenerateTempTableSelectCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;
        
                switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        str = GenerateReportTempTableSelectCommand(args);
                        break;
                    }
                case ReportType.Chart:
                    {
                        str = string.Format("select Xaxis,Yaxis from {0};", args.reportDataTableName);
                        if (args.Criteria.MetricCode.ToLower() == "adrequests")
                        {

                            str = string.Format("select Xaxis,Yaxis AS Yaxis from {0};", args.reportDataTableName);
                        }
                     
                        break;
                    }
            }
            return str;
        }

        private static string GenerateGroupByNameInsertCommand(ReportGeneratorArgs args)
        {
            string str = string.Empty;
            var columnNameSQLgr = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
            string groupBY = string.Empty;

            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                groupBY = "group by Date,";
                if (args.IsAccumulated)
                {
                    groupBY = "group by ";

                }
                columnNameSQLgr += string.Format("{0}, ", args.DataFieldNameAlias);

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQLgr += " TimeId, ";
                    groupBY += " TimeId,";
                }

                groupBY += args.DataFieldNameAlias+"lower";
            }
            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                string timeIdColumn;

                timeIdColumn = string.Empty;

                switch (args.Criteria.SummaryBy)
                {
               
                    case 0:
                        timeIdColumn = ",TimeId";
                        break;
                    default:
                        break;
                }

                string selectCommand = string.Empty;

                string insertStatement = string.Format(@"insert into {2}(DateID,{1}{0},Impressions,Clicks,RequestsByType,WonImpressions,



 conv_pr,
conv_pr_ct,
conv_pr_vt,
 conv_ot,
conv_ot_ct,
conv_ot_vt,
conv_pr_rev,
conv_pr_ct_rev,
conv_pr_vt_rev,
conv_ot_rev,
conv_ot_ct_rev,
conv_ot_vt_rev,


vcreativeviews,
vstart,
vfirstquartile,
vmidpoint,
vthirdquartile,
vcomplete,
custom_events,
pageviews,
NetCost,
AdjustedNetCost,
GrossCost,
platformfee,
DataFee,
thirdpartyfee,
AgencyRevenue,
BillableCost
) ", timeIdColumn, args.DataFieldNameAlias+","+ args.DataFieldNameAlias+"lower", args.groupDataTableName);

                var innerJoin = string.Empty;
                var columnNameSQL = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
                if (args.IsAccumulated)
                {
                     columnNameSQL = string.Format("1 AS Date,", ReportGeneratorArgs.DateField);
                }

                //  use this dummy where to force using the index
                var dummyWhere = " WHERE  DateID>0 ";

                columnNameSQL += string.Format("min( {0}.{1} ) AS {2}, ", args.DataTableName, args.DataFieldName, args.DataFieldNameAlias);
                columnNameSQL += string.Format(" {0}.{1}  AS {2} , ", args.DataTableName, args.DataFieldNameToLower, args.DataFieldNameAlias + "lower");
                innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQL += " TimeId, ";
                }
                //if (args.Criteria.SummaryBy == 2)
                //{
                //    columnNameSQL += " TimeId, ";
                //}
                switch (args.EntityType)
                {
                    case EntityType.AudianceSegmentsForAdvertiser:
                    case EntityType.Audiances:
                    case EntityType.Deal:
                    case EntityType.API:
                    case EntityType.App:
                        break;
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                         selectCommand = string.Format(
                            @"SELECT  {0} 
CAST(SUM(Impressions) AS bigint) AS Impress,
CAST(SUM(Clicks) AS bigint) AS Clicks,
CAST(SUM(RequestsByType) AS bigint) AS RequestsByType,
CAST(SUM(WonImpressions) AS bigint) AS WonImpressions,

 CAST(SUM(conv_pr) AS bigint) AS conv_pr,
CAST(SUM(conv_pr_ct) AS bigint) AS conv_pr_ct,
CAST(SUM(conv_pr_vt) AS bigint) AS conv_pr_vt,
 CAST(SUM(conv_ot) AS bigint) AS conv_ot,
CAST(SUM(conv_ot_ct) AS bigint) AS conv_ot_ct,
CAST(SUM(conv_ot_vt) AS bigint) AS conv_ot_vt,


SUM(conv_pr_rev) AS conv_pr_rev,
SUM(conv_pr_ct_rev) AS  conv_pr_ct_rev,
SUM(conv_pr_vt_rev) AS  conv_pr_vt_rev,
SUM(conv_ot_rev) AS  conv_ot_rev,
SUM(conv_ot_ct_rev)  AS conv_ot_ct_rev,
SUM(conv_ot_vt_rev)  AS conv_ot_vt_rev,





CAST(SUM(vcreativeviews) AS bigint) AS VCreativeViews, 

CAST(SUM(vstart) AS bigint) AS VStart, 
CAST(SUM(vfirstquartile) AS bigint) AS VFirstQuartile, 
CAST(SUM(vmidpoint) AS bigint) AS VMidPoint, 

CAST(SUM(vthirdquartile) AS bigint) AS VThirdQuartile, 
CAST(SUM(vcomplete) AS bigint) AS VComplete, 
CAST(SUM(custom_events) AS bigint) AS CustomEvents, 
CAST(SUM(pageviews) AS bigint) AS PageViews, 

SUM(NetCost)  as NetCost,
SUM(AdjustedNetCost)  as AdjustedNetCost,
SUM(GrossCost)  as GrossCost,
SUM(platformfee)  as platformfee,
SUM(DataFee)  as DataFee,
SUM(thirdpartyfee)  as thirdpartyfee,
SUM(AgencyRevenue)  as AgencyRevenue,
SUM(FSA.BillableCost)  as BillableCost











FROM {3} FSA 
{1}
{2} {4};",
                           columnNameSQL, innerJoin, dummyWhere,args.reportDataTableName,groupBY);
                        break;
                    default:
                        break;
                }

                str = string.Format("{0} {1}", insertStatement, selectCommand);
            }

            return str;
        }
        

        private static string GenerateTempTableCountCommand(string selectCommand)
        {
            // return string.Format(@"update report_data set TotalCount=(SELECT count(1) FROM  {0});", selectCommand);

            return string.Format(@"", selectCommand);

        }

        private static string GenerateSelectCommand(string dates, SummaryBy summaryBy, ReportGeneratorArgs args, IList<int> TimeId=null)
        {
            string HavingGroupn = "  Having ";

            bool isAcumlated = false;
            string DateFilter = "DateID";
            const string orderBy = " ";
            string TimeFilter = string.Empty;
            string TimeStringFormat = "hourid";
            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Format(" group by DateID");
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DateID";
            var includeTime = false;
            var idColumn = string.Empty;
            var duplicateKey = string.Empty;
            var selectSql = string.Empty;
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                // groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, string.IsNullOrWhiteSpace(args.Fieldname) ? String.Empty : args.Fieldname);
                selectSql = idColumn = string.Format(",{0}", args.DataFieldId);
            }
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId, string.Format(args.SelectDataIds, args.Criteria.ItemsList));
            }
            if (string.IsNullOrWhiteSpace(args.Fieldname))
            {
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {
                 
                    groupByFields = string.Format(",{0}", args.DataFieldId);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                }
            }
            else
            {
                if (args.SubType == SubType.None || args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                    groupByFields = string.Format(",{0}{1}", args.DataFieldId, args.Fieldname);
                else
                    groupByFields = string.Format("{1}", args.DataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",COALESCE({0},0) as {1} {2}", args.RealFieldname.Trim(','), args.Fieldname.Trim(','), idColumn);

                if (!string.IsNullOrWhiteSpace(args.SecondFieldname))
                {

                    if (args.SecondSubType != SubType.None )
                        groupByFields = string.Format("{0}{1}", groupByFields, args.SecondFieldname);
 
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                    selectSql =  string.Format(",COALESCE({0},0) as {0} ", args.SecondFieldname.Trim(','))+ selectSql;

                }
            }

            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.RealFieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }
            if ((!string.IsNullOrWhiteSpace(args.SecondFieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.SecondAdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.SecondFieldname.Trim(','), args.Criteria.SecondAdvancedCriteria)+filterBySql;
            }

            switch (summaryBy)
            {
                case SummaryBy.Hour:
                    {
                        //DateFilter = "newDateID";
                        dateFieldName = "dateid as newDateID";
                        DateFilter = "dateid ";
                        FactStatTable = args.FactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "dateid as newDateID";
                                    DateFilter = "dateid ";
                                    includeTime = true;
                                    groupBy = string.Format(" group by DateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                                    groupBy = groupBy.Replace("DateID,", string.Empty);
                                    if (groupBy.IndexOf("group by DateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                                    }
                                    isAcumlated = true;
                                    dateFieldName = "1";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Day:
                    {
                        //DateFilter = "newDateID";
                        dateFieldName = "dateid as newDateID";
                        DateFilter = "dateid ";
                        FactStatTable = args.DayFactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "dateid as newDateID";
                                    DateFilter = "dateid ";
                                    includeTime = true;
                                    groupBy = string.Format(" group by DateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                   // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                                    groupBy=groupBy.Replace("DateID,", string.Empty);
                                    if (groupBy.IndexOf("group by DateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                                    }
                                    isAcumlated = true;
                                    dateFieldName = "1";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                   // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        DateFilter = "weekid";
                        //dateFieldName = "to_number(to_char(todate( weekid || ' ' || monthid, 'WW YYYYMM'), 'YYYYMMDD'), '99999999')   as DateID";
                        dateFieldName = "weekid as DateID";
                        includeTime = false;
                        groupBy = string.Format(" group by weekid{0}", groupByFields);

                        if(TimeId!=null)
                        TimeFilter = string.Format("and TimeId in ({0})", string.Join(", ", TimeId));

                        TimeStringFormat = "weekid";
                        FactStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                    {
                        dateFieldName = "2";
                       // dateFieldName = "monthid  as DateID";
                        DateFilter = "monthid";
                        FactStatTable = args.MonthFactStatTable;
                        groupBy = groupBy.Replace("DateID,", string.Empty);
                        if(groupBy.IndexOf("group by DateID")>0)
                        {
                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                        }
                        isAcumlated = true;
                        break;
                    }
                case SummaryBy.Month:
                    {
                        DateFilter = "monthid";
                        dateFieldName = "monthid  as DateID";

                      //  groupBy = string.Format(" group by monthid");
                        groupBy = string.Format(" group by monthid{0}", groupByFields);
                        FactStatTable = args.MonthFactStatTable;
                        break;
                    }
            }


            var populateReportSQL = string.Empty;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        switch (args.EntityType)
                        {

                            case EntityType.Deal:

                                {


                                    populateReportSQL = string.Format(@"insert into {4} (requests_dcr,DisplayedImpressions,unfilledrequests,WonImpressions,AvailableImpressions{0}{1},DateID{2}) 
                                                            select sum({5}) as requests_dcr,   sum(impressions) as DisplayedImpressions, sum(unfilledrequests_d) as unfilledrequests, sum(wins) as WonImpressions, sum(requests_d) as AvailableImpressions{3}",
                                                                                                                                                                                   args.SecondFieldname+ args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);


                                    if (isAcumlated)
                                    {

                                        populateReportSQL = string.Format(@"insert into {4} (requests_dcr, DisplayedImpressions,unfilledrequests,WonImpressions,AvailableImpressions{0}{1}{2}) 
                                                            select sum({5}) as requests_dcr,  sum(impressions) as DisplayedImpressions, sum(unfilledrequests_d) as unfilledrequests, sum(wins) as WonImpressions, sum(requests_d) as AvailableImpressions{3}",
                                                                                                                                                                                      args.Fieldname + args.SecondFieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName ,args.FilledRequestsId);


                                    }


                                    duplicateKey = "";
                                 
                                    if (string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
                                    {
                                        filterBySql += string.Format("     AND EXISTS(SELECT 1 FROM dim_buyer_deals buyDeal WHERE buyDeal.id = dealid and buyDeal.accountid ={0})", args.AccountId);
                                    }
                                   // filterBySql += string.Format(" And   campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    HavingGroupn = HavingGroupn + " (sum(impressions)>0  OR sum(unfilledrequests_d)>0  OR sum(wins)>0  OR sum(requests_d)>0 )";
                                    break;


                                }

                            case EntityType.Audiances:

                                {


                                    populateReportSQL = string.Format(@"insert into {4} (Impressions,avrcost,grossrevenue ,discount,billedsegment ,AccountId,AdvertiserId ,Campaignid ,UsedSegmentsId,Revenue {0}{1},DateID{2}) 
                                                            select sum(Impressions) as Impressions,sum({5}.avrcost ) as avrcost ,sum({5}.grossrevenue ) as grossrevenue  ,sum({5}.discount) as discount , billedsegment, camp.accountid, {5}.AdvertiserId as FSAAdvertiserId, Campaignid, UsedSegmentsId, sum(Revenue) as Revenue{3}",
                                                                                                                                                                                   args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, FactStatTable);


                                    if (isAcumlated)
                                    {

                                        populateReportSQL = string.Format(@"insert into {4} (Impressions,avrcost,grossrevenue , discount,billedsegment,AdvertiserId,Campaignid,UsedSegmentsId,Revenue{0}{1}{2}) 
                                                            select sum(Impressions) as Impressions,  sum({5}.avrcost ) as avrcost  ,sum({5}.grossrevenue ) as grossrevenue  ,sum({5}.discount) as discount ,billedsegment, camp.accountid, {5}.AdvertiserId  as FSAAdvertiserId, Campaignid,  UsedSegmentsId, sum(Revenue) as Revenue{3}",
                                                                                                                                                                                      args.Fieldname + args.SecondFieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, FactStatTable);


                                    }


                                    duplicateKey = "";
                                    // filterBySql += string.Format(" And   campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    HavingGroupn = HavingGroupn + " (sum(impressions)>0  )";
                                    FactStatTable = FactStatTable + " inner join dim_campaigns camp on campaignid=camp.id ";
                                    break;


                                }
                            case EntityType.API:
                            case EntityType.App:
                                {

                                
                                    populateReportSQL = string.Format(@"insert into {4} (Requests,Impressions,Revenue,Clicks{0}{1},DateID{2}) 
                                                            select  sum(Requests) as Requests, sum(Impressions) as Impressions, sum(NetCost) as Revenue, sum(Clicks) as Clicks{3}",
                                                                                                                                                                                  args.Fieldname, idColumn, includeTime ? string.Format(",TimeId ", TimeStringFormat) : "", selectSql, args.reportDataTableName);


                                    if (isAcumlated)
                                    {

                                        populateReportSQL = string.Format(@"insert into {4} (Requests,Impressions,Revenue,Clicks{0}{1}{2}) 
                                                            select  sum(Requests) as Requests, sum(Impressions) as Impressions, sum(NetCost) as Revenue, sum(Clicks) as Clicks{3}",
                                                                                                                                                                                      args.Fieldname, idColumn, includeTime ? string.Format(",TimeId ", TimeStringFormat) : "", selectSql, args.reportDataTableName);


                                    }


                                    duplicateKey = "";
                                    filterBySql += string.Format(" And   campaigntype!={0}  ", (int)args.Criteria.NotInCampaignType);
                                    HavingGroupn = HavingGroupn + " (sum(Requests)>0  OR sum(Clicks)>0  OR sum(Impressions)>0  OR sum(NetCost)>0 )";

                                  
                                    break;

                                    
                                }

                            case EntityType.AudianceSegmentsForAdvertiser:


                                populateReportSQL = string.Format(@"insert into {4} (Impressions,



  conv_pr,
 conv_pr_ct,
 conv_pr_vt,
conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
conv_ot_ct_rev,
 conv_ot_vt_rev,


vcreativeviews,
vstart,
vfirstquartile,
vmidpoint,
vthirdquartile,
vcomplete,
custom_events,pageviews,DataFee,Clicks{0}{1},DateID{2}) 
                                                            select   sum(Impressions) as Impressions,


0  AS conv_pr,
0  AS conv_pr_ct,
0  AS conv_pr_vt,
0 AS conv_ot,
0  AS conv_ot_ct,
0  AS conv_ot_vt,
0 AS conv_pr_rev,
0  AS conv_pr_ct_rev,
0 AS conv_pr_vt_rev,
0 AS conv_ot_rev,
0 AS conv_ot_ct_rev,
0 AS conv_ot_vt_rev,

sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events,sum(pageviews) as pageviews, sum(DataFee) as DataFee,  sum(Clicks) as Clicks {3}",
                               args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);
                                if (isAcumlated)
                                {
                                    populateReportSQL = string.Format(@"insert into {4} (Impressions,


  conv_pr,
 conv_pr_ct,
 conv_pr_vt,
conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
conv_ot_ct_rev,
 conv_ot_vt_rev,

vcreativeviews,
vstart,
vfirstquartile,
vmidpoint,
vthirdquartile,
vcomplete,custom_events,pageviews,
NetCost,
 AdjustedNetCost,
 GrossCost,
 platformfee,
 DataFee,
 thirdpartyfee,
 AgencyRevenue,
 BillableCost

,Clicks{0}{1}{2}) 
                                                            select   sum(Impressions) as Impressions,

0  AS conv_pr,
0  AS conv_pr_ct,
0  AS conv_pr_vt,
0 AS conv_ot,
0  AS conv_ot_ct,
0  AS conv_ot_vt,
0 AS conv_pr_rev,
0  AS conv_pr_ct_rev,
0 AS conv_pr_vt_rev,
0 AS conv_ot_rev,
0 AS conv_ot_ct_rev,
0 AS conv_ot_vt_rev,
sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events, sum(pageviews) as pageviews,sum(DataFee) as DataFee,   sum(Clicks) as Clicks {3}",
                                          args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                }
                                HavingGroupn = HavingGroupn + " (sum(Impressions)>0 Or sum(Clicks)>0  )";
                                //filterBySql += string.Format(" And campaignid>0  ");
                                duplicateKey = "";
                                //FactStatTable = FactStatTable + " inner join usedsegmentsgroups_segments on usedsegmentsid=usedsegmentsgroupid ";
                                break;
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {

                                    populateReportSQL = string.Format(@"insert into {4} (Impressions,RequestsByType,WonImpressions,


  conv_pr,
 conv_pr_ct,
 conv_pr_vt,
conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
conv_ot_ct_rev,
 conv_ot_vt_rev,

vcreativeviews,
vstart,
vfirstquartile,
vmidpoint,
vthirdquartile,
vcomplete,
custom_events,pageviews,



NetCost,
AdjustedNetCost,
GrossCost,
platformfee,
DataFee,
thirdpartyfee,
AgencyRevenue,
BillableCost


,Clicks{0}{1},DateID{2}) 
                                                            select   sum(Impressions) as Impressions,Sum({5}) as RequestsByType , Sum(Wins) as WonImpressions ,

 SUM(conv_pr)  AS conv_pr,
SUM(conv_pr_ct)  AS conv_pr_ct,
SUM(conv_pr_vt)  AS conv_pr_vt,
SUM(conv_ot) AS conv_ot,
SUM(conv_ot_ct)  AS conv_ot_ct,
SUM(conv_ot_vt)  AS conv_ot_vt,
SUM(conv_pr_rev) AS conv_pr_rev,
SUM(conv_pr_ct_rev)  AS conv_pr_ct_rev,
SUM(conv_pr_vt_rev) AS conv_pr_vt_rev,
SUM(conv_ot_rev) AS conv_ot_rev,
SUM(conv_ot_ct_rev) AS conv_ot_ct_rev,
SUM(conv_ot_vt_rev) AS conv_ot_vt_rev,
sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,

sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events, sum(pageviews) as pageviews,
SUM(NetCost)  as NetCost,
SUM(AdjustedNetCost)  as AdjustedNetCost,
SUM(GrossCost)  as GrossCost,
SUM(platformfee)  as platformfee,
SUM(DataFee)  as DataFee,
SUM(thirdpartyfee)  as thirdpartyfee,
SUM(AgencyRevenue)  as AgencyRevenue,
SUM(BillableCost)  as BillableCost

, sum(Clicks) as Clicks {3}",
                                   args.SecondFieldname + args.Fieldname,  idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);
                                    if (isAcumlated)
                                    {
                                        populateReportSQL = string.Format(@"insert into {4} (Impressions,RequestsByType,WonImpressions,

  conv_pr,
 conv_pr_ct,
 conv_pr_vt,
conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
conv_ot_ct_rev,
 conv_ot_vt_rev,

vcreativeviews,
vstart,
vfirstquartile,
vmidpoint,
vthirdquartile,
vcomplete,custom_events,pageviews,
 NetCost,
AdjustedNetCost,
GrossCost,
platformfee,
DataFee,
thirdpartyfee,
AgencyRevenue,
BillableCost

,Clicks{0}{1}{2}) 
                                                            select   sum(Impressions) as Impressions, Sum({5}) as RequestsByType , Sum(Wins) as WonImpressions ,



 SUM(conv_pr)  AS conv_pr,
SUM(conv_pr_ct)  AS conv_pr_ct,
SUM(conv_pr_vt)  AS conv_pr_vt,
SUM(conv_ot) AS conv_ot,
SUM(conv_ot_ct)  AS conv_ot_ct,
SUM(conv_ot_vt)  AS conv_ot_vt,
SUM(conv_pr_rev) AS conv_pr_rev,
SUM(conv_pr_ct_rev)  AS conv_pr_ct_rev,
SUM(conv_pr_vt_rev) AS conv_pr_vt_rev,
SUM(conv_ot_rev) AS conv_ot_rev,
SUM(conv_ot_ct_rev) AS conv_ot_ct_rev,
SUM(conv_ot_vt_rev) AS conv_ot_vt_rev,
sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events,sum(pageviews) as pageviews, 
SUM(NetCost)  as NetCost,
SUM(AdjustedNetCost)  as AdjustedNetCost,
SUM(GrossCost)  as GrossCost,
SUM(platformfee)  as platformfee,
SUM(DataFee)  as DataFee,
SUM(thirdpartyfee)  as thirdpartyfee,
SUM(AgencyRevenue)  as AgencyRevenue,
SUM(BillableCost)  as BillableCost

, sum(Clicks) as Clicks {3}",
                                              args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    }
                                    HavingGroupn = HavingGroupn + " (sum(Impressions)>0 Or sum(Clicks)>0  OR sum(BillableCost)>0 )";
                                    //filterBySql += string.Format(" And campaignid>0  ");
                                    duplicateKey = "";
                                    break;
                                }
                        }
                        break;
                    }
                case ReportType.Chart:
                    {
                        throw new NotImplementedException();
                    }

            }
          var resultDtGeneration=  GenerateBetweenDates(dates, DateFilter, summaryBy);
            if (!(groupBy.IndexOf("group by") >=0))
            {
                HavingGroupn = string.Empty;
            }

            if ((groupBy.IndexOf("group by") >= 0) &&args.EntityType==EntityType.Audiances )
            {
                groupBy = groupBy + " ,camp.AccountId ,  FSAAdvertiserId , Campaignid, UsedSegmentsId,billedsegment";
            }
            var selectCommand = string.Empty;


            if (!string.IsNullOrEmpty(args.AccountIdFieldName))
            {
                selectCommand = string.Format("{0},{1}{2} from {3} where {12} {13} and {5} ={6} {14} {7} {8} {9} {10} {11};",
                                populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                appIDsSql, filterBySql, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);

            }
            else
            {
                selectCommand = string.Format("{0},{1}{2} from {3} where {12} {13} {14} {7} {8} {9} {10} {11};",
                                populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                appIDsSql, filterBySql, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);

            }
            if (isAcumlated)
            {
                if (!string.IsNullOrEmpty(args.AccountIdFieldName))
                {

                    selectCommand = string.Format("{0}{2} from {3} where {12}  {13} and {5} ={6} {14} {7} {8} {9} {10} {11};",
                                 populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                 appIDsSql, filterBySql, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);
                }
                else
                {


                    selectCommand = string.Format("{0}{2} from {3} where {12}  {13}  {14} {7} {8} {9} {10} {11};",
                                 populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                 appIDsSql, filterBySql, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);

                }
            }
            return selectCommand;
        }

        private static string GenerateSelectForUnionCommand(string dates, SummaryBy summaryBy, ReportGeneratorArgs args,  ref string groupByVar ,IList<int> TimeId = null)
        {
            string HavingGroupn = "  Having ";
            string DateFilter = "DateID";
            const string orderBy = " ";
            string TimeFilter = string.Empty;
            string TimeStringFormat = "hourid";
            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Format(" group by DateID");
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DateID";
            var includeTime = false;
            var idColumn = string.Empty;
            var duplicateKey = string.Empty;
            var selectSql = string.Empty;
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                // groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, string.IsNullOrWhiteSpace(args.Fieldname) ? String.Empty : args.Fieldname);
                selectSql = idColumn = string.Format(",{0}", args.DataFieldId);
            }
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId, string.Format(args.SelectDataIds, args.Criteria.ItemsList));
            }
            if (string.IsNullOrWhiteSpace(args.Fieldname))
            {
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {

                    groupByFields = string.Format(",{0}", args.DataFieldId);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                }
            }
            else
            {
                if (args.SubType == SubType.None || args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                    groupByFields = string.Format(",{0}{1}", args.DataFieldId, args.Fieldname);
                else
                    groupByFields = string.Format("{1}", args.DataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",COALESCE({0},0) as {1} {2}", args.RealFieldname.Trim(','),  args.Fieldname.Trim(','), idColumn);

                if (!string.IsNullOrEmpty(args.SecondFieldname))
                {
                    groupByFields = string.Format("{0}{1}", groupByFields, args.SecondFieldname);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                    selectSql = string.Format(",COALESCE({0},0) as {0} ", args.SecondFieldname.Trim(','))+ selectSql;

                }
            }

            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.RealFieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }
            if ((!string.IsNullOrWhiteSpace(args.SecondFieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.SecondAdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.SecondFieldname.Trim(','), args.Criteria.SecondAdvancedCriteria)+filterBySql                ;
            }
            switch (summaryBy)
            {

                case SummaryBy.Day:
                    {
                        //DateFilter = "newDateID";
                        dateFieldName = "dateid as newDateID";
                        DateFilter = "dateid ";
                        FactStatTable = args.DayFactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "dateid as newDateID";
                                    DateFilter = "dateid ";
                                    includeTime = true;
                                    groupBy = string.Format(" group by DateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                  //  groupBy = groupBy.Replace("DateID,", string.Empty);
                                    if (groupBy.IndexOf("group by DateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                                    }
                                    dateFieldName = "1 as newDateID";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        DateFilter = "weekid ";
                        //dateFieldName = "to_number(to_char(todate( weekid || ' ' || monthid, 'WW YYYYMM'), 'YYYYMMDD'), '99999999')   as DateID";
                        dateFieldName = "weekid as newDateID";
                        includeTime = false;
                        groupBy = string.Format(" group by weekid{0}", groupByFields);

                        if (TimeId != null)
                            TimeFilter = string.Format("and TimeId in ({0})", string.Join(", ", TimeId));

                        TimeStringFormat = "weekid";
                        FactStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                    {
                        dateFieldName = "1 as newDateID";
                        DateFilter = "monthid ";
                        FactStatTable = args.MonthFactStatTable;
                        groupBy = groupBy.Replace("DateID", "newDateID");
                        if (groupBy.IndexOf("group by DateID") > 0)
                        {
                            groupBy = groupBy.Replace("group by DateID", string.Empty);
                        }


                        break;
                    }
                case SummaryBy.Month:
                    {
                        DateFilter = "monthid ";
                        dateFieldName = "monthid  as newDateID";

                        //  groupBy = string.Format(" group by monthid");
                        groupBy = string.Format(" group by monthid{0}", groupByFields);
                        FactStatTable = args.MonthFactStatTable;
                        break;
                    }
            }


            var populateReportSQL = string.Empty;
            var resultDtGeneration = GenerateBetweenDates(dates, DateFilter, summaryBy);
            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        switch (args.EntityType)
                        {
                            case EntityType.Deal:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select sum({5}) as requests_dcr,   sum(requests_d) as AvailableImpressions, sum(unfilledrequests_d) as unfilledrequests, sum(impressions) as DisplayedImpressions, sum(wins) as WonImpressions{3}",
                                                                                                                                                                                 args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);
                                    duplicateKey = "";
                                   // filterBySql += string.Format(" And   campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    HavingGroupn = HavingGroupn + "( sum(Requests_d)>0 OR sum(unfilledrequests_d)>0 OR sum(impressions)>0 OR sum(wins)>0 )";
                                    if (string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
                                       
                                    {
                                        filterBySql += string.Format("     AND EXISTS(SELECT 1 FROM dim_buyer_deals buyDeal WHERE buyDeal.id = dealid and buyDeal.accountid ={0})", args.AccountId);

                                    }
                                    break;


                                }

                            case EntityType.Audiances:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select sum(Impressions) as Impressions, sum(avrcost   ) as avrcost   , sum(grossrevenue  ) as grossrevenue  ,sum(discount ) as discount , billedsegment ,AccountId ,AdvertiserId , Campaignid, UsedSegmentsId, sum(Revenue) as Revenue{3}",
                                                                                                                                                                                 args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    // filterBySql += string.Format(" And   campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    HavingGroupn = HavingGroupn + "( sum(Impressions)>0 )";
                                    FactStatTable = FactStatTable + " inner join dim_campaigns camp on campaignid=camp.id ";
                                    break;


                                }
                            case EntityType.API:
                            case EntityType.App:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select  sum(Requests) as Requests, sum(Impressions) as Impressions, sum(NetCost) as Revenue, sum(Clicks) as Clicks{3}",
                                                                                                                                                                                  args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    filterBySql += string.Format(" And   campaigntype!={0}  ", (int)args.Criteria.NotInCampaignType);
                                    HavingGroupn = HavingGroupn + "( sum(Requests)>0 OR sum(Impressions)>0 OR sum(Clicks)>0 OR sum(NetCost)>0 )";
                                    break;


                                }

                            case EntityType.AudianceSegmentsForAdvertiser:
                                {
                                    populateReportSQL = string.Format(@" 
                                                            select   sum(Impressions) as Impressions,

 0  AS conv_pr,
0  AS conv_pr_ct,
0  AS conv_pr_vt,
0 AS conv_ot,
0  AS conv_ot_ct,
0  AS conv_ot_vt,
0 AS conv_pr_rev,
0  AS conv_pr_ct_rev,
0 AS conv_pr_vt_rev,
0 AS conv_ot_rev,
0 AS conv_ot_ct_rev,
0 AS conv_ot_vt_rev,

sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events,sum(pageviews) as pageviews, sum(DataFee) as DataFee,  sum(Clicks) as Clicks {3}",
           args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);

                                    //filterBySql += string.Format(" And campaignid>0  ");
                                    HavingGroupn = HavingGroupn + " (sum(Impressions)>0 OR sum(Clicks)>0 )";
                                    duplicateKey = "";
                                    //FactStatTable = FactStatTable + " inner join usedsegmentsgroups_segments on usedsegmentsid=usedsegmentsgroupid ";
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@" 
                                                            select   sum(Impressions) as Impressions,sum({5})  as RequestsByType,sum(wins) as WonImpressions ,



 SUM(conv_pr)  AS conv_pr,
SUM(conv_pr_ct)  AS conv_pr_ct,
SUM(conv_pr_vt)  AS conv_pr_vt,
SUM(conv_ot) AS conv_ot,
SUM(conv_ot_ct)  AS conv_ot_ct,
SUM(conv_ot_vt)  AS conv_ot_vt,
SUM(conv_pr_rev) AS conv_pr_rev,
SUM(conv_pr_ct_rev)  AS conv_pr_ct_rev,
SUM(conv_pr_vt_rev) AS conv_pr_vt_rev,
SUM(conv_ot_rev) AS conv_ot_rev,
SUM(conv_ot_ct_rev) AS conv_ot_ct_rev,
SUM(conv_ot_vt_rev) AS conv_ot_vt_rev,
sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(coalesce((custom_events->'instll')::bigint, 0) ) as custom_events,sum(pageviews) as pageviews,
SUM(NetCost)  as NetCost,
SUM(AdjustedNetCost)  as AdjustedNetCost,
SUM(GrossCost)  as GrossCost,
SUM(platformfee)  as platformfee,
SUM(DataFee)  as DataFee,
SUM(thirdpartyfee)  as thirdpartyfee,
SUM(AgencyRevenue)  as AgencyRevenue,
SUM(BillableCost)  as BillableCost

, sum(Clicks) as Clicks {3}",
                                            args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName,args.FilledRequestsId);

                                    //filterBySql += string.Format(" And campaignid>0  ");
                                    HavingGroupn = HavingGroupn + " (sum(Impressions)>0 OR sum(Clicks)>0 OR sum(BillableCost)>0 )";
                         duplicateKey = "";
                                    break;
                                }
                        }
                        break;
                    }
                case ReportType.Chart:
                    {
                        throw new NotImplementedException();
                    }

            }
            if (!(groupBy.IndexOf("group by") >= 0))
            {
                HavingGroupn = string.Empty;
            }
            if ((groupBy.IndexOf("group by") >= 0) && args.EntityType == EntityType.Audiances)
            {
                groupBy = groupBy + ",camp.AccountId , FSAAdvertiserId , Campaignid, UsedSegmentsId,billedsegment";
            }
            var selectCommand = string.Empty;
            if (!string.IsNullOrEmpty(args.AccountIdFieldName))
            {
                 selectCommand = string.Format("{0},{1}{2} from {3} where {12}  {13} and {5} ={6} {14} {7} {8} {9} {10} {11}",
                                  populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                  appIDsSql, filterBySql, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);

            }
            else
            {
                 selectCommand = string.Format("{0},{1}{2} from {3} where {12}  {13} {14}  {7} {8} {9} {10} {11}",
                                 populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                                 appIDsSql,  args.TrafficPlannerCrt==null  ? filterBySql: args.DrillDownDataFilter, groupBy + HavingGroupn, orderBy, duplicateKey, resultDtGeneration, TimeFilter, args.AdvertiserIdEqualFormat);


            }

            groupByVar =groupBy +";"  ;
            groupByVar = groupByVar.Replace("newDateID", "DateID");
            groupByVar = groupByVar.Replace("monthid", "DateID");
            groupByVar = groupByVar.Replace("weekid", "DateID");
            return selectCommand;
        }

        private static string GenerateFinalSelectForUnionCommand(SummaryBy summaryBy, ReportGeneratorArgs args,  IList<int> TimeId = null)
        {
            string DateFilter = "DateID";
            const string orderBy = " ";
            string TimeFilter = string.Empty;
            string TimeStringFormat = "hourid";
            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Format(" group by DateID");
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DateID";
            var includeTime = false;
            var idColumn = string.Empty;
            var duplicateKey = string.Empty;
            var selectSql = string.Empty;
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                // groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, string.IsNullOrWhiteSpace(args.Fieldname) ? String.Empty : args.Fieldname);
                selectSql = idColumn = string.Format(",{0}", args.DataFieldId);
            }
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId, string.Format(args.SelectDataIds, args.Criteria.ItemsList));
            }
            if (string.IsNullOrWhiteSpace(args.Fieldname))
            {
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {

                    groupByFields = string.Format(",{0}", args.DataFieldId);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                }
            }
            else
            {
                if (args.SubType == SubType.None || args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                    groupByFields = string.Format(",{0}{1}", args.DataFieldId, args.Fieldname);
                else
                    groupByFields = string.Format(",{1}", args.DataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",COALESCE({0},0) as {1} {2}", args.RealFieldname.Trim(','),  args.Fieldname.Trim(','), idColumn);


                if (!string.IsNullOrEmpty(args.SecondFieldname))
                {
                    groupByFields = string.Format("{0}{1}", groupByFields, args.SecondFieldname);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                    selectSql =  string.Format(",COALESCE({0},0) as {0} ", args.SecondFieldname.Trim(',')) + selectSql;

                }
            }

            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.RealFieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }
            if ((!string.IsNullOrWhiteSpace(args.SecondFieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.SecondAdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.SecondFieldname.Trim(','), args.Criteria.SecondAdvancedCriteria)+ filterBySql;
            }
            switch (summaryBy)
            {

                case SummaryBy.Day:
                    {
                        //DateFilter = "newDateID";
                        dateFieldName = "dateid as newDateID";
                        DateFilter = "dateid ";
                        FactStatTable = args.DayFactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "dateid as newDateID";
                                    DateFilter = "dateid ";
                                    includeTime = true;
                                    groupBy = string.Format(" group by DateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                                    groupBy = groupBy.Replace("DateID,", string.Empty);
                                    if (groupBy.IndexOf("group by DateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                                    }
                                    dateFieldName = "1 as newDateID";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "to_number(substring(dateid||'' from 1 for 6)  || '01',  '99999999'  ) as newDateID";
                                    DateFilter = "dateid ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        DateFilter = "weekid";
                        //dateFieldName = "to_number(to_char(todate( weekid || ' ' || monthid, 'WW YYYYMM'), 'YYYYMMDD'), '99999999')   as DateID";
                        dateFieldName = "weekid as newDateID";
                        includeTime = false;
                        groupBy = string.Format(" group by weekid{0}", groupByFields);

                        if (TimeId != null)
                            TimeFilter = string.Format("and TimeId in ({0})", string.Join(", ", TimeId));

                        TimeStringFormat = "weekid";
                        FactStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                    {
                        dateFieldName = "1 as newDateID";
                        DateFilter = "monthid";
                        FactStatTable = args.MonthFactStatTable;
                        groupBy = groupBy.Replace("DateID,", string.Empty);
                        if (groupBy.IndexOf("group by DateID") > 0)
                        {
                            groupBy = groupBy.Replace("group by DateID", string.Empty);
                        }

                        break;
                    }
                case SummaryBy.Month:
                    {
                        DateFilter = "monthid";
                        dateFieldName = "monthid  as newDateID";

                        //  groupBy = string.Format(" group by monthid");
                        groupBy = string.Format(" group by monthid{0}", groupByFields);
                        FactStatTable = args.MonthFactStatTable;
                        break;
                    }
            }

            if ((groupBy.IndexOf("group by") >= 0) && args.EntityType == EntityType.Audiances)
            {
                groupBy = groupBy + ",camp.AccountId , FSAAdvertiserId , Campaignid, UsedSegmentsId,billedsegment";
            }
            var populateReportSQL = string.Empty;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        switch (args.EntityType)
                        {
                            case EntityType.Deal:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select sum({5}) as requests_dcr,  sum(Requests_d) as AvailableImpressions, sum(unfilledrequests_d) as unfilledrequests, sum(impressions) as DisplayedImpressions, sum(wins) as WonImpressions{3}",
                                                                                                                                                                                args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName,args.FilledRequestsId);
                                    duplicateKey = "";
                                   // filterBySql += string.Format(" And     campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    break;


                                }
                            case EntityType.Audiances:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select sum(Impressions) as Impressions,  billedsegment,AccountId ,AdvertiserId , Campaignid,  UsedSegmentsId, sum(Revenue) as Revenue{3}",
                                                                                                                                                                                args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    // filterBySql += string.Format(" And     campaigntype={0}  ", (int)args.Criteria.CampaignType);
                                    break;


                                }
                            case EntityType.API:
                            case EntityType.App:
                                {


                                    populateReportSQL = string.Format(@" 
                                                            select  sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Revenue) as Revenue, sum(Clicks) as Clicks{3}",
                                                                                                                                                                                  args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    filterBySql += string.Format(" And     campaigntype!={0}  ", (int)args.Criteria.NotInCampaignType);
                                    break;


                                }

                            case EntityType.AudianceSegmentsForAdvertiser:

                                {
                                    populateReportSQL = string.Format(@" 
                                                            select   sum(Impressions) as Impressions, 


0  AS conv_pr,
0  AS conv_pr_ct,
0  AS conv_pr_vt,
0 AS conv_ot,
0  AS conv_ot_ct,
0  AS conv_ot_vt,
0 AS conv_pr_rev,
0  AS conv_pr_ct_rev,
0 AS conv_pr_vt_rev,
0 AS conv_ot_rev,
0 AS conv_ot_ct_rev,
0 AS conv_ot_vt_rev,

sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(custom_events) as custom_events,sum(pageviews) as pageviews, sum(DataFee) as DataFee , sum(Clicks) as Clicks {3}",
                                            args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);

                                    //filterBySql += string.Format(" And campaignid>0  ");

                                    duplicateKey = "";
                                    break;
                                }

                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@" 
                                                            select   sum(Impressions) as Impressions,sum(RequestsByType) as RequestsByType, sum(WonImpressions) as WonImpressions ,




 SUM(conv_pr)  AS conv_pr,
SUM(conv_pr_ct)  AS conv_pr_ct,
SUM(conv_pr_vt)  AS conv_pr_vt,
SUM(conv_ot) AS conv_ot,
SUM(conv_ot_ct)  AS conv_ot_ct,
SUM(conv_ot_vt)  AS conv_ot_vt,
SUM(conv_pr_rev) AS conv_pr_rev,
SUM(conv_pr_ct_rev)  AS conv_pr_ct_rev,
SUM(conv_pr_vt_rev) AS conv_pr_vt_rev,
SUM(conv_ot_rev) AS conv_ot_rev,
SUM(conv_ot_ct_rev) AS conv_ot_ct_rev,
SUM(conv_ot_vt_rev) AS conv_ot_vt_rev,
sum(vcreativeviews) as vcreativeviews, 
sum(vstart) as vstart,
sum(vfirstquartile) as vfirstquartile,
sum(vmidpoint) as vmidpoint,
sum(vthirdquartile) as vthirdquartile,
sum(vcomplete) as vcomplete,
sum(custom_events) as custom_events, sum(pageviews) as pageviews,
SUM(NetCost)  as NetCost,
SUM(AdjustedNetCost)  as AdjustedNetCost,
SUM(GrossCost)  as GrossCost,
SUM(platformfee)  as platformfee,
SUM(DataFee)  as DataFee,
SUM(thirdpartyfee)  as thirdpartyfee,
SUM(AgencyRevenue)  as AgencyRevenue,
SUM(BillableCost)  as BillableCost

, sum(Clicks) as Clicks {3}",
                                            args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName, args.FilledRequestsId);

                                    //filterBySql += string.Format(" And campaignid>0  ");

                                    duplicateKey = "";
                                    break;
                                }
                        }
                        break;
                    }
                case ReportType.Chart:
                    {
                        throw new NotImplementedException();
                    }

            }
            var selectCommand = string.Format("{0},{1}{2} ",
                              populateReportSQL, "newDateID as DateID", includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "");



  
            return selectCommand;
        }
        private static string GenerateInsertSelectCommand( SummaryBy summaryBy, ReportGeneratorArgs args)
        {
            string DateFilter = "DateID";
            const string orderBy = " ";
            string TimeFilter = string.Empty;
            string TimeStringFormat = "hourid";
            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Format(" group by DateID");
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DateID";
            var includeTime = false;
            var idColumn = string.Empty;
            var duplicateKey = string.Empty;
            var selectSql = string.Empty;
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                // groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, string.IsNullOrWhiteSpace(args.Fieldname) ? String.Empty : args.Fieldname);
                selectSql = idColumn = string.Format(",{0}", args.DataFieldId);
            }
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId, string.Format(args.SelectDataIds, args.Criteria.ItemsList));
            }
            if (string.IsNullOrWhiteSpace(args.Fieldname))
            {
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {

                    groupByFields = string.Format(",{0}", args.DataFieldId);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                }
            }
            else
            {
                if (args.SubType == SubType.None || args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                    groupByFields = string.Format(",{0}{1}", args.DataFieldId, args.Fieldname);
                else
                    groupByFields = string.Format(",{1}", args.DataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",COALESCE({0},0) as {1} {2}", args.RealFieldname.Trim(','),  args.Fieldname.Trim(','), idColumn);

                if (!string.IsNullOrEmpty(args.SecondFieldname))
                {
                    groupByFields = string.Format("{0}{1}", groupByFields, args.SecondFieldname);
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                    selectSql =  string.Format(",COALESCE({0},0) as {0} ", args.SecondFieldname.Trim(',')) +selectSql;

                }
            }

            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.RealFieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }
            if ((!string.IsNullOrWhiteSpace(args.SecondFieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.SecondAdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.SecondFieldname.Trim(','), args.Criteria.SecondAdvancedCriteria)+ filterBySql;
            }

            switch (summaryBy)
            {

                case SummaryBy.Day:
                    {
                        //DateFilter = "newDateID";
                        dateFieldName = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') as newDateID";
                        DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
                        FactStatTable = args.DayFactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') as newDateID";
                                    DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
                                    includeTime = true;
                                    groupBy = string.Format(" group by DateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') )) as newDateID";
                                    DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') as newDateID";
                                    DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
                                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                                    groupBy = groupBy.Replace("DateID,", string.Empty);
                                    if (groupBy.IndexOf("group by DateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                                    }
                                    dateFieldName = "1";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "to_number(to_char(date_trunc('month',dateid ), 'YYYYMMDD'), '99999999') as newDateID";
                                    DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    // groupBy = groupBy.Replace("DateID,", string.Empty);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        DateFilter = "monthid";
                        dateFieldName = "to_number(to_char(todate( weekid || ' ' || monthid, 'WW YYYYMM'), 'YYYYMMDD'), '99999999')   as DateID";
                        includeTime = false;
                        groupBy = string.Format(" group by monthid,TimeId{0}", groupByFields);

                   

                        TimeStringFormat = "weekid";
                        FactStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                    {
                        dateFieldName = "1";
                        DateFilter = "monthid";
                        FactStatTable = args.MonthFactStatTable;
                        groupBy = groupBy.Replace("DateID,", string.Empty);
                        if (groupBy.IndexOf("group by DateID") > 0)
                        {
                            groupBy = groupBy.Replace("group by DateID", string.Empty);
                        }

                        break;
                    }
                case SummaryBy.Month:
                    {
                        DateFilter = "monthid";
                        dateFieldName = "to_number(monthid ||'01', '99999999') as DateID";

                        //  groupBy = string.Format(" group by monthid");
                        groupBy = string.Format(" group by monthid{0}", groupByFields);
                        FactStatTable = args.MonthFactStatTable;
                        break;
                    }
            }

            if ((groupBy.IndexOf("group by") >= 0) && args.EntityType == EntityType.Audiances)
            {
                groupBy = groupBy + ",camp.AccountId , FSAAdvertiserId , Campaignid, UsedSegmentsId,billedsegment";
            }
            var populateReportSQL = string.Empty;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        switch (args.EntityType)
                        {

                            case EntityType.Deal:

                                {


                                    populateReportSQL = string.Format(@"insert into {4} (requests_dcr ,requests_d,unfilledrequests_d,impressions,wins{0}{1},DateID{2}) 
                                                             ",
                                                                                                                                                                                args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    //filterBySql += string.Format(" And     EXISTS(SELECT * FROM dim_campaigns  as te  WHERE te.typeid ={0} and te.id = campaignid )  ", (int)args.Criteria.CampaignType);
                                    break;


                                }

                            case EntityType.Audiances:

                                {


                                    populateReportSQL = string.Format(@"insert into {4} (Impressions, avrcost  ,grossrevenue ,discount,billedsegment,AccountId,AdvertiserId   ,Campaignid, UsedSegmentsId,Revenue{0}{1},DateID{2}) 
                                                             ",
                                                                                                                                                                                args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    //filterBySql += string.Format(" And     EXISTS(SELECT * FROM dim_campaigns  as te  WHERE te.typeid ={0} and te.id = campaignid )  ", (int)args.Criteria.CampaignType);
                                    break;


                                }
                            case EntityType.API:
                            case EntityType.App:
                                {


                                    populateReportSQL = string.Format(@"insert into {4} (Requests,Impressions,Revenue,Clicks{0}{1},DateID{2}) 
                                                             ",
                                                                                                                                                                                  args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    filterBySql += string.Format(" And     EXISTS(SELECT * FROM dim_campaigns  as te  WHERE te.typeid !={0} and te.id = campaignid )  ", (int)args.Criteria.NotInCampaignType);
                                    break;


                                }

                            case EntityType.AudianceSegmentsForAdvertiser:
                                {
                                    populateReportSQL = string.Format(@"insert into {4} (Impressions, 



 conv_pr,
 conv_pr_ct,
 conv_pr_vt,
 conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
 conv_ot_ct_rev,
 conv_ot_vt_rev,
vcreativeviews, 
 vstart,
 vfirstquartile,
 vmidpoint,
 vthirdquartile,
 vcomplete,
 custom_events,pageviews, DataFee,Clicks{0}{1},DateID{2}) 
                                                               ",
                                            args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@"insert into {4} (Impressions,RequestsByType,WonImpressions, 


conv_pr,
 conv_pr_ct,
 conv_pr_vt,
 conv_ot,
 conv_ot_ct,
 conv_ot_vt,
 conv_pr_rev,
 conv_pr_ct_rev,
 conv_pr_vt_rev,
 conv_ot_rev,
 conv_ot_ct_rev,
 conv_ot_vt_rev,
vcreativeviews, 
 vstart,
 vfirstquartile,
 vmidpoint,
 vthirdquartile,
 vcomplete,
 custom_events,pageviews,


NetCost,
AdjustedNetCost,
 GrossCost,
 platformfee,
DataFee,
 thirdpartyfee,
 AgencyRevenue,
 BillableCost



,Clicks{0}{1},DateID{2}) 
                                                               ",
                                            args.SecondFieldname + args.Fieldname, idColumn, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", selectSql, args.reportDataTableName);
                                    duplicateKey = "";
                                    break;
                                }
                        }
                        break;
                    }
                case ReportType.Chart:
                    {
                        throw new NotImplementedException();
                    }

            }
            //var selectCommand = string.Format("{0},{1}{2} from {3} where {12} in ({4}) {13} and {5} ={6} {7} {8} {9} {10} {11};",
            //                  populateReportSQL, dateFieldName, includeTime ? string.Format(",{0} as TimeId", TimeStringFormat) : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
            //                  appIDsSql, filterBySql, groupBy, orderBy, duplicateKey, DateFilter, TimeFilter);
            return populateReportSQL;


        }

        private static string GenerateChartSelectCommand(string dates, ReportGeneratorArgs args, SummaryBy summaryBy = SummaryBy.Day, IList<int> TimeId = null)
        {
            const string orderBy = " ";
            string TimeFilter = string.Empty;
            string TimeStringFormat = string.Empty;
            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Empty;
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DateID";
            var populateReportSQL = string.Empty;
            groupBy = string.Format(" group by Xaxis");
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId,
                                          string.Format(args.SelectDataIds, args.Criteria.ItemsList));
            }
            // DateFilter = "to_number(to_char(dateid, 'YYYYMMDD'), '99999999') ";
            string FillterColmn = "dateid ";
            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) &&
                (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.RealFieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }
            if ((!string.IsNullOrWhiteSpace(args.SecondFieldname)) &&
                          (!string.IsNullOrWhiteSpace(args.Criteria.SecondAdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.SecondFieldname.Trim(','), args.Criteria.SecondAdvancedCriteria) + filterBySql;
            }
            if (args.EntityType == EntityType.Deal&& string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                filterBySql += string.Format("     AND EXISTS(SELECT 1 FROM dim_buyer_deals buyDeal WHERE buyDeal.id = dealid and buyDeal.accountid ={0})", args.AccountId);
            }


            if (args.EntityType == EntityType.Audiances &&  args.Criteria.CampName > 0   )
            {
                filterBySql += string.Format("     AND EXISTS(Select camp.Id from dim_campaigns camp where camp.Id=FSA.campaignId and camp.Id={0}  )", args.Criteria.CampName);
            }
            if (args.EntityType == EntityType.Audiances && args.Criteria.CompanyName > 0)
            {
                filterBySql += string.Format("     AND EXISTS(Select acc.Id from dim_accounts acc  where acc.Id ={0} and  acc.Id=FSA.AccountId )", args.Criteria.CompanyName);
            }

            if (args.EntityType == EntityType.Audiances && args.Criteria.AdvertiserId>0)
            {
                filterBySql += string.Format("     AND EXISTS(Select campadv.Id from dim_advertisers campadv where campadv.Id=FSA.AdvertiserId and  campadv.Id={0}  )", args.Criteria.AdvertiserId);
            }
            switch (args.ChartCase)
            {
                case ChartCase.Hours:
                    {
                        FactStatTable = args.FactStatTable;
                        //   dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DateID), \"%Y-%m-%d\"), ' ', FSA.TimeId , \":00:00 \") AS Xaxis ";
                        dateFieldName = " to_timestamp(to_char(DateID, '99999999') ||  FSA.HourId ||  \':00:00 \'  , 'YYYYMMDDHH24:MI:SS')  AS Xaxis ";
                        groupBy = string.Format(" group by dateid,hourid");
                        break;
                    }
                case ChartCase.SixHours:
                    {
                        FactStatTable = args.FactStatTable;
                        //   dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DateID), \"%Y-%m-%d\"), ' ', (FSA.TimeId DIV  6)*6 , \":00:00 \") AS Xaxis  ";

                        dateFieldName = "  to_timestamp(to_char(DateID, '99999999') ||  ((FSA.HourId/6)::int)*6 ||  \':00:00 \'  , 'YYYYMMDDHH24:MI:SS') AS Xaxis  ";
                        groupBy = string.Format(" group by Xaxis");
                        break;
                    }
                case ChartCase.Day:
                    {
                        FactStatTable = args.DayFactStatTable;
                        dateFieldName = " to_timestamp(to_char(DateID, '99999999'), 'YYYYMMDD') AS Xaxis  ";
                        groupBy = string.Format(" group by DateID");
                        break;
                    }
                case ChartCase.Week:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = " to_timestamp(   etl.date_get_weekid(to_date(dateid||'', 'YYYYMMDD') ) ||'' , 'YYYYMMDD') AS Xaxis ";

                            groupBy = string.Format(" group by dateid");
                        }
                        if (summaryBy == SummaryBy.Week)
                        {
                            FactStatTable = args.WeekFactStatTable;
                            dateFieldName = "  to_timestamp(to_char(weekid, '99999999') , 'YYYYMMDD') AS Xaxis ";

                            groupBy = string.Format(" group by weekid");

                            //if (TimeId != null)
                            //{
                            //    TimeFilter = string.Format("and TimeId in ({0})", string.Join(", ", TimeId));

                            //    dates = string.Join(", ", TimeId);
                            //}

                            //TimeStringFormat = "weekid";

                            FillterColmn = "weekid";
                        }
                        break;
                    }
                case ChartCase.Month:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = "  to_timestamp(   to_char(date_trunc('month',to_date(dateid||'','YYYYMMDD') ), 'YYYYMMDD') , 'YYYYMMDD') AS Xaxis ";

                            groupBy = string.Format(" group by dateid");
                        }
                        if (summaryBy == SummaryBy.Month || summaryBy == SummaryBy.Accumulated)
                        {
                            FactStatTable = args.MonthFactStatTable;
                           
                            dateFieldName = "  to_timestamp(to_char(monthid, '99999999') , 'YYYYMMDD') AS Xaxis ";
                            groupBy = string.Format(" group by monthid");
                            FillterColmn = "monthid";
                        }
                        break;
                    }
            }

            //if ((groupBy.IndexOf("group by") >= 0) && args.EntityType == EntityType.Audiances)
            //{
            //    groupBy = groupByVar + "A,ccountId , Campaignid, UsedSegmentsId";
            //}

            string attachedFilter = string.Empty;
            var resultDtGeneration = GenerateBetweenDates(dates, FillterColmn, summaryBy);

            populateReportSQL = string.Format(@"insert into {2} (Yaxis,Xaxis) 
                                                            select {0},{1}", GetChartSql(args,ref attachedFilter), dateFieldName,args.reportDataTableName);


            var selectCommand = string.Empty;
            if (!string.IsNullOrEmpty(args.AccountIdFieldName))
            {
                 selectCommand = string.Format("{0} from {1} as FSA where {9}   and {3} ={4} {10} {5} {6} {7} {8};",
                                                  populateReportSQL, FactStatTable, dates,
                                                  args.AccountIdFieldName, args.AccountId,
                                                  appIDsSql, filterBySql, groupBy, orderBy, resultDtGeneration+ attachedFilter, args.AdvertiserIdEqualFormat);
            }
            else
            {
                  selectCommand = string.Format("{0} from {1} as FSA where {9}   {10} {5} {6} {7} {8};",
                                                  populateReportSQL, FactStatTable, dates,
                                                  args.AccountIdFieldName, args.AccountId,
                                                  appIDsSql, filterBySql, groupBy, orderBy, resultDtGeneration + attachedFilter, args.AdvertiserIdEqualFormat);

            }
            return selectCommand;
        }

        private static string GenerateDropGroupByNameTempTaple(ReportGeneratorArgs args)
        {
            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("DROP TABLE IF EXISTS {0};",args.groupDataTableName);
            }
            return string.Empty;
        }

        private static string GenerateDropTempTaple()
        {
            return string.Format("DROP TABLE IF EXISTS {0};");
        }

        private static string GetChartSql(ReportGeneratorArgs args,ref string attachedFilter)
        {
            var str = string.Empty;
            switch (args.Criteria.MetricCode.ToLower())
            {
                case "revenue":
                    {
                        str = "SUM(NetCost)";
                        break;
                    }
                case "adrequests":
                    {
                        str = "cast(SUM(Requests) as bigint)";
                        break;
                    }
                case "adimpress":
                case "impress":
                    {
                        str = "cast(SUM(Impressions) as bigint)";
                        break;
                    }
                case "adclicks":
                case "clicks":
                    {
                        str = "cast(SUM(Clicks) as bigint)";
                        break;
                    }
                case "fillrate":
                    {
                        str = "SUM(Impressions::bigint)/SUM(Requests)";
                        attachedFilter = " AND Requests > 0 ";
                        break;
                    }
                case "ctr":
                case "campctr":
                    {
                        str = "SUM(Clicks::bigint)/SUM(Impressions)";
                        attachedFilter = " AND Impressions > 0 ";
                        break;
                    }
                case "ecpm":
                    {
                        str = "((SUM(NetCost::bigint)/SUM(Impressions)) * 1000)";
                        attachedFilter = " AND Impressions > 0 ";
                        break;
                    }
                case "avgcpc":
                    {
                        // str = "(SUM(Spend::bigint)+SUM(totaldataprice::bigint))/SUM(Clicks)";
                        str = "(SUM(BillableCost::bigint))/SUM(Clicks)";
                        attachedFilter = " AND Clicks > 0 ";

                        if (args.EntityType != EntityType.AudianceSegmentsForAdvertiser)
                            // str = "(SUM(Spend::bigint)+SUM(totaldataprice::bigint))/SUM(Clicks)";
                            str = "(SUM(BillableCost::bigint))/SUM(Clicks)";
                        else
                            str = "SUM(DataFee::bigint)/SUM(Clicks)";

                        break;
                    }
                case "spend":
                    {
                        if(args.EntityType!=EntityType.AudianceSegmentsForAdvertiser)
                        str = "SUM(BillableCost)";
                        else
                            str = "SUM(DataFee)";

                        break;
                    }


                case "ai":
                    {  if (string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
                        {
                            str = "SUM(requests_d)";
                        }
                        else if (string.IsNullOrEmpty(args.Criteria.AdvancedCriteria))
                        {
                            str = "SUM(requests_d)";
                        }
                        else {
                            str = "SUM(requests_d)";
                        }
                        break;
                    }
                case "ads":
                    {
                       if (string.IsNullOrEmpty(args.Criteria.ItemsList))
                        {
                            str = "(SUM(requests_d)-SUM(unfilledrequests_d))";
                        }
                       else if (string.IsNullOrEmpty(args.Criteria.AdvancedCriteria))
                        {
                            str = "(SUM(requests_dca))";
                        }
                        else
                        {

                            str = "(SUM(requests_dag))";
                        }
                        /*if (string.IsNullOrEmpty(args.Criteria.AdvancedCriteria))
                        {
                            str = "(SUM(requests_d)-SUM(unfilledrequests_d))";
                        }
                        else
                        {

                            str = "(SUM(requests_dca))";
                        }*/
                        break;
                    }
                case "wi":
                    {
                        str = "SUM(wins)";
                        break;
                    }
                case "di":
                    {
                        str = "SUM(impressions)";
                        break;
                    }

                case "revaudi":
                    {
                        str = "SUM(revenue)";
                        break;
                    }
                case "impaudi":
                    {
                        str = "SUM(impressions)";
                        break;
                    }
            }
            return str;
        }
        private static string GetPartialDates(DateTime dateFrom, DateTime dateTo, SummaryBy summaryBy, out DateTime newDateFrom, out DateTime newDateTo)
        {
            var dayStr = string.Empty;

            newDateFrom = dateFrom;
            newDateTo = dateTo;
            if (dateTo == dateFrom)
            {
                dayStr += newDateTo.ToString("yyyyMMdd") + ",";
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
                                    dayStr += newDateFrom.ToString("yyyyMMdd") + ",";
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
                                    dayStr += newDateTo.ToString("yyyyMMdd") + ",";
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
                                    dayStr += newDateFrom.ToString("yyyyMMdd") + ",";
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
                                        dayStr += newDateFrom.ToString("yyyyMMdd") + ",";
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
                                            dayStr += newDateTo.ToString("yyyyMMdd") + ",";
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            return dayStr = dayStr.Trim(',');
        }
        private static string GetDays(DateTime dateFrom, DateTime dateTo, SummaryBy summaryBy)
        {
            var returnStr = string.Empty;
            switch (summaryBy)
            {
                case SummaryBy.Day:
                    {
                        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
                        for (var date = dateFrom; date <= dateTo; date = date.AddDays(1))
                        {
                            returnStr += date.ToString("yyyyMMdd") + ",";
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
                                returnStr += date.ToString("yyyyMMdd") + ",";
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
                                returnStr += date.ToString("yyyyMMdd") + ",";
                            }
                        }
                        break;
                    }
            }
            return returnStr.Trim(',');
        }
        private static string GenerateReportTempTableSelectCommand(ReportGeneratorArgs args)
        {if (!string.IsNullOrEmpty(args.OrderByStruct))
            {
                args.OrderByStruct = args.OrderByStruct.Replace(",", ",FSA.");
                  
                    
                    }
            var str = string.Empty;
            var innerJoin = string.Empty;
            string groupBY = string.Empty;
            var columnNameSQL = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
            //  use this dummy where to force using the index
            var dummyWhere = " WHERE  DateID>0  ";
            //string.Format("ORDER BY {0} {1}", args.Criteria.OrderColumn, args.Criteria.OrderType);
            var paging = string.Format("Order By {2} LIMIT {1} OFFSET {0}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                    args.Criteria.ItemsPerPage, args.OrderByStruct);
            if (args.IsAccumulated)
            {
                args.OrderByStruct = args.OrderByStruct.Replace("DateID,", "");
                args.OrderByStruct = args.OrderByStruct.Replace("DateID", "");
               
                args.OrderByStruct=args.OrderByStruct.Trim();
                if (!string.IsNullOrEmpty(args.OrderByStruct))
                {
                    paging = string.Format("Order By {2} LIMIT {1} OFFSET {0}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                           args.Criteria.ItemsPerPage, args.OrderByStruct);
                }
                else {

                    paging = string.Format(" LIMIT {1} OFFSET {0}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                         args.Criteria.ItemsPerPage, args.OrderByStruct);
                }
            }

            if (args.IsAccumulated)
            {

                columnNameSQL = " ";

            }
           

           
            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                groupBY = "group by Date,";
                if (args.IsAccumulated)
                {
                    groupBY = "group by ";

                }
                columnNameSQL += string.Format("{0}, ", args.DataFieldNameAlias);

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQL += " TimeId, ";
                    groupBY += " TimeId,";
                }

                groupBY += args.DataFieldNameAlias +"lower";

                switch (args.EntityType)
                {
                    case EntityType.AudianceSegmentsForAdvertiser:
                    case EntityType.Audiances:
                    case EntityType.Deal:
                    case EntityType.API:
                    case EntityType.App:
                        break;
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                        str = string.Format(@"insert into {5}_counts SELECT  '{4}', count(*) FROM {4};SELECT  {0} (select  reltuples from {5}_counts where relname='{4}') as TotalCount,
CAST(Impressions AS bigint) AS Impress,
CAST(WonImpressions AS bigint) AS WonImpressions,
CAST(RequestsByType AS bigint) AS RequestsByType,
CAST(Clicks AS bigint)  AS Clicks,

 CAST(conv_pr AS bigint) AS conv_pr,
CAST(conv_pr_ct AS bigint) AS conv_pr_ct,
CAST(conv_pr_vt AS bigint) AS conv_pr_vt,
 CAST(conv_ot AS bigint) AS conv_ot,
CAST(conv_ot_ct AS bigint) AS conv_ot_ct,
CAST(conv_ot_vt AS bigint) AS conv_ot_vt,

FSA.conv_pr_rev AS conv_pr_rev,
FSA.conv_pr_ct_rev AS  conv_pr_ct_rev,
FSA.conv_pr_vt_rev AS conv_pr_vt_rev,
FSA.conv_ot_rev AS   conv_ot_rev,
FSA.conv_ot_ct_rev AS conv_ot_ct_rev,
FSA.conv_ot_vt_rev AS conv_ot_vt_rev,




CAST(vcreativeviews as bigint ) as VCreativeViews, 
CAST(vstart AS bigint) as VStart,
CAST(vfirstquartile AS bigint) as VFirstQuartile,
CAST(vmidpoint AS bigint) as VMidPoint,
CAST(vthirdquartile AS bigint) as VThirdQuartile,
CAST(vcomplete AS bigint) as VComplete,
CAST(custom_events AS bigint) as CustomEvents,
CAST(pageviews AS bigint) as PageViews,

FSA.NetCost  as NetCost,
FSA.AdjustedNetCost as AdjustedNetCost,
FSA.GrossCost  as GrossCost,
FSA.platformfee  as platformfee,
FSA.DataFee  as DataFee,
FSA.thirdpartyfee  as thirdpartyfee,
FSA.AgencyRevenue  as AgencyRevenue,
FSA.BillableCost  as BillableCost


FROM {4} FSA 
{1}
{2}
{3};",
                             columnNameSQL, dummyWhere, ""
                             , paging,args.groupDataTableName, args.reportDataTableName);
                        break;
                }

            }
            else
            {

                string IdCoulmn = string.Empty;
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {
                    columnNameSQL += string.Format(" {0}.{1} AS {2}, ", args.DataTableName, args.DataFieldName, args.DataFieldNameAlias);
                    innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);
                    IdCoulmn = ",FSA."+args.DataFieldId + " AS Id";
                }

                foreach (var item in args.DataFieldNames)
                {
                    columnNameSQL += string.Format(" {0}.{1} AS {2}, ", args.DataTableName, item.Key, item.Value);
                }

                var outerJoin = string.Empty;

                //If group by hour
                if (args.Criteria.SummaryBy == 0 )
                {
                    columnNameSQL += " TimeId, ";
                }

                if (!string.IsNullOrWhiteSpace(args.Fieldname) || args.FieldNames.Count != 0)
                {
                    if (!string.IsNullOrWhiteSpace(args.Fieldname) && args.LocalizedStringFieldName.IndexOf("_") >= 0)
                    {
                        if (args.Criteria.Culture.Contains("en"))
                            columnNameSQL += args.LocalizedStringFieldName + "en AS Name,";
                        else
                            columnNameSQL += args.LocalizedStringFieldName + "ar AS Name,";
                    }
                    else if (!string.IsNullOrWhiteSpace(args.Fieldname) && args.SubType == SubType.SubSite)
                    {
                        columnNameSQL += args.LocalizedStringFieldName + " AS Name,";
                    }
                    else if (!string.IsNullOrWhiteSpace(args.Fieldname) && args.SubType == SubType.AppSite)
                    {
                        if (!string.IsNullOrWhiteSpace(args.SecondFieldname))
                        {
                            columnNameSQL += "COALESCE(" + "O2." + args.SecondLocalizedStringFieldName + ",O." + args.LocalizedStringFieldName + ") AS Name,";
                        }
                        else
                        {
                            columnNameSQL += "O." + args.LocalizedStringFieldName + " AS Name,";
                        }

                        
                    }
                    else
                    {
                        columnNameSQL += "O." + args.LocalizedStringFieldName + " AS SubName,"; ;
                    }

                    foreach (var item in args.FieldNames)
                    {
                        columnNameSQL += string.Format("{0} AS {1},", item.Key, item.Value);
                    }


                    outerJoin = string.Format(
                        @"LEFT OUTER JOIN  {0} O ON O.{1} = FSA.{2}
 ",
                        args.TableIdName, args.IdFieldName,
                        args.Fieldname.Trim(','), args.LocalizedStringFieldName,
                        args.Criteria.Culture, ReportGeneratorArgs.DataBase);
                    dummyWhere = " AND  DateID>0 ";
                }
                if (!string.IsNullOrWhiteSpace(args.SecondFieldname))
                {
                    if (!string.IsNullOrWhiteSpace(args.SecondFieldname) && args.SecondLocalizedStringFieldName.IndexOf("_") >= 0)
                    {
                        if (args.Criteria.Culture.Contains("en"))
                            columnNameSQL += args.SecondLocalizedStringFieldName + "en AS SecondSubName,";
                        else
                            columnNameSQL += args.SecondLocalizedStringFieldName + "ar AS SecondSubName,";
                    }
                    else
                    {
                        columnNameSQL +="O2."+ args.SecondLocalizedStringFieldName + " AS SecondSubName,";
                    }

                  


                    outerJoin = string.Format(
                        @"LEFT OUTER JOIN  {0} O2 ON O2.{1} = FSA.{2}
 ",
                        args.SecondTableIdName, args.SecondIdFieldName,
                        args.SecondFieldname.Trim(','), args.SecondLocalizedStringFieldName,
                        args.Criteria.Culture, ReportGeneratorArgs.DataBase)+ outerJoin;
                
                }

                switch (args.EntityType)
                {
                    case EntityType.API:
                        str = string.Format(
                                @"insert into {5}_counts SELECT  '{5}', count(*) FROM {5}; SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,
CAST(Requests AS bigint) AS r,
CAST(Impressions AS bigint) AS i,
 Revenue AS rv,
CAST(Clicks AS bigint) AS c
FROM {5} FSA 
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging, args.reportDataTableName);
                        break;
                    case EntityType.Deal:
                        {
                            if (!string.IsNullOrWhiteSpace(args.Fieldname))
                                dummyWhere = " where  DateID>0 AND  " + args.Fieldname.Trim(',') + ">0";

                          
                            str = string.Format(
                          @"insert into {5}_counts SELECT  '{5}', count(*),SUM( AvailableImpressions)  FROM {5} {3}; UPDATE {5}_counts SET TotalAvailableImpressions=(SELECT SUM( AvailableImpressions) FROM {5}); SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,(select  TotalAvailableImpressions from {5}_counts where relname='{5}') as TotalAvailableImpressions,
CAST(requests_dcr AS bigint) AS requests_dcr,
CAST(DisplayedImpressions AS bigint) AS DisplayedImpressions,
CAST(unfilledrequests AS bigint)  AS unfilledrequests,
 WonImpressions AS WonImpressions,
CAST(AvailableImpressions AS bigint) AS AvailableImpressions


FROM {5} FSA 
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging, args.reportDataTableName);


                            break;
                        }

                    case EntityType.Audiances:
                        {
                            string AdvertiserName = string.Empty;
                            if (args.Criteria.Culture.Contains("en"))
                                AdvertiserName = "adv.Name_" + "en AS AdvertiserName";
                            else
                                AdvertiserName = "adv.Name_" + "ar AS AdvertiserName";

                            if (args.Criteria.CompanyName > 0)
                            {
                                dummyWhere = dummyWhere + string.Format("  AND CAMP.AccountId ={0} ", args.Criteria.CompanyName);
                            }
                            if (  args.Criteria.CampName > 0)
                            {
                                dummyWhere = dummyWhere + string.Format("  AND CAMP.Id={0}  ", args.Criteria.CampName);
                            }
                            if (args.Criteria.AdvertiserId>0)
                            {
                                dummyWhere = dummyWhere + string.Format("  AND FSA.AdvertiserId={0} ", args.Criteria.AdvertiserId);
                            }
                            //IdCoulmn = IdCoulmn.Replace("AS Id", "AS DataProviderId");

                            str = string.Format(
                                 @"insert into {5}_counts SELECT  '{5}', count(*) FROM {5};SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,
CAST(Impressions AS bigint) As Impressions ,
 billedsegment AS billedsegmentId,
cast(usedseg.usedsegmentsids  as VARCHAR ) AS UsedSegments,
 Revenue AS Revenue,
 FSA.grossrevenue  AS grossrevenue ,
 FSA.avrcost  AS avrcost ,
 FSA.Discount AS Discount,
 CAMP.Name AS CampaignName,

{7}


{6}

FROM {5} FSA INNER JOIN dim_advertisers adv ON FSA.AdvertiserId= adv.Id iNNER JOIN dim_campaigns CAMP ON CAMP.id=FSA.CAMPAIGNID INNER JOIN dim_accounts acc ON FSA.AccountId= acc.Id   INNER JOIN dim_users users ON acc.primaryuserid= users.Id   iNNER JOIN dim_usedsegmentsgroups usedseg ON usedseg.id=FSA.usedsegmentsid
{1}
{2}
{3}
{4};",
                                 columnNameSQL, innerJoin, outerJoin, dummyWhere, paging, args.reportDataTableName, IdCoulmn, AdvertiserName);
                            break;
                        }
                    case EntityType.App:
                        {
                            str = string.Format(
                                @"insert into {5}_counts SELECT  '{5}', count(*) FROM {5};SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,
CAST(Requests AS bigint) AS AdRequests,
CAST(Impressions AS bigint) AS AdImpress,
 Revenue AS Revenue,
CAST(Clicks AS bigint) AS Clicks
{6}

FROM {5} FSA 
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging,args.reportDataTableName, IdCoulmn);
                            break;
                        }

                    case EntityType.AudianceSegmentsForAdvertiser:
                        {
                            str = string.Format(
                             @"insert into {5}_counts SELECT  '{5}', count(*) FROM {5};SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,
CAST(Impressions AS bigint) AS Impress,

CAST(Clicks AS bigint) AS Clicks,
 CAST(conv_pr AS bigint) AS conv_pr,
CAST(conv_pr_ct AS bigint) AS conv_pr_ct,
CAST(conv_pr_vt AS bigint) AS conv_pr_vt,
 CAST(conv_ot AS bigint) AS conv_ot,
CAST(conv_ot_ct AS bigint) AS conv_ot_ct,
CAST(conv_ot_vt AS bigint) AS conv_ot_vt,

FSA.conv_pr_rev AS conv_pr_rev,
FSA.conv_pr_ct_rev AS  conv_pr_ct_rev,
FSA.conv_pr_vt_rev AS conv_pr_vt_rev,
FSA.conv_ot_rev AS   conv_ot_rev,
FSA.conv_ot_ct_rev AS conv_ot_ct_rev,
FSA.conv_ot_vt_rev AS conv_ot_vt_rev,
CAST(vcreativeviews as bigint ) as VCreativeViews, 
CAST(vstart AS bigint) as VStart,
CAST(vfirstquartile AS bigint) as VFirstQuartile,
CAST(vmidpoint AS bigint) as VMidPoint,
CAST(vthirdquartile AS bigint) as VThirdQuartile,
CAST(vcomplete AS bigint) as VComplete,
CAST(custom_events AS bigint) as CustomEvents,
CAST(pageviews AS bigint) as PageViews,
FSA.DataFee AS DataFee,
O.providerid  AS ProviderId,
FSA.segmentid AS SegmentId
{6}
FROM {5} FSA 
{1}
{2}
{3}
{4};",
                            columnNameSQL, innerJoin, outerJoin, dummyWhere, paging, args.reportDataTableName, IdCoulmn);

                            break;
                        }
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                        {
                            str = string.Format(
                                @"insert into {5}_counts SELECT  '{5}', count(*) FROM {5};SELECT  {0} (select  reltuples from {5}_counts where relname='{5}') as TotalCount,
CAST(Impressions AS bigint) AS Impress,
CAST(WonImpressions AS bigint) AS WonImpressions,
CAST(RequestsByType AS bigint) AS RequestsByType,
CAST(Clicks AS bigint) AS Clicks,
 CAST(conv_pr AS bigint) AS conv_pr,
CAST(conv_pr_ct AS bigint) AS conv_pr_ct,
CAST(conv_pr_vt AS bigint) AS conv_pr_vt,
 CAST(conv_ot AS bigint) AS conv_ot,
CAST(conv_ot_ct AS bigint) AS conv_ot_ct,
CAST(conv_ot_vt AS bigint) AS conv_ot_vt,

FSA.conv_pr_rev AS conv_pr_rev,
FSA.conv_pr_ct_rev AS  conv_pr_ct_rev,
FSA.conv_pr_vt_rev AS conv_pr_vt_rev,
FSA.conv_ot_rev AS   conv_ot_rev,
FSA.conv_ot_ct_rev AS conv_ot_ct_rev,
FSA.conv_ot_vt_rev AS conv_ot_vt_rev,
CAST(vcreativeviews as bigint ) as VCreativeViews, 
CAST(vstart AS bigint) as VStart,
CAST(vfirstquartile AS bigint) as VFirstQuartile,
CAST(vmidpoint AS bigint) as VMidPoint,
CAST(vthirdquartile AS bigint) as VThirdQuartile,
CAST(vcomplete AS bigint) as VComplete,
CAST(custom_events AS bigint) as CustomEvents,
CAST(pageviews AS bigint) as PageViews,


FSA.NetCost  as NetCost,
FSA.AdjustedNetCost  as AdjustedNetCost,
FSA.GrossCost  as GrossCost,
FSA.platformfee  as platformfee,
FSA.DataFee  as DataFee,
FSA.thirdpartyfee  as thirdpartyfee,
FSA.AgencyRevenue  as AgencyRevenue,
FSA.BillableCost  as BillableCost





{6}
FROM {5} FSA 
{1}
{2}
{3}
{4};",
                               columnNameSQL, innerJoin, outerJoin, dummyWhere, paging,args.reportDataTableName, IdCoulmn);
                            break;
                        }
                }
            }

            return str;
        }

        private static string GenerateSelectCountCommand(ReportGeneratorArgs args)
        {
            var selectCommand = string.Format("{0}",args.reportDataTableName);

            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select DateID,{0} from {1} group by DateID,{0}) counting", args.DataFieldNameAlias,args.groupDataTableName);
            }
            if (args.IsAccumulated && args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select {0} from {1} group by {0}) counting", args.DataFieldNameAlias, args.groupDataTableName);
            }

            return selectCommand;
        }
        private static string GenerateBetweenDates(string dates, string dateField, SummaryBy summaryBy)
       {
          IList<int> datesNo=     (dates ?? "").Split(',').Select<string, int>(int.Parse).ToList();
            datesNo= datesNo.OrderBy(M=>M).ToList();
          IDictionary<int, int> dicgouping = new Dictionary<int, int>();
            int groupIndex = 0;
            int diffDay = 0;
            int IncreasedgroupIndex = 0;
            if (summaryBy==SummaryBy.Day)
            {
                groupIndex = 1;
                diffDay = 1;
                IncreasedgroupIndex =1;
            }
            if (summaryBy == SummaryBy.Accumulated)
            {
                diffDay = 28;
                groupIndex = 100;
                IncreasedgroupIndex = 100;
            }
            if (summaryBy == SummaryBy.Week)
            {
                diffDay = 7;
                groupIndex = 7;
                IncreasedgroupIndex = 7;
            }
            if (summaryBy == SummaryBy.Month)
            {
                diffDay =28 ;
                groupIndex = 100;
                IncreasedgroupIndex =100;
            }
       
            dicgouping.Add( datesNo[0], IncreasedgroupIndex);

            DateTime firstDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime secondDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            for (int i=1; i< datesNo.Count;i++)

            {
                firstDate = DateTime.ParseExact(datesNo[i-1].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                secondDate = DateTime.ParseExact(datesNo[i].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                var diff= (secondDate - firstDate).TotalDays;

                if (diff == diffDay ) 

                {
                    dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                }
                else
                {
                    if (summaryBy == SummaryBy.Month || summaryBy == SummaryBy.Accumulated)
                    {
                        if (diffDay >= 28 && diffDay <= 31)
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
            var results= dicgouping.GroupBy(n => n.Value).Select(g => g.Count() >= 2 ?
      dateField + " BETWEEN " + g.First().Key + " AND " + g.Last().Key
      :
         dateField + "="+ g.Select(x => x.Key).First()).ToList();


  //          List<string> items = datesNo
  //.Select((n, i) => new { number = n, group = n - i })
  //.GroupBy(n => n.group)
  //.Select(g =>
  //  g.Count() >= 2 ?
  //   "BETWEEN "+dateField + g.First().number + " AND " + g.Last().number
  //  :
  //    String.Join(", "+dateField+"=", g.Select(x => x.number))
  //)
  //   .ToList();

         return   "(" +String.Join(" oR ", results)+")";

          //  var  groupIndex = 1;
          //  foreach (var dateNo in datesNo)
          //  {

          //      dicgouping.Add(dateNo, groupIndex);



          //  }

          //return string.Empty;


        }
        /*private static string GenerateReportTempTableSelectCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;
            var innerJoin = string.Empty;
            var columnNameSQL = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
            //  use this dummy where to force using the index
            var dummyWhere = " WHERE  DateID>0 ";
            //string.Format("ORDER BY {0} {1}", args.Criteria.OrderColumn, args.Criteria.OrderType);
            var paging = string.Format("LIMIT {0}, {1}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                       args.Criteria.ItemsPerPage);

            if (args.IsAccumulated)
            {

                columnNameSQL = " ";

            }

            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                string groupBY = "group by Date,";
                if (args.IsAccumulated)
                {
                    groupBY = "group by ";

                }

                columnNameSQL += string.Format("{0}, ", args.DataFieldNameAlias);

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQL += " TimeId, ";
                    groupBY += " TimeId,";
                }

                groupBY += args.DataFieldNameAlias;

                switch (args.EntityType)
                {
                    case EntityType.API:
                    case EntityType.App:
                        break;
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                        str = string.Format(@"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
CAST(sum(Impressions) AS SIGNED) AS Impress,
CAST(sum(Clicks) AS SIGNED) AS Clicks,
sum(FSA.Spend) AS Spend
FROM grouped_data FSA force index(grouped_index)
{1}
{2}
{3};",
                             columnNameSQL, dummyWhere, groupBY, paging);
                        break;
                }

            }
            else
            {
                if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {
                    columnNameSQL += string.Format(" {0}.{1} AS {2}, ", args.DataTableName, args.DataFieldName, args.DataFieldNameAlias);
                    innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);


                }

                foreach (var item in args.DataFieldNames)
                {
                    columnNameSQL += string.Format(" {0}.{1} AS {2}, ", args.DataTableName, item.Key, item.Value);
                }

                var outerJoin = string.Empty;

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQL += " TimeId, ";
                }

                if (!string.IsNullOrWhiteSpace(args.Fieldname) || args.FieldNames.Count != 0)
                {
                    if (!string.IsNullOrWhiteSpace(args.Fieldname))
                    {
                        columnNameSQL += "Value AS Name,";
                    }

                    foreach (var item in args.FieldNames)
                    {
                        columnNameSQL += string.Format("{0} AS {1},", item.Key, item.Value);
                    }


                    outerJoin = string.Format(
                        @"LEFT OUTER JOIN  {0} O ON O.{1} = FSA.{2}
LEFT OUTER JOIN {5}localizedstrings Locs
ON {3} = Locs.LocalizedStringID
WHERE (isnull(Locs.Culture) or Locs.Culture ='{4}') ",
                        args.TableIdName, args.IdFieldName,
                        args.Fieldname.Trim(','), args.LocalizedStringFieldName,
                        args.Criteria.Culture, ReportGeneratorArgs.DataBase);
                    dummyWhere = " AND  DateID>0 ";
                }
                if (args.IsAccumulated && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                {

                    if (!string.IsNullOrEmpty(args.IdFieldName))
                    {
                        dummyWhere = dummyWhere + string.Format(" group by {0} ", args.DataFieldId + "," + args.Fieldname.Replace(",",string.Empty));
                    }
                    else
                    {

                        dummyWhere = dummyWhere + string.Format(" group by {0} ", args.DataFieldId);
                    }
                }

                switch (args.EntityType)
                {
                    case EntityType.API:
                        str = args.IsAccumulated ? string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
SUM(CAST(Requests AS SIGNED)) AS r,
SUM(CAST(Impressions AS SIGNED)) AS i,
 SUM(Revenue) AS rv,
SUM(CAST(Clicks AS SIGNED)) AS c
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging) : string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
CAST(Requests AS SIGNED) AS r,
CAST(Impressions AS SIGNED) AS i,
 Revenue AS rv,
CAST(Clicks AS SIGNED) AS c
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging);
                        break;
                    case EntityType.App:
                        {
                            str = args.IsAccumulated ? string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
SUM(CAST(Requests AS SIGNED)) AS AdRequests,
SUM(CAST(Impressions AS SIGNED)) AS AdImpress,
 SUM(Revenue) AS Revenue,
SUM(CAST(Clicks AS SIGNED)) AS Clicks
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging) : string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
CAST(Requests AS SIGNED) AS AdRequests,
CAST(Impressions AS SIGNED) AS AdImpress,
 Revenue AS Revenue,
CAST(Clicks AS SIGNED) AS Clicks
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                                columnNameSQL, innerJoin, outerJoin, dummyWhere, paging);
                            break;
                        }
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                        {
                            str = args.IsAccumulated ? string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
Sum(CAST(Impressions AS SIGNED)) AS Impress,
Sum(CAST(Clicks AS SIGNED)) AS Clicks,
Sum(FSA.Spend) AS Spend
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                               columnNameSQL, innerJoin, outerJoin, dummyWhere, paging) : string.Format(
                                @"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount,
CAST(Impressions AS SIGNED) AS Impress,
CAST(Clicks AS SIGNED) AS Clicks,
FSA.Spend AS Spend
FROM report_data FSA force index(operator_by_day)
{1}
{2}
{3}
{4};",
                               columnNameSQL, innerJoin, outerJoin, dummyWhere, paging);
                            break;
                        }
                }
            }

            return str;
        }

        private static string GenerateSelectCountCommand(ReportGeneratorArgs args)
        {
            var selectCommand = "report_data";
            string innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);
            if (args.IsAccumulated)
            {
                selectCommand = "dual";
            }
            if (args.IsAccumulated && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select {0} from report_data FSA force index(operator_by_day) {1} WHERE  DateID>0 group by {0}) counting", args.DataFieldId, innerJoin);
            }

            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select DateID,{0} from grouped_data group by DateID,{0}) counting", args.DataFieldNameAlias);
            }
            if (args.IsAccumulated && args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select {0} from grouped_data group by {0}) counting", args.DataFieldNameAlias);
            }
            return selectCommand;
        }*/


    }


}
