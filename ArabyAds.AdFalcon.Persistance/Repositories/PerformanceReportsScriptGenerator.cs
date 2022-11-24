using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    public enum GroupBy
    {
        Default = 0,
        Account = 1,
        Platform = 2
    }


    public delegate string GroupByCustomFieldJoin(PerformanceReportGeneratorArgs args);
    public delegate string GroupByCustomFieldSelect(PerformanceReportGeneratorArgs args);

    public class PerformanceReportGeneratorArgs : BaseGeneratorArgs
    {
        public BaseAppSitePerformanceDetailsCriteria Criteria { get; set; }

        public GroupBy GroupBy { get; set; }

        public string GroupByFieldId { get; set; }
        public string GroupByFieldName { get; set; }
        public string GroupByFieldNameAlias { get; set; }
        public string GroupByTableName { get; set; }
        public bool IsCustomGroupByFieldName { get; set; }
        public GroupByCustomFieldJoin GroupByCustomFieldJoin;
        public GroupByCustomFieldSelect GroupByCustomFieldSelect;
        public bool GetTotalMetric { get; set; }
        public string IndexName { get; set; }

        public static string DataBase = ArabyAds.AdFalcon.Domain.Configuration.DB;

        public static PerformanceReportGeneratorArgs GetInstance(BaseAppSitePerformanceDetailsCriteria criteria, ReportType reportType, EntityType entityType, GroupBy groupBy)
        {
            var instance = new PerformanceReportGeneratorArgs
            {
                Criteria = criteria,
                ReportType = reportType,
                EntityType = entityType,
                GroupBy = groupBy,
            };

            switch (entityType)
            {
                case EntityType.App:
                    {
                        switch (groupBy)
                        {
                            case GroupBy.Default:
                                instance.GroupByFieldId = "AppSiteId";
                                instance.GroupByFieldName = "Name";
                                instance.GroupByFieldNameAlias = "Name";
                                instance.GroupByTableName = DataBase + "appsite";
                                instance.IndexName = "account_date_app_appsitemetric";
                                break;
                            case GroupBy.Account:
                                instance.GroupByFieldId = "AccountId";
                                instance.GroupByTableName = DataBase + "account";
                                instance.IsCustomGroupByFieldName = true;
                                instance.GroupByCustomFieldJoin = new GroupByCustomFieldJoin(GroupByFieldQueryDelegates.GroupByFieldJoinForAccounts);
                                instance.GroupByCustomFieldSelect = new GroupByCustomFieldSelect(GroupByFieldQueryDelegates.GroupByFieldSelectForAccounts);
                                instance.IndexName = "account_date_app_appsitemetric";
                                break;
                            case GroupBy.Platform:
                                instance.GroupByFieldId = "PlatformId";
                                instance.GroupByTableName = DataBase + "platforms";
                                instance.IndexName = "account_date_app_appsitemetric";
                                instance.IsCustomGroupByFieldName = true;
                                instance.GroupByCustomFieldJoin = new GroupByCustomFieldJoin(GroupByFieldQueryDelegates.GroupByFieldJoinForPlatforms);
                                instance.GroupByCustomFieldSelect = new GroupByCustomFieldSelect(GroupByFieldQueryDelegates.GroupByFieldSelectForPlatforms);
                                break;
                        }

                        instance.FactStatTable = "fact_stat_app";
                        instance.DayFactStatTable = "fact_stat_app_day";
                        instance.WeekFactStatTable = "fact_stat_app_week";
                        instance.MonthFactStatTable = "fact_stat_app_month";

                        break;
                    }
                case EntityType.Campaign:
                case EntityType.AdGroup:
                case EntityType.Ad:
                case EntityType.API:
                    break;
            };

            return instance;
        }
    }

    /// <summary>
    /// Contain methods for custom final select queries for the reports
    /// </summary>
    class GroupByFieldQueryDelegates
    {
        internal static string GroupByFieldJoinForAccounts(PerformanceReportGeneratorArgs args)
        {
            string innerJoin = string.Format(@"INNER JOIN {0} as account ON FSA.{1} = {0}.Id
                                               INNER JOIN {2}users as users ON {0}.primaryuserid = users.Id ",
                                                args.GroupByTableName, args.GroupByFieldId, PerformanceReportGeneratorArgs.DataBase);
            return innerJoin;
        }

        internal static string GroupByFieldSelectForAccounts(PerformanceReportGeneratorArgs args)
        {
            string columnNameSQL = string.Format(@"FSA.{0} Id, case when company is null or company ='' then concat(users.FirstName,' ',users.LastName) 
                                                else company end  as Name, ", args.GroupByFieldId);
            return columnNameSQL;
        }

        internal static string GroupByFieldJoinForPlatforms(PerformanceReportGeneratorArgs args)
        {
            string innerJoin = string.Format(@"INNER JOIN {0} as platforms ON FSA.{1} = {0}.platformid
                                               LEFT OUTER JOIN {2}localizedstrings localized ON {0}.platformnameid = localized.LocalizedStringID and localized.Culture = '{3}'",
                                    args.GroupByTableName, args.GroupByFieldId, PerformanceReportGeneratorArgs.DataBase, args.Criteria.Culture);

            return innerJoin;
        }

        internal static string GroupByFieldSelectForPlatforms(PerformanceReportGeneratorArgs args)
        {
            string columnNameSQL = string.Format("FSA.{0} Id, Value AS Name, ", args.GroupByFieldId);
            return columnNameSQL;
        }
    }

    /// <summary>
    /// Generates performance reports scripts
    /// </summary>
    public class PerformanceReportsScriptGenerator
    {

        internal static string GenerateQueryScript(PerformanceReportGeneratorArgs args)
        {
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();

            reportCommand.AppendLine(GenerateTempTableCommand(args));
            reportCommand.AppendLine(GenerateInsertStatement(args));
            reportCommand.AppendLine(GenerateTempTableCountCommand());
            reportCommand.AppendLine(GenerateTotalMetricCommand(args));
            reportCommand.AppendLine(GenerateFinalSelectStatement(args));
            reportCommand.Append(GenerateDropTempTable());

            return reportCommand.ToString();
        }

   
        #region Helpers

        private static string GenerateTempTableCommand(PerformanceReportGeneratorArgs args)
        {
            var str = string.Empty;
            string timeIdColumn = string.Empty;
            string timeIdIndex = string.Empty;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {

                        switch (args.EntityType)
                        {

                            case EntityType.App:
                                {
                                    str = string.Format(
                                        @"DROP TABLE IF EXISTS performance_data;
                                        create temporary table performance_data (
                                        `{0}` int(11) NOT NULL,
                                        `Requests` bigint(11) DEFAULT '0',
                                        `Impressions` bigint(11) DEFAULT '0',
                                        `Clicks` bigint(11) DEFAULT '0',
                                        `Revenue` decimal(17,5) DEFAULT '0.00000',
                                         UNIQUE KEY unique_key_index({0}),
                                         Key sorting_index({1} {2})
                                        ) ENGINE=MyISAM;", args.GroupByFieldId,args.Criteria.OrderColumn,args.Criteria.OrderType);
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                            case EntityType.API:
                                break;

                        }
                        break;

                    }
                case ReportType.Chart:
                    break;
            }
            return str;
        }

        private static string GenerateInsertStatement(PerformanceReportGeneratorArgs args)
        {
            string selectStatement = GenerateSelectScript(args);

            string insertStatement = string.Empty;

            switch (args.EntityType)
            {
                case EntityType.App:
                    insertStatement = string.Format(@"insert into performance_data ({0},Requests,Impressions,Clicks,Revenue) 
                                                      {1}  
                                                    on duplicate key update  Requests=Requests+values(Requests), Impressions = Impressions + values(Impressions), Clicks = Clicks + values(Clicks), Revenue = Revenue + values(Revenue);
                                                    ", args.GroupByFieldId, selectStatement.ToString());
                    break;
                case EntityType.Campaign:
                    break;
                case EntityType.AdGroup:
                    break;
                case EntityType.Ad:
                case EntityType.API:
                    break;
                default:
                    break;
            }

            return insertStatement;
        }

        private static string GenerateSelectScript(PerformanceReportGeneratorArgs args)
        {
            string containerSelectStatement = string.Empty;
            string innerSelectStatement = GenerateInnerSelectStatement(args);

         
            switch (args.EntityType)
            {
                case EntityType.App:
                    {
                        containerSelectStatement = string.Format(@"select {0},sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Clicks) as Clicks,sum(Revenue) as Revenue 
                                                    from ( {1} ) as t
                                                    group by {2}
                                                    order by {3} {4}", args.GroupByFieldId,
                                                                        innerSelectStatement,
                                                                        args.GroupByFieldId,
                                                                        string.IsNullOrEmpty(args.Criteria.OrderColumn) ? "requests" : args.Criteria.OrderColumn,
                                                                        string.IsNullOrEmpty(args.Criteria.OrderType) ? "desc" : args.Criteria.OrderType);
                    }
                    break;
                case EntityType.Campaign:
                case EntityType.AdGroup:
                case EntityType.Ad:
                case EntityType.API:
                default:
                    break;
            }


            return containerSelectStatement;
        }

        private static string GenerateInnerSelectStatement(PerformanceReportGeneratorArgs args)
        {
            DateTime fromDate, toDate;
            fromDate = args.Criteria.FromDate;
            toDate = args.Criteria.ToDate;

            StringBuilder selectStatements = new StringBuilder();

            if ((fromDate.Day != 1) ||
                       (toDate.Month == (toDate.AddDays(1).Month)))
            {
                // Partial Dates
                //get partial Month dates
                DateTime newDateFrom, newDateTo;
                List<DateTime> partialDates = RepositoryScriptHelper.GetPartialDates(fromDate, toDate, SummaryBy.Month,
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
                    selectStatements.AppendLine(GenerateSingleSelectCommand(tableDays, SummaryBy.Day, args));
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
                    selectStatements.AppendLine(GenerateSingleSelectCommand(tableDays, SummaryBy.Month, args));
                }
            }

            string selectStatementsString = selectStatements.ToString();
            selectStatementsString = selectStatementsString.Substring(0, selectStatementsString.LastIndexOf("union"));


            return selectStatementsString;
        }

        private static string GenerateSingleSelectCommand(string dates, SummaryBy summaryBy, PerformanceReportGeneratorArgs args)
        {
            string factStatTable = string.Empty;
            StringBuilder whereStatement = new StringBuilder(string.Format("where DateID in ({0}) ", dates));

            string selectStatement = string.Empty;

            #region Building where statement

            string accountIdsCondition = string.Empty;
            string appsiteIdsCondition = string.Empty;
            string countryIdsCondition = string.Empty;
            string platformIdsCondition = string.Empty;
            string operatorIdsCondition = string.Empty;
            string deviceIdsCondition = string.Empty;
            string campaignTypeCondition = string.Empty;

            if (args.Criteria.AccountIds != null && args.Criteria.AccountIds.Count() != 0)
            {
                countryIdsCondition = string.Format("AND accountId in ({0}) ", string.Join(",", args.Criteria.AccountIds));
                whereStatement.AppendLine(countryIdsCondition);
            }

            if (args.Criteria.AppSiteIds != null && args.Criteria.AppSiteIds.Count() != 0)
            {
                appsiteIdsCondition = string.Format("AND appsiteid in ({0}) ", string.Join(",", args.Criteria.AppSiteIds));
                whereStatement.AppendLine(appsiteIdsCondition);
            }

            if (args.Criteria.CountryIds != null && args.Criteria.CountryIds.Count() != 0)
            {
                countryIdsCondition = string.Format("AND countryid in ({0}) ", string.Join(",", args.Criteria.CountryIds));
                whereStatement.AppendLine(countryIdsCondition);
            }

            if (args.Criteria.PlatformIds != null && args.Criteria.PlatformIds.Count() != 0)
            {
                platformIdsCondition = string.Format("AND platformid in ({0}) ", string.Join(",", args.Criteria.PlatformIds));
                whereStatement.AppendLine(platformIdsCondition);
            }

            if (args.Criteria.OperatorIds != null && args.Criteria.OperatorIds.Count() != 0)
            {
                operatorIdsCondition = string.Format("AND operatorId in ({0}) ", string.Join(",", args.Criteria.OperatorIds));
                whereStatement.AppendLine(operatorIdsCondition);
            }

            if (args.Criteria.DeviceIds != null && args.Criteria.DeviceIds.Count() != 0)
            {
                deviceIdsCondition = string.Format("AND deviceid in ({0}) ", string.Join(",", args.Criteria.DeviceIds));
                whereStatement.AppendLine(deviceIdsCondition);
            }

            campaignTypeCondition = string.Format("AND CampaignType = {0}", (int)args.Criteria.CampaignType);
            whereStatement.AppendLine(campaignTypeCondition);

            whereStatement.AppendLine(string.Format("AND {0} IS NOT NULL", args.GroupByFieldId));
            #endregion

            switch (summaryBy)
            {

                case SummaryBy.Day:
                    {
                        factStatTable = args.DayFactStatTable;
                        break;
                    }
                case SummaryBy.Week:
                    {
                        factStatTable = args.WeekFactStatTable;
                        break;
                    }
                case SummaryBy.Accumulated:
                case SummaryBy.Month:
                    {
                        factStatTable = args.MonthFactStatTable;
                        break;
                    }
            }

            string forceIndexName = args.IndexName;

            switch (args.ReportType)
            {
                case ReportType.Report:
                    {
                        selectStatement = string.Format(@"select {0},sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Clicks) as Clicks,sum(Revenue) as Revenue
                                                        from {1} 
                                                        {3}
                                                        group by {0} 
                                                        union ", args.GroupByFieldId, factStatTable,forceIndexName, whereStatement);
                        break;
                    }
                case ReportType.Chart:
                    break;
            }

            return selectStatement;
        }

        private static string GenerateTempTableCountCommand()
        {
            return string.Format(@"set @TotalCount=(SELECT count(1) FROM performance_data);");
        }

        private static string GenerateTotalMetricCommand(PerformanceReportGeneratorArgs args)
        {
            if (args.GetTotalMetric)
            {
                return string.Format(@"set @TotalMetricSum=(SELECT sum({0}) FROM performance_data);", args.Criteria.OrderColumn);
            }

            return "set @TotalMetricSum=0;";
        }

        private static string GenerateFinalSelectStatement(PerformanceReportGeneratorArgs args)
        {
            string finalSelectStatement = string.Empty;

            var dummyWhere = string.Format("WHERE  FSA.{0}>0", args.GroupByFieldId);

            var orderBy = string.Format("order by {0} {1}", args.Criteria.OrderColumn, args.Criteria.OrderType);

            string columnNameSQL, innerJoin;

            if (args.IsCustomGroupByFieldName)
            {
                columnNameSQL = args.GroupByCustomFieldSelect(args);
                innerJoin = args.GroupByCustomFieldJoin(args);
            }
            else
            {
                columnNameSQL = string.Format("FSA.{0} Id, {1}.{2} AS {3}, ", args.GroupByFieldId, args.GroupByTableName, args.GroupByFieldName, args.GroupByFieldNameAlias);
                innerJoin = string.Format("INNER JOIN {0}  ON FSA.{1} = {0}.Id", args.GroupByTableName, args.GroupByFieldId);
            }

            var paging = string.Format("LIMIT {0}, {1}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                    args.Criteria.ItemsPerPage);


            switch (args.EntityType)
            {

                case EntityType.App:
                    {
                        finalSelectStatement = string.Format(@"SELECT  {0} CAST(@TotalCount AS SIGNED) as TotalCount, 
                                                                       CAST(@TotalMetricSum AS SIGNED) as TotalMetricSum,
                                            CAST(Requests AS SIGNED) AS AdRequests,
                                            CAST(Impressions AS SIGNED) AS AdImpress,
                                             Revenue AS Revenue,
                                            CAST(Clicks AS SIGNED) AS Clicks
                                            FROM performance_data FSA force index(sorting_index)
                                            {1}
                                            {2}
                                            {3}
                                            {4};", columnNameSQL, innerJoin, dummyWhere, orderBy, paging);
                    }
                    break;
                case EntityType.API:
                case EntityType.Campaign:
                case EntityType.AdGroup:
                case EntityType.Ad:
                    break;
            }


            return finalSelectStatement;
        }

        private static string GenerateDropTempTable()
        {
            return "DROP TABLE IF EXISTS performance_data;";
        }

        #endregion
    }

 
}
