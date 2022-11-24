using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Services;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.ConfigurationSetting;

namespace Noqoush.AdFalcon.Persistence.Reports.Repositories
{
    public enum ChartCase
    {
        Hours, SixHours, Day, Week, Month

    }
    public enum ReportType
    {
        Report = 1, Chart = 2
    }
    public enum EntityType
    {
        App = 1,
        Campaign = 2,
        AdGroup = 3,
        Ad = 4
    }
    public enum SubType
    {
        None = 0,
        Operator = 1,
        Platform = 2,
        Manufacturer = 3,
        GeoLocation = 4

    }
    public class ReportGeneratorArgs
    {
        private static IConfigurationManager configurationManager;
        private static IAccountRepository accountRepository;
        private static IAccountStatistic accountStatistic;
        public const string DateField = "DimDateID";

        public ReportCriteriaDto Criteria { get; set; }
        public ChartCase ChartCase { get; set; }

        public ReportType ReportType { get; set; }
        public EntityType EntityType { get; set; }
        public SubType SubType { get; set; }

        public int Threshold { get; set; }
        public int Count { get; set; }

        //this data should be changed 
        public string AccountIdFieldName { get; set; }
        public string FactStatTable { get; set; } // "fact_stat_app";
        public string DayFactStatTable { get; set; } // "fact_stat_app_day";
        public string WeekFactStatTable { get; set; } // "fact_stat_app_week";
        public string MonthFactStatTable { get; set; } // "fact_stat_app_month";

        public int AccountId { get; set; }
        public string IdFieldName { get; set; }//"OperatorID"
        public string Fieldname { get; set; }//"DimOperatorID"
        public string TableIdName { get; set; }//"operators"
        public string LocalizedStringFieldName { get; set; }//"OperatorNameId"

