using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.ConfigurationSetting;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
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
        GeoLocation = 4

    }
    public class ReportGeneratorArgs : BaseGeneratorArgs
    {
        private static IConfigurationManager configurationManager;
        private static IAccountRepository accountRepository;

        private static IAdvertiserAccountRepository advertiserAccountRepository;
        private static IAccountStatistic accountStatistic;
        public const string DateField = "DateID";
        public static string DataBase = ArabyAds.AdFalcon.Domain.Configuration.DB;

        public ReportCriteriaDto Criteria { get; set; }
        public ChartCase ChartCase { get; set; }

        public SubType SubType { get; set; }

        public int Threshold { get; set; }
        public int Count { get; set; }

        //this data should be changed 
        public string AccountIdFieldName { get; set; }
        
        public int AccountId { get; set; }
        public bool IsAccumulated { get; set; }//"OperatorID"
        public string IdFieldName { get; set; }//"OperatorID"
        public string Fieldname { get; set; }//"OperatorID"
        public string TableIdName { get; set; }//"operators"
        public string LocalizedStringFieldName { get; set; }//"OperatorNameId"
        public Dictionary<string, string> FieldNames;

        public string DataTableName { get; set; }//"appsite"
        public string FilterDataFieldId { get; set; }//"AppSiteID"
        public string DataFieldId { get; set; }//"AppSiteID"
        public string SelectDataIds { get; set; }//"AppSiteID"
        //public string SelectAllDataIds { get; set; }//"AppSiteID"
        public string DataFieldName { get; set; }//"Name"
        public string DataFieldNameAlias { get; set; }//"SubName"

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
                case EntityType.API:
                case EntityType.App:
                    {
                        Ids = accountStatistic.GetAppIds(account);
                        break;
                    }
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
                case EntityType.Campaign:
                    {
                        Ids = accountStatistic.GetCampaignIdsPerUser(accountId, userId);
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

        public static ReportGeneratorArgs GetInstance(ReportCriteriaDto criteria, int accountId, ReportType reportType, EntityType entityType, SubType subType)
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
                Count = count,
                Threshold = threshold,
                DataFieldNames = new Dictionary<string, string>(),
                FieldNames = new Dictionary<string, string>()
            };
            if (!criteria.IsPrimaryUser && string.IsNullOrEmpty(instance.Criteria.ItemsList))
            { instance.Criteria.ItemsList = GetAccountEntityIds(entityType, accountId, criteria.userId); }
            switch (entityType)
            {
                case EntityType.App:
                    {
                        instance.DataTableName = DataBase + "appsite";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_app";
                        instance.DayFactStatTable = "fact_stat_app_day";
                        instance.WeekFactStatTable = "fact_stat_app_week";
                        instance.MonthFactStatTable = "fact_stat_app_month";
                        instance.AccountIdFieldName = "AccountID";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "appsite where AccountId={0}";
                        break;
                    }
                case EntityType.API:
                    {
                        instance.DataTableName = DataBase + "appsite";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldName = "PublisherId";
                        instance.DataFieldNameAlias = "aid";
                        instance.FactStatTable = "fact_stat_app";
                        instance.DayFactStatTable = "fact_stat_app_day";
                        instance.WeekFactStatTable = "fact_stat_app_week";
                        instance.MonthFactStatTable = "fact_stat_app_month";
                        instance.AccountIdFieldName = "AccountID";
                        instance.SelectDataIds = "{0}";
                        instance.DataFieldNames.Add("Name", "an");
                        break;
                    }
                case EntityType.Campaign:
                    {
                        instance.DataTableName = DataBase + "campaigns";
                        instance.DataFieldId = "CampaignID";
                        instance.FilterDataFieldId = "CampaignID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from " + DataBase + "campaigns where id in ({0})";
                        //instance.SelectAllDataIds = "select id from " + DataBase + "campaigns where AccountId={0}";
                        break;
                    }
                case EntityType.AdGroup:
                    {

                        instance.DataTableName = DataBase + "adgroups";
                        instance.DataFieldId = "AdGroupID";
                        instance.FilterDataFieldId = "AdGroupID";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from " + DataBase + "adgroups where id in ({0})";
                        //instance.SelectAllDataIds = @"select " + DataBase + "adgroups.id FROM " + DataBase + "adgroups INNER JOIN " + DataBase + "campaigns ON " + DataBase + "adgroups.CampaignId = " + DataBase + "campaigns.Id where  AccountId={0}";
                        break;
                    }
                case EntityType.Ad:
                    {
                        instance.DataTableName = DataBase + "ads";
                        instance.DataFieldId = "AdId";
                        instance.FilterDataFieldId = "AdId";
                        instance.DataFieldName = "Name";
                        instance.DataFieldNameAlias = "SubName";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = @"SELECT " + DataBase + "ads.Id FROM " + DataBase + "ads INNER JOIN " + DataBase + "adgroups ON " + DataBase + "ads.AdGroupId = " + DataBase + "adgroups.Id INNER JOIN " + DataBase + "campaigns on " + DataBase + "adgroups.CampaignId = campaigns.Id WHERE " + DataBase + "campaigns.AccountId ={0}";
                        break;
                    }
            }

            switch (subType)
            {
                case SubType.Operator:
                    {
                        instance.TableIdName = DataBase + "operators";
                        instance.Fieldname = ",OperatorID";
                        instance.IdFieldName = "OperatorID";
                        instance.LocalizedStringFieldName = "OperatorNameId";
                        break;
                    }
                case SubType.Platform:
                    {
                        instance.TableIdName = DataBase + "platforms";
                        instance.Fieldname = ",PlatformID";
                        instance.IdFieldName = "PlatformId";
                        instance.LocalizedStringFieldName = "PlatformNameId";
                        break;
                    }
                case SubType.Manufacturer:
                    {
                        instance.TableIdName = DataBase + "manufacturers";
                        instance.Fieldname = ",ManufacturerID";
                        instance.IdFieldName = "ManufacturerId";
                        instance.LocalizedStringFieldName = "NameId";
                        break;
                    }
                case SubType.GeoLocation:
                    {
                        instance.TableIdName = DataBase + "locations";
                        instance.Fieldname = ",CountryID";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "NameId";
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

            if ((string.IsNullOrWhiteSpace(instance.Criteria.ItemsList)) && (instance.Count < instance.Threshold))
            {
                // instance.Criteria.ItemsList = string.Format(instance.SelectAllDataIds, instance.AccountId.ToString());
                instance.Criteria.ItemsList = GetAccountEntityIds(entityType, accountId);
                instance.SelectDataIds = "{0}";
            }
            return instance;
        }

    }
    public static class ScriptGenerator
    {
        public static string GenerateQueryScript(ReportGeneratorArgs args)
        {
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
                        foreach (var table in tableInfo)
                        {
                            var tableName = table.Key;
                            var tableDays = table.Value;
                            args.DayFactStatTable = tableName;
                            reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Day, args));
                        }
                        break;
                    }
                case 1: //day
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Day);
                        foreach (var table in tableInfo)
                        {
                            var tableName = table.Key;
                            var tableDays = table.Value;
                            args.DayFactStatTable = tableName;
                            reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Day, args));
                        }
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
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.DayFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Day, args));
                            }
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.WeekFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Week, args));
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
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.DayFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Day, args));
                            }
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.MonthFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Accumulated, args));
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
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate,args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.DayFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Day, args));
                            }
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.MonthFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateSelectCommand(tableDays, SummaryBy.Month, args));
                            }
                        }
                        break;
                    }
            }

            reportCommand.AppendLine(GenerateGroupByNameTapleCommand(args));

            reportCommand.AppendLine(GenerateGroupByNameInsertCommand(args));

            reportCommand.AppendLine(GenerateTempTableCountCommand(GenerateSelectCountCommand(args)));
            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            reportCommand.AppendLine(GenerateDropGroupByNameTempTaple(args));
            reportCommand.AppendLine(GenerateDropTempTaple());
            
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString().Trim('\n').Trim();
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
            switch (args.ChartCase)
            {
                case ChartCase.Hours: //Hours
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        foreach (var table in tableInfo)
                        {
                            var tableName = table.Key;
                            var tableDays = table.Value;
                            args.FactStatTable = tableName;
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args));
                        }
                        break;
                    }
                case ChartCase.SixHours: //day
                    {
                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Hour);
                        foreach (var table in tableInfo)
                        {
                            var tableName = table.Key;
                            var tableDays = table.Value;
                            args.FactStatTable = tableName;
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args));
                        }
                        break;
                    }
                case ChartCase.Day: //Day
                    {

                        var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Day);
                        foreach (var table in tableInfo)
                        {
                            var tableName = table.Key;
                            var tableDays = table.Value;
                            args.DayFactStatTable = tableName;
                            reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args));
                        }
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
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.DayFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args, SummaryBy.Day));
                            }
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.WeekFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args, SummaryBy.Week));
                            }
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
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.DayFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args, SummaryBy.Day));
                            }
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            foreach (var table in tableInfo)
                            {
                                var tableName = table.Key;
                                var tableDays = table.Value;
                                args.MonthFactStatTable = tableName;
                                reportCommand.AppendLine(GenerateChartSelectCommand(tableDays, args, SummaryBy.Month));
                            }
                        }
                        break;
                    }
            }


            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            reportCommand.AppendLine(GenerateDropTempTaple());
            
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
                                         : string.Format("`{0}` smallint(5) unsigned DEFAULT NULL,", args.Fieldname.Trim(','));
                        key = string.IsNullOrWhiteSpace(args.Fieldname)
                                         ? string.Empty
                                         : string.Format("{0}", args.Fieldname);
                        if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
                        {
                            idColumn = string.Format("`{0}` int(11) NOT NULL,", args.DataFieldId);
                            key = string.Format(",{0}", args.DataFieldId) + key;
                        }

                        switch (args.Criteria.SummaryBy)
                        {
                            case 0:
                                timeIdColumn = "`TimeId` int(11) NULL,";
                                timeIdIndex = ",TimeId";
                                break;
                            default:
                                break;
                        }

                        switch (args.EntityType)
                        {
                            case EntityType.API:
                            case EntityType.App:
                                {
                                    str = string.Format(
@"DROP TABLE IF EXISTS report_data;
create temporary table report_data (
`DateID` int(11) NOT NULL,
{0}{1}
`Requests` bigint(11) DEFAULT '0',
`Impressions` bigint(11) DEFAULT '0',
`Clicks` bigint(11) DEFAULT '0',
`Revenue` decimal(21,12) DEFAULT '0.00000',{2}
 UNIQUE KEY operator_by_day(DateID{3}{4})
) ENGINE=MyISAM;", timeIdColumn, filterByColumn, idColumn, timeIdIndex, key);
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    str = string.Format(
@"DROP TABLE IF EXISTS report_data;
create temporary table report_data (
`DateID` int(11) NOT NULL,
{0}{1}
`Impressions` bigint(11) DEFAULT '0',
`Clicks` bigint(11) DEFAULT '0',
`Spend` decimal(21,12) DEFAULT '0.00000',{2}
UNIQUE KEY operator_by_day(DateID{3}{4})
) ENGINE=MyISAM;", timeIdColumn, filterByColumn, idColumn,timeIdIndex, key);
                                    break;
                                }

                        }
                        break;
                    }
                case ReportType.Chart:
                    {
                        str = string.Format(
                            @"DROP TABLE IF EXISTS report_data;
create temporary table report_data (
`Id` int(11) NOT NULL AUTO_INCREMENT,
`Xaxis` DateTime NOT NULL,
`Yaxis` decimal(21,12) ,
PRIMARY KEY (`Id`)
) ENGINE=MyISAM;");
                        break;
                    }
            }
            //if(args.IsAccumulated)
            //{
            //    str = str.Replace("UNIQUE KEY operator_by_day(DateID,", "UNIQUE KEY operator_by_day(");
            //    str = str.Replace("UNIQUE KEY operator_by_day(DateID)", "KEY operator_by_day(DateID)");
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
                        idColumn = string.Format("`{0}` VARCHAR(255) NOT NULL,", args.DataFieldNameAlias);
                        key = string.Format(",{0}", args.DataFieldNameAlias) + key;

                        switch (args.Criteria.SummaryBy)
                        {
                            case 0:
                                timeIdColumn = "`TimeId` int(11) NULL,";
                                timeIdIndex = ",TimeId";
                                break;
                            default:
                                break;
                        }


                        switch (args.EntityType)
                        {
                            case EntityType.API:
                            case EntityType.App:
                                break;
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                str = string.Format(@"DROP TABLE IF EXISTS grouped_data;
                                create temporary table grouped_data (
                                `DateID` int(11) NOT NULL,
                                {0}
                                `Impressions` int(11) DEFAULT '0',
                                `Clicks` int(11) DEFAULT '0',
                                `Spend` decimal(21,12) DEFAULT '0.00000',{1}
                                KEY grouped_index(DateID{2}{3})
                                ) ENGINE=MyISAM;", timeIdColumn, idColumn, timeIdIndex, key);
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
                        str = "select Xaxis,Yaxis from report_data;";
                        break;
                    }
            }
            return str;
        }

        private static string GenerateGroupByNameInsertCommand(ReportGeneratorArgs args)
        {
            string str = string.Empty;

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

                string insertStatement = string.Format(@"insert into grouped_data(DateID,{1}{0},Impressions,Clicks,Spend) ", timeIdColumn, args.DataFieldNameAlias);

                var innerJoin = string.Empty;
                var columnNameSQL = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
                //  use this dummy where to force using the index
                var dummyWhere = " WHERE  DateID>0 ";

                columnNameSQL += string.Format(" {0}.{1} AS {2}, ", args.DataTableName, args.DataFieldName, args.DataFieldNameAlias);
                innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);

                //If group by hour
                if (args.Criteria.SummaryBy == 0)
                {
                    columnNameSQL += " TimeId, ";
                }

                switch (args.EntityType)
                {
                        
                    case EntityType.API:
                    case EntityType.App:
                        break;
                    case EntityType.Campaign:
                    case EntityType.AdGroup:
                    case EntityType.Ad:
                         selectCommand = string.Format(
                            @"SELECT  {0} 
CAST(Impressions AS SIGNED) AS Impress,
CAST(Clicks AS SIGNED) AS Clicks,
FSA.Spend AS Spend
FROM report_data FSA force index(operator_by_day)
{1}
{2};",
                           columnNameSQL, innerJoin, dummyWhere);
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
            return string.Format(@"set @TotalCount=(SELECT count(1) FROM  {0});", selectCommand);
        }

        private static string GenerateSelectCommand(string dates, SummaryBy summaryBy, ReportGeneratorArgs args)
        {
            const string orderBy = " order by NULL";

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
                groupByFields = string.Format(",{0}{1}", args.DataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",IFNULL({0},0){1}", args.Fieldname.Trim(','), idColumn);
            }

            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) && (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.Fieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }

            switch (summaryBy)
            {

                case SummaryBy.Day:
                    {
                        FactStatTable = args.DayFactStatTable;
                        switch (args.Criteria.SummaryBy)
                        {
                            case 0: //Hour
                                {
                                    dateFieldName = "DateID as newDateID";
                                    includeTime = true;
                                    groupBy = string.Format(" group by newDateID,TimeId{0}", groupByFields);  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "DATE_FORMAT(DateID - INTERVAL ( DATE_FORMAT(DateID, \'%w\') ) DAY , \'%Y%m%d\') as newDateID";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    break;
                                }
                            case 4: //Accumlated
                                {
                                    dateFieldName = "DATE_FORMAT(DateID , \'%Y%m01\') as newDateID";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    groupBy=groupBy.Replace("newDateID,", string.Empty);
                                    if (groupBy.IndexOf("group by newDateID") > 0)
                                    {
                                        groupBy = groupBy.Replace("group by newDateID", string.Empty);
                                    }
                                   dateFieldName = "1";
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "DATE_FORMAT(DateID , \'%Y%m01\') as newDateID";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
                                    break;
                                }
                        }
                        break;
                    }
                case SummaryBy.Week:
                    {
                        FactStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                    {
                        dateFieldName = "1";
                        FactStatTable = args.MonthFactStatTable;
                        groupBy = groupBy.Replace("DateID,", string.Empty);
                        if(groupBy.IndexOf("group by DateID")>0)
                        {
                        groupBy = groupBy.Replace("group by DateID", string.Empty);
                        }
                      
                        break;
                    }
                case SummaryBy.Month:
                    {
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
                            case EntityType.API:
                            case EntityType.App:
                                {

                                
                                    populateReportSQL = string.Format(@"insert into report_data (Requests,Impressions,Revenue,Clicks{0}{1},DateID{2}) 
                                                            select  sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Revenue) as Revenue, sum(Clicks) as Clicks{3}",
                                                                                                                                                                                  args.Fieldname, idColumn, includeTime ? ",TimeId" : "", selectSql);
                                    duplicateKey = "on duplicate key update  Requests=IFNULL(Requests,0)+IFNULL(values(Requests),0), Impressions = IFNULL(Impressions,0) + IFNULL(values(Impressions),0), Clicks = IFNULL(Clicks,0) + IFNULL(values(Clicks),0), Revenue = IFNULL(Revenue,0) + IFNULL(values(Revenue),0)";
                                    filterBySql += string.Format(" And CampaignType={0} ", (int)args.Criteria.CampaignType);
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@"insert into report_data (Impressions,Spend,Clicks{0}{1},DateID{2}) 
                                                            select   sum(Impressions) as Impressions, sum(Spend) as Spend, sum(Clicks) as Clicks {3}",
                                            args.Fieldname, idColumn,includeTime ? ",TimeId" : "", selectSql);
                                    duplicateKey = "on duplicate key update  Impressions = IFNULL(Impressions,0) + IFNULL(values(Impressions),0), Clicks = IFNULL(Clicks,0) + IFNULL(values(Clicks),0), Spend = IFNULL(Spend,0) + IFNULL(values(Spend),0)";
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
            var selectCommand = string.Format("{0},{1}{2} from {3} where DateID in ({4})  and {5} ={6} {7} {8} {9} {10} {11};",
                              populateReportSQL, dateFieldName, includeTime ? ",TimeId as TimeId" : "", FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
                              appIDsSql, filterBySql, groupBy, orderBy, duplicateKey);
            return selectCommand;
        }
        private static string GenerateChartSelectCommand(string dates, ReportGeneratorArgs args, SummaryBy summaryBy = SummaryBy.Day)
        {
            const string orderBy = " order by NULL";

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
            //else
            //{
            //    appIDsSql = string.Format("AND {0} IN ({1})", args.DataFieldId, string.Format(args.SelectDataIds, args.AccountId));
            //}
            if ((!string.IsNullOrWhiteSpace(args.Fieldname)) &&
                (!string.IsNullOrWhiteSpace(args.Criteria.AdvancedCriteria)))
            {
                filterBySql = string.Format("AND {0} IN ({1})", args.Fieldname.Trim(','), args.Criteria.AdvancedCriteria);
            }

            switch (args.ChartCase)
            {
                case ChartCase.Hours:
                    {
                        FactStatTable = args.FactStatTable;
                        dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DateID), \"%Y-%m-%d\"), ' ', FSA.TimeId , \":00:00 \") AS Xaxis ";
                        break;
                    }
                case ChartCase.SixHours:
                    {
                        FactStatTable = args.FactStatTable;
                        dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DateID), \"%Y-%m-%d\"), ' ', (FSA.TimeId DIV  6)*6 , \":00:00 \") AS Xaxis  ";
                        break;
                    }
                case ChartCase.Day:
                    {
                        FactStatTable = args.DayFactStatTable;
                        dateFieldName = " cast(DateID as DATETIME) AS Xaxis ";
                        groupBy = string.Format(" group by DateID");
                        break;
                    }
                case ChartCase.Week:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = " cast(DATE_FORMAT(DateID - INTERVAL ( DATE_FORMAT(DateID, \'%w\') ) DAY , \'%Y%m%d\') as DATETIME) AS Xaxis ";
                        }
                        if (summaryBy == SummaryBy.Week)
                        {
                            FactStatTable = args.WeekFactStatTable;
                            dateFieldName = " cast(DateID as DATETIME) AS Xaxis ";
                            groupBy = string.Format(" group by DateID");
                        }
                        break;
                    }
                case ChartCase.Month:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = " cast(DATE_FORMAT(DateID , \'%Y%m01\') as DATETIME) AS Xaxis ";
                        }
                        if (summaryBy == SummaryBy.Month || summaryBy == SummaryBy.Accumulated)
                        {
                            FactStatTable = args.MonthFactStatTable;
                            dateFieldName = " cast(DateID as DATETIME) AS Xaxis ";
                            groupBy = string.Format(" group by DateID");
                        }
                        break;
                    }
            }


            populateReportSQL = string.Format(@"insert into report_data (Yaxis,Xaxis) 
                                                            select {0},{1}", GetChartSql(args), dateFieldName);

            var selectCommand = string.Format("{0} from {1} as FSA where DateID in ({2})  and {3} ={4} {5} {6} {7} {8};",
                                              populateReportSQL, FactStatTable, dates,
                                              args.AccountIdFieldName, args.AccountId,
                                              appIDsSql, filterBySql, groupBy, orderBy);
            return selectCommand;
        }

        private static string GenerateDropGroupByNameTempTaple(ReportGeneratorArgs args)
        {
            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                return "DROP TABLE IF EXISTS grouped_data;";
            }
            return string.Empty;
        }

        private static string GenerateDropTempTaple()
        {
            return "DROP TABLE IF EXISTS report_data;";
        }

        private static string GetChartSql(ReportGeneratorArgs args)
        {
            var str = string.Empty;
            switch (args.Criteria.MetricCode.ToLower())
            {
                case "revenue":
                    {
                        str = "SUM(Revenue)";
                        break;
                    }
                case "adrequests":
                    {
                        str = "cast(SUM(Requests) as SIGNED)";
                        break;
                    }
                case "adimpress":
                case "impress":
                    {
                        str = "cast(SUM(Impressions) as SIGNED)";
                        break;
                    }
                case "adclicks":
                case "clicks":
                    {
                        str = "cast(SUM(Clicks) as SIGNED)";
                        break;
                    }
                case "fillrate":
                    {
                        str = "SUM(Impressions)/SUM(Requests)";
                        break;
                    }
                case "ctr":
                case "campctr":
                    {
                        str = "SUM(Clicks)/SUM(Impressions)";
                        break;
                    }
                case "ecpm":
                    {
                        str = "((SUM(Revenue)/SUM(Impressions)) * 1000)";
                        break;
                    }
                case "avgcpc":
                    {
                        str = "SUM(Spend)/SUM(Clicks)";
                        break;
                    }
                case "spend":
                    {
                        str = "SUM(Spend)";
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


                switch (args.EntityType)
                {
                    case EntityType.API:
                        str = string.Format(
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
                            str = string.Format(
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
                            str = string.Format(
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

            if (args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select DateID,{0} from grouped_data group by DateID,{0}) counting", args.DataFieldNameAlias);
            }
            if (args.IsAccumulated && args.Criteria.GroupByName && args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                selectCommand = string.Format("(select {0} from grouped_data group by {0}) counting", args.DataFieldNameAlias);
            }

            return selectCommand;
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
