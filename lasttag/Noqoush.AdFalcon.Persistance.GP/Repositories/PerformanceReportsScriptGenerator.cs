using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Reports.RepositoriesGP
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
        public string PerformanceTableName { get; set; }
        public string GroupByFieldId { get; set; }
        public string GroupByFieldName { get; set; }
        public string GroupByFieldNameAlias { get; set; }
        public string GroupByTableName { get; set; }
        public bool IsCustomGroupByFieldName { get; set; }
        public GroupByCustomFieldJoin GroupByCustomFieldJoin;
        public GroupByCustomFieldSelect GroupByCustomFieldSelect;
        public bool GetTotalMetric { get; set; }
        public string IndexName { get; set; }

        public static string DataBase = Noqoush.AdFalcon.Domain.Configuration.DB;

        public static PerformanceReportGeneratorArgs GetInstance(BaseAppSitePerformanceDetailsCriteria criteria, ReportType reportType, EntityType entityType, GroupBy groupBy)
        {
            var instance = new PerformanceReportGeneratorArgs
            {
                Criteria = criteria,
                ReportType = reportType,
                EntityType = entityType,
                PerformanceTableName = "performance_data" + DateTime.Now.Ticks.ToString(),
              
                GroupBy = groupBy,
            };
            instance.DropStatements = "DROP TABLE IF EXISTS " + instance.PerformanceTableName+";";
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
                                instance.GroupByTableName = "dim_appsites";
                                instance.IndexName = "";
                                break;
                            case GroupBy.Account:
                                instance.GroupByFieldId = "pubAccountId";
                                instance.GroupByTableName = "dim_accounts";
                                instance.IsCustomGroupByFieldName = true;
                                instance.GroupByCustomFieldJoin = new GroupByCustomFieldJoin(GroupByFieldQueryDelegates.GroupByFieldJoinForAccounts);
                                instance.GroupByCustomFieldSelect = new GroupByCustomFieldSelect(GroupByFieldQueryDelegates.GroupByFieldSelectForAccounts);
                                instance.IndexName = "";
                                break;
                            case GroupBy.Platform:
                                instance.GroupByFieldId = "deviceosId";
                                instance.GroupByTableName =  "dim_platforms";
                                instance.IndexName = "";
                                instance.IsCustomGroupByFieldName = true;
                                instance.GroupByCustomFieldJoin = new GroupByCustomFieldJoin(GroupByFieldQueryDelegates.GroupByFieldJoinForPlatforms);
                                instance.GroupByCustomFieldSelect = new GroupByCustomFieldSelect(GroupByFieldQueryDelegates.GroupByFieldSelectForPlatforms);
                                break;
                        }

                        instance.FactStatTable = "fact_stat_h";
                        instance.DayFactStatTable = "fact_stat_d";
                        instance.WeekFactStatTable = "fact_stat_w";
                        instance.MonthFactStatTable = "fact_stat_m";

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
            string innerJoin = string.Format(@"INNER JOIN {0} as account ON FSA.{1} = account.Id
                                               INNER JOIN dim_users as users ON account.primaryuserid = users.Id ",
                                                args.GroupByTableName, args.GroupByFieldId, PerformanceReportGeneratorArgs.DataBase);
            return innerJoin;
        }

        internal static string GroupByFieldSelectForAccounts(PerformanceReportGeneratorArgs args)
        {
            string columnNameSQL = string.Format(@"FSA.{0} Id, case when company is null or company ='' then users.FirstName||' '|| users.LastName 
                                                else company end  as Name, ", args.GroupByFieldId);
            return columnNameSQL;
        }

        internal static string GroupByFieldJoinForPlatforms(PerformanceReportGeneratorArgs args)
        {
            string innerJoin = string.Format(@"INNER JOIN {0} as platforms ON FSA.{1} = platforms.Id
                                               ",
                                    args.GroupByTableName, args.GroupByFieldId, PerformanceReportGeneratorArgs.DataBase, args.Criteria.Culture);

            return innerJoin;
        }

        internal static string GroupByFieldSelectForPlatforms(PerformanceReportGeneratorArgs args)
        {
            string localized = string.Empty;
            if (args.Criteria.Culture.Contains("en"))
                localized = "platform_en AS Name";
            else
                localized =  "platform_ar AS Name";
            string columnNameSQL = string.Format("FSA.{0} Id, {1}, ", args.GroupByFieldId, localized);

       
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
           // reportCommand.AppendLine(GenerateTempTableCountCommand());
           // reportCommand.AppendLine(GenerateTotalMetricCommand(args));
            reportCommand.AppendLine(GenerateFinalSelectStatement(args));
           // reportCommand.Append(GenerateDropTempTable());

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
                                        @"DROP TABLE IF EXISTS {3};
                                        create temporary table {3} (
                                        {0} int NOT NULL,
                                        Requests bigint DEFAULT '0',
                                        Impressions bigint DEFAULT '0',
                                        Clicks bigint DEFAULT '0',
                                        Revenue decimal(21,12) DEFAULT '0.00000',
                                         CONSTRAINT unique_key_index UNIQUE({0})
                                        
                                        ); ", args.GroupByFieldId,args.Criteria.OrderColumn,args.Criteria.OrderType,args.PerformanceTableName);
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
                    insertStatement = string.Format(@"insert into {2} ({0},Requests,Impressions,Clicks,Revenue) 
                                                      {1}  
                                                    ;
                                                    ", args.GroupByFieldId, selectStatement.ToString(),args.PerformanceTableName);
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
                        containerSelectStatement = string.Format(@"select {0},sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Clicks) as Clicks,sum(revenue) as Revenue 
                                                    from ( {1} ) as t
                                                    group by {2}
                                                     ", args.GroupByFieldId,
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
                //foreach (var table in tableInfo)
                //{
                //    var tableName = table.Key;
                //    var tableDays = table.Value;
                //    args.DayFactStatTable = tableName;
                  
                //}
                selectStatements.AppendLine(GenerateSingleSelectCommand(tableInfo[args.DayFactStatTable], SummaryBy.Day, args));
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
                if(tableInfo!=null && tableInfo.Count>0)
                selectStatements.AppendLine(GenerateSingleSelectCommand(tableInfo[args.MonthFactStatTable], SummaryBy.Month, args));
            }

            string selectStatementsString = selectStatements.ToString();
            selectStatementsString = selectStatementsString.Substring(0, selectStatementsString.LastIndexOf("union All"));


            return selectStatementsString;
        }

        private static string GenerateSingleSelectCommand(string dates, SummaryBy summaryBy, PerformanceReportGeneratorArgs args)
        {
            string factStatTable = string.Empty;
            StringBuilder whereStatement ;
            if(summaryBy== SummaryBy.Day)
             whereStatement = new StringBuilder(string.Format("where dateid in ({0}) ", dates));
            else
                whereStatement = new StringBuilder(string.Format("where monthid in ({0}) ", dates));
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
                countryIdsCondition = string.Format("AND pubaccountId in ({0}) ", string.Join(",", args.Criteria.AccountIds));
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
                platformIdsCondition = string.Format("AND deviceosid in ({0}) ", string.Join(",", args.Criteria.PlatformIds));
                whereStatement.AppendLine(platformIdsCondition);
            }

            if (args.Criteria.OperatorIds != null && args.Criteria.OperatorIds.Count() != 0)
            {
                operatorIdsCondition = string.Format("AND mobileoperatorid in ({0}) ", string.Join(",", args.Criteria.OperatorIds));
                whereStatement.AppendLine(operatorIdsCondition);
            }

            if (args.Criteria.DeviceIds != null && args.Criteria.DeviceIds.Count() != 0)
            {
                deviceIdsCondition = string.Format("AND devicemodelid in ({0}) ", string.Join(",", args.Criteria.DeviceIds));
                whereStatement.AppendLine(deviceIdsCondition);
            }

            campaignTypeCondition = string.Format("  And   campaigntype!={0}  ", (int)args.Criteria.NotInCampaignType);
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
                        selectStatement = string.Format(@"select {0},sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Clicks) as Clicks,sum(appsiterevenue) as Revenue
                                                        from {1}  
                                                        {3}
                                                        group by {0} 
                                                        union All ", args.GroupByFieldId, factStatTable,forceIndexName, whereStatement);
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

            var orderBy = string.Format("order by {0} {1} NULLS Last", args.Criteria.OrderColumn, args.Criteria.OrderType);

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

            var paging = string.Format("LIMIT  {1}  OFFSET {0} ", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                    args.Criteria.ItemsPerPage);


            switch (args.EntityType)
            {

                case EntityType.App:
                    {
                        //if (args.GetTotalMetric)
                        //    finalSelectStatement = string.Format(@"SELECT  {0} (SELECT reltuples::int AS estimate FROM pg_class where relname='{6}') as TotalCount, 
                        //                                               (SELECT sum({5}) FROM {6}) as TotalMetricSum,
                        //                    CAST(Requests AS int) AS AdRequests,
                        //                    CAST(Impressions AS int) AS AdImpress,
                        //                     Revenue AS Revenue,
                        //                    CAST(Clicks AS int) AS Clicks
                        //                    FROM {6} FSA 
                        //                    {1}
                        //                    {2}
                        //                    {3}
                        //                    {4};", columnNameSQL, innerJoin, dummyWhere, orderBy, paging, args.Criteria.OrderColumn,args.PerformanceTableName);

                        //else

                            finalSelectStatement = string.Format(@"SELECT  {0} (SELECT reltuples::int AS estimate FROM pg_class where relname='{6}') as TotalCount, 
                                                                      0 as TotalMetricSum,
                                            CAST(Requests AS bigint) AS AdRequests,
                                            CAST(Impressions AS bigint) AS AdImpress,
                                             Revenue AS Revenue,
                                            CAST(Clicks AS bigint) AS Clicks
                                            FROM {6} FSA 
                                            {1}
                                            {2}
                                            {3}
                                            {4};", columnNameSQL, innerJoin, dummyWhere, orderBy, paging, args.Criteria.OrderColumn, args.PerformanceTableName);
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