        public string DataTableName { get; set; }//"appsite"
        public string FilterDataFieldId { get; set; }//"AppSiteID"
        public string DataFieldId { get; set; }//"AppSiteID"
        public string SelectDataIds { get; set; }//"AppSiteID"
        //public string SelectAllDataIds { get; set; }//"AppSiteID"
        public string DataFieldName { get; set; }//"Name"
        static ReportGeneratorArgs()
        {
            configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>();
            accountRepository = Framework.IoC.Instance.Resolve<IAccountRepository>();
            accountStatistic = Framework.IoC.Instance.Resolve<IAccountStatistic>();
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

        public static ReportGeneratorArgs GetInstance(ReportCriteriaDto criteria, int accountId, ReportType reportType, EntityType entityType, SubType subType)
        {

            // Get Counts
            int threshold, count;
            int.TryParse(configurationManager.GetConfigurationSetting(null, null, "Threshold"), out threshold);
            count = GetAccountEntityCount(entityType, accountId);

            var instance = new ReportGeneratorArgs
            {
                Criteria = criteria,
                AccountId = accountId,
                ReportType = reportType,
                EntityType = entityType,
                SubType = subType,
                Count = count,
                Threshold = threshold
            };
            switch (entityType)
            {
                case EntityType.App:
                    {
                        instance.DataTableName = "appsite";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_app";
                        instance.DayFactStatTable = "fact_stat_app_day";
                        instance.WeekFactStatTable = "fact_stat_app_week";
                        instance.MonthFactStatTable = "fact_stat_app_month";
                        instance.AccountIdFieldName = "AppAccountID";
                        instance.SelectDataIds = "{0}";
                        //instance.SelectAllDataIds = "select id from appsite where AccountId={0}";
                        break;
                    }
                case EntityType.Campaign:
                    {
                        instance.DataTableName = "campaigns";
                        instance.DataFieldId = "DimCampaignID";
                        instance.FilterDataFieldId = "AdsID";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from ads where AdGroupId in (select id from adgroups where CampaignId in ({0}))";
                        //instance.SelectAllDataIds = "select id from campaigns where AccountId={0}";
                        break;
                    }
                case EntityType.AdGroup:
                    {

                        instance.DataTableName = "adgroups";
                        instance.DataFieldId = "adsGroupID";
                        instance.FilterDataFieldId = "AdsID";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from ads where AdGroupId in ({0})";
                        /* instance.SelectAllDataIds = @"select adgroups.id FROM adgroups
 INNER JOIN campaigns ON adgroups.CampaignId = campaigns.Id
 where  AccountId={0}";*/
                        break;
                    }
                case EntityType.Ad:
                    {
                        instance.DataTableName = "ads";
                        instance.DataFieldId = "adsID";
                        instance.FilterDataFieldId = "AdsID";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "{0}";
                        /* instance.SelectAllDataIds = @"SELECT ads.Id FROM ads INNER JOIN adgroups ON ads.AdGroupId = adgroups.Id INNER JOIN campaigns on adgroups.CampaignId = campaigns.Id
 WHERE campaigns.AccountId ={0}";*/
                        break;
                    }
            }

            switch (subType)
            {
                case SubType.Operator:
                    {
                        instance.TableIdName = "operators";
                        instance.Fieldname = ",DimOperatorID";
                        instance.IdFieldName = "OperatorID";
                        instance.LocalizedStringFieldName = "OperatorNameId";
                        break;
                    }
                case SubType.Platform:
                    {
                        instance.TableIdName = "platforms";
                        instance.Fieldname = ",DimPlatformID";
                        instance.IdFieldName = "PlatformId";
                        instance.LocalizedStringFieldName = "PlatformNameId";
                        break;
                    }
                case SubType.Manufacturer:
                    {
                        instance.TableIdName = "manufacturers";
                        instance.Fieldname = ",DimManufacturerID";
                        instance.IdFieldName = "ManufacturerId";
                        instance.LocalizedStringFieldName = "NameId";
                        break;
                    }
                case SubType.GeoLocation:
                    {
                        instance.TableIdName = "locations";
                        instance.Fieldname = ",DimCountryID";
                        instance.IdFieldName = "Id";
                        instance.LocalizedStringFieldName = "NameId";
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
        public static string GenerateSelectScript(ReportGeneratorArgs args)
        {
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();
            //get generate temp table command
            reportCommand.AppendLine(GenerateTempTableCommand(args));

            string partialDates;
            switch (args.Criteria.SummaryBy)
            {
                case 0: //None
                    {
                        reportCommand.AppendLine(GenerateSelectCommand(GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day), SummaryBy.Day, args));
                        break;
                    }
                case 1: //day
                    {
                        reportCommand.AppendLine(GenerateSelectCommand(GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day), SummaryBy.Day, args));
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
                            partialDates = GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy, out newDateFrom,
                                                           out newDateTo);
                            if (string.IsNullOrWhiteSpace(partialDates))
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            reportCommand.AppendLine(GenerateSelectCommand(partialDates, SummaryBy.Day, args));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            partialDates = GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            if (!string.IsNullOrWhiteSpace(partialDates))
                            {
                                reportCommand.AppendLine(GenerateSelectCommand(partialDates, SummaryBy.Week, args));
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
                            partialDates = GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy, out newDateFrom,
                                                           out newDateTo);
                            if (string.IsNullOrWhiteSpace(partialDates))
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            reportCommand.AppendLine(GenerateSelectCommand(partialDates, SummaryBy.Day, args));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            partialDates = GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            if (!string.IsNullOrWhiteSpace(partialDates))
                            {
                                reportCommand.AppendLine(GenerateSelectCommand(partialDates, SummaryBy.Month, args));
                            }
                        }
                        break;
                    }
            }


            reportCommand.AppendLine(GenerateTempTableCountCommand(GenerateSelectCountCommand(args)));
            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            reportCommand.AppendLine("DROP TABLE IF EXISTS report_data;");
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
            var chartCase = ChartCase.Hours;
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();
            var dateDiff = args.Criteria.ToDate.Subtract(args.Criteria.FromDate).TotalDays;

            //get generate temp table command
            reportCommand.AppendLine(GenerateTempTableCommand(args));

            string partialDates;
            if (dateDiff <= 1)
            {
                args.ChartCase = ChartCase.Hours;
            }
            if ((dateDiff > 1) && (dateDiff <= 7))
            {
                args.ChartCase = ChartCase.SixHours;
            }
            if ((dateDiff > 7) && (dateDiff <= 122))
            {
                args.ChartCase = ChartCase.Day;
            }
            if ((dateDiff > 122) && (dateDiff <= 180))
            {
                args.ChartCase = ChartCase.Week;
            }
            if (dateDiff > 180)
            {
                args.ChartCase = ChartCase.Month;
            }
            switch (args.ChartCase)
            {
                case ChartCase.Hours: //Hours
                    {
                        reportCommand.AppendLine(GenerateChartSelectCommand(args.Criteria.FromDate.ToString("yyyyMMdd"), args));
                        break;
                    }
                case ChartCase.SixHours: //day
                    {
                        reportCommand.AppendLine(GenerateChartSelectCommand(GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day), args));
                        break;
                    }
                case ChartCase.Day: //Day
                    {

                        reportCommand.AppendLine(GenerateChartSelectCommand(GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day), args));
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
                            partialDates = GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week, out newDateFrom,
                                                           out newDateTo);
                            if (string.IsNullOrWhiteSpace(partialDates))
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            reportCommand.AppendLine(GenerateChartSelectCommand(partialDates, args, SummaryBy.Day));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }
                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            partialDates = GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            if (!string.IsNullOrWhiteSpace(partialDates))
                            {
                                reportCommand.AppendLine(GenerateChartSelectCommand(partialDates, args, SummaryBy.Week));
                            }
                        }
                        break;
                    }

                case ChartCase.Month: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month, out newDateFrom,
                                                           out newDateTo);
                            if (string.IsNullOrWhiteSpace(partialDates))
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            reportCommand.AppendLine(GenerateChartSelectCommand(partialDates, args, SummaryBy.Day));
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            partialDates = GetDays(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            if (!string.IsNullOrWhiteSpace(partialDates))
                            {
                                reportCommand.AppendLine(GenerateChartSelectCommand(partialDates, args, SummaryBy.Month));
                            }
                        }
                        break;
                    }
            }


            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            reportCommand.AppendLine("DROP TABLE IF EXISTS report_data;");
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString();
        }

        private static string GenerateTempTableCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;


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
                        switch (args.EntityType)
                        {
                            case EntityType.App:
                                {
                                    str = string.Format(
@"DROP TABLE IF EXISTS report_data;
create temporary table report_data (
`DimDateID` int(11) NOT NULL,{0}
`Request` int(11) DEFAULT '0',
`Impression` int(11) DEFAULT '0',
`Clicks` int(11) DEFAULT '0',
`Revenue` decimal(17,5) DEFAULT '0.00000',{1}
 UNIQUE KEY operator_by_day(DimDateID{2})
) ENGINE=MyISAM;", filterByColumn, idColumn, key);
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    str = string.Format(
@"DROP TABLE IF EXISTS report_data;
create temporary table report_data (
`DimDateID` int(11) NOT NULL,{0}
`Impression` int(11) DEFAULT '0',
`Clicks` int(11) DEFAULT '0',
`Spend` decimal(17,5) DEFAULT '0.00000',{1}
UNIQUE KEY operator_by_day(DimDateID{2})
) ENGINE=MyISAM;", filterByColumn, idColumn, key);
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
`Yaxis` decimal(17,5) ,
PRIMARY KEY (`Id`)
) ENGINE=MyISAM;");
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

        private static string GenerateReportTempTableSelectCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;
            var innerJoin = string.Empty;
            var columnNameSQL = string.Format("{0} AS Date,", ReportGeneratorArgs.DateField);
            //  use this dummy where to force using the index
            var dummyWhere = " WHERE  DimDateID>0 ";
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                columnNameSQL += string.Format(" {0}.{1} AS SubName, ", args.DataTableName, args.DataFieldName);
                innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.DataTableName, args.DataFieldId);
            }

            var outerJoin = string.Empty;

            if (!string.IsNullOrWhiteSpace(args.Fieldname))
            {
                columnNameSQL += "Value AS Name,";
                outerJoin = string.Format(
                    @"LEFT OUTER JOIN  {0} O ON O.{1} = FSA.{2}
LEFT OUTER JOIN localizedstrings Locs
ON {3} = Locs.LocalizedStringID
WHERE (isnull(Locs.Culture) or Locs.Culture ='{4}') ",
                    args.TableIdName, args.IdFieldName,
                    args.Fieldname.Trim(','), args.LocalizedStringFieldName,
                    args.Criteria.Culture);
                dummyWhere = " AND  DimDateID>0 ";
            }

            //string.Format("ORDER BY {0} {1}", args.Criteria.OrderColumn, args.Criteria.OrderType);
            var paging = string.Format("LIMIT {0}, {1}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                       args.Criteria.ItemsPerPage);



            switch (args.EntityType)
            {
                case EntityType.App:
                    {
                        str = string.Format(
                             @"SELECT {0} @0 as TotalCount,
 CAST(Request AS SIGNED) AS AdRequests,
 CAST(Impression AS SIGNED) AS AdImpress,
 Revenue AS Revenue,
 CAST(Clicks AS SIGNED) AS AdClicks
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
                           @"SELECT {0} @0 as TotalCount,
CAST(Impression AS SIGNED) AS Impress,
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

            return str;
        }

        private static string GenerateSelectCountCommand(ReportGeneratorArgs args)
        {
            var selectCommand = "report_data";
            /*
            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
                if (!args.DataFieldId.Equals(args.FilterDataFieldId, StringComparison.OrdinalIgnoreCase))
                {
                    selectCommand = string.Format("((SELECT 1 as c FROM report_data  group by DimDateID,{0}{1} )as temp)", args.DataFieldId, args.Fieldname);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(args.Fieldname))
                {
                    selectCommand = string.Format("((SELECT 1 as c FROM report_data  group by DimDateID{0} )as temp)", args.Fieldname);
                }
            }*/
            return selectCommand;
        }
        private static string GenerateTempTableCountCommand(string selectCommand)
        {
            return string.Format(@"set @0=(SELECT count(1) FROM  {0});", selectCommand);
        }

        private static string GenerateSelectCommand(string dates, SummaryBy summaryBy, ReportGeneratorArgs args)
        {
            const string orderBy = " order by NULL";

            var appIDsSql = string.Empty;
            var filterBySql = string.Empty;
            var groupBy = string.Format(" group by DimDateID");
            var groupByFields = string.Empty;
            var FactStatTable = string.Empty;
            var dateFieldName = "DimDateID";
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
                    groupByFields = string.Format(",{0}", args.FilterDataFieldId);
                    groupBy = string.Format(" group by DimDateID{0}", groupByFields);
                }
            }
            else
            {
                groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DimDateID{0}", groupByFields);
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
                            case 0: //None
                                {
                                    dateFieldName = "DATE_FORMAT(DimDateID - INTERVAL ( DATE_FORMAT(DimDateID, \'%w\') ) DAY , \'%Y%m%d\') as newDimDateID";
                                    groupBy = string.Format(" group by {0}", groupByFields.Trim(','));  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "DATE_FORMAT(DimDateID - INTERVAL ( DATE_FORMAT(DimDateID, \'%w\') ) DAY , \'%Y%m%d\') as newDimDateID";
                                    groupBy = string.Format(" group by newDimDateID{0}", groupByFields);
                                    break;
                                }
                            case 3: //month
                                {
                                    dateFieldName = "DATE_FORMAT(DimDateID , \'%Y%m01\') as newDimDateID";
                                    groupBy = string.Format(" group by newDimDateID{0}", groupByFields);
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
                            case EntityType.App:
                                {
                                    populateReportSQL = string.Format(@"insert into report_data (Request,Impression,Revenue,Clicks{0}{1},DimDateID) 
                                                            select sum(Request) as Request, sum(Impression) as Impression, sum(Revenue) as Revenue, sum(Clicks) as Clicks{2}",
                                            args.Fieldname, idColumn, selectSql);
                                    duplicateKey = "on duplicate key update  Request=Request+values(Request), Impression = Impression + values(Impression), Clicks = Clicks + values(Clicks), Revenue = Revenue + values(Revenue)";
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@"insert into report_data (Impression,Spend,Clicks{0}{1},DimDateID) 
                                                            select  sum(Impression) as Impression, sum(AdsSpend) as Spend, sum(Clicks) as Clicks {2}",
                                            args.Fieldname, idColumn, selectSql);
                                    duplicateKey = "on duplicate key update  Impression = Impression + values(Impression), Clicks = Clicks + values(Clicks), Spend = Spend + values(Spend)";
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
            var selectCommand = string.Format("{0},{1} from {2} where DimDateID in ({3})  and {4} ={5} {6} {7} {8} {9} {10};",
                              populateReportSQL, dateFieldName, FactStatTable, dates, args.AccountIdFieldName, args.AccountId,
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
            var dateFieldName = "DimDateID";
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
                        dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DimDateID), \"%Y-%m-%d\"), ' ', FSA.DimTimeKey , \":00:00 \") AS Xaxis ";
                        break;
                    }
                case ChartCase.SixHours:
                    {
                        FactStatTable = args.FactStatTable;
                        dateFieldName = " CONCAT(DATE_FORMAT(CONCAT(FSA.DimDateID), \"%Y-%m-%d\"), ' ', (FSA.DimTimeKey DIV  6)*6 , \":00:00 \") AS Xaxis  ";
                        break;
                    }
                case ChartCase.Day:
                    {
                        FactStatTable = args.DayFactStatTable;
                        dateFieldName = " cast(DimDateID as DATETIME) AS Xaxis ";
                        groupBy = string.Format(" group by DimDateID");
                        break;
                    }
                case ChartCase.Week:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = " cast(DATE_FORMAT(DimDateID - INTERVAL ( DATE_FORMAT(DimDateID, \'%w\') ) DAY , \'%Y%m%d\') as DATETIME) AS Xaxis ";
                        }
                        if (summaryBy == SummaryBy.Week)
                        {
                            FactStatTable = args.WeekFactStatTable;
                            dateFieldName = " cast(DimDateID as DATETIME) AS Xaxis ";
                            groupBy = string.Format(" group by DimDateID");
                        }
                        break;
                    }
                case ChartCase.Month:
                    {
                        if (summaryBy == SummaryBy.Day)
                        {
                            FactStatTable = args.DayFactStatTable;
                            dateFieldName = " cast(DATE_FORMAT(DimDateID , \'%Y%m01\') as DATETIME) AS Xaxis ";
                        }
                        if (summaryBy == SummaryBy.Month)
                        {
                            FactStatTable = args.MonthFactStatTable;
                            dateFieldName = " cast(DimDateID as DATETIME) AS Xaxis ";
                            groupBy = string.Format(" group by DimDateID");
                        }
                        break;
                    }
            }


            populateReportSQL = string.Format(@"insert into report_data (Yaxis,Xaxis) 
                                                            select {0},{1}", GetChartSql(args), dateFieldName);

            var selectCommand = string.Format("{0} from {1} as FSA where DimDateID in ({2})  and {3} ={4} {5} {6} {7} {8};",
                                              populateReportSQL, FactStatTable, dates,
                                              args.AccountIdFieldName, args.AccountId,
                                              appIDsSql, filterBySql, groupBy, orderBy);
            return selectCommand;
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
                        str = "cast(SUM(Request) as SIGNED)";
                        break;
                    }
                case "adimpress":
                case "impress":
                    {
                        str = "cast(SUM(Impression) as SIGNED)";
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
                        str = "SUM(Impression)/SUM(Request)";
                        break;
                    }
                case "ctr":
                case "campctr":
                    {
                        str = "SUM(Clicks)/SUM(Impression)";
                        break;
                    }
                case "ecpm":
                    {
                        str = "((SUM(Revenue)/SUM(Impression)) * 1000)";
                        break;
                    }
                case "avgcpc":
                    {
                        str = "SUM(AdsSpend)/SUM(Clicks)";
                        break;
                    }
                case "spend":
                    {
                        str = "SUM(adsspend)";
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
    }



}
