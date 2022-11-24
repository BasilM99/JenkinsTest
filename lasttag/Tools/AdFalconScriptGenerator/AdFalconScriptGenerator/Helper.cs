using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdFalconScriptGenerator
{
    public enum ChartCase
    {
        Hours, SixHours, Day, Week, Month

    }
    public enum Type
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
        public const string DateField = "DateID";
        public const string DataBase = "adfalcon.";

        public ReportCriteriaDto Criteria { get; set; }
        public ChartCase ChartCase { get; set; }

        public Type Type { get; set; }
        public EntityType EntityType { get; set; }
        public SubType SubType { get; set; }

        public int Threshold { get; set; }
        public int Count { get; set; }

        //this data should be changed 

        public string AccountIdFieldName { get; set; }
        public string FactStatTable { get; set; } // "fact_stat_app";
        public string DayFactStatTable { get; set; } // "fact_stat_app_day2";
        public string WeekFactStatTable { get; set; } // "fact_stat_app_week2";
        public string MonthFactStatTable { get; set; } // "fact_stat_app_month2";

        public int AccountId { get; set; }
        public string IdFieldName { get; set; }//"OperatorID"
        public string Fieldname { get; set; }//"OperatorID"
        public string TableIdName { get; set; }//"operators"
        public string LocalizedStringFieldName { get; set; }//"OperatorNameId"

        public string DataTableName { get; set; }//"appsite"
        public string FilterDataFieldId { get; set; }//"AppSiteID"
        public string DataFieldId { get; set; }//"AppSiteID"
        public string SelectDataIds { get; set; }//"AppSiteID"
        public string SelectAllDataIds { get; set; }//"AppSiteID"
        public string DataFieldName { get; set; }//"Name"

        //only for this test app
        public string AccountIds { get; set; }//"Name"

        public static ReportGeneratorArgs GetInstance(ReportCriteriaDto criteria, int accountId, Type type, EntityType entityType, SubType subType, int count, int threshold, string accountIds)
        {
            var instance = new ReportGeneratorArgs
            {
                Criteria = criteria,
                AccountId = accountId,
                Type = type,
                EntityType = entityType,
                SubType = subType,
                Count = count,
                Threshold = threshold,
                AccountIds = accountIds
            };
            switch (entityType)
            {
                case EntityType.App:
                    {
                        instance.DataTableName = DataBase + "appsite";
                        instance.DataFieldId = "AppSiteID";
                        instance.FilterDataFieldId = "AppSiteID";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_app";
                        instance.DayFactStatTable = "fact_stat_app_day";
                        instance.WeekFactStatTable = "fact_stat_app_week";
                        instance.MonthFactStatTable = "fact_stat_app_month";
                        instance.AccountIdFieldName = "AccountID";
                        instance.SelectDataIds = "{0}";
                        instance.SelectAllDataIds = "select id from " + DataBase + "appsite where AccountId={0}";
                        break;
                    }
                case EntityType.Campaign:
                    {
                        instance.DataTableName = DataBase + "campaigns";
                        instance.DataFieldId = "CampaignID";
                        instance.FilterDataFieldId = "AdId";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from " + DataBase + "ads where AdGroupId in (select id from " + DataBase + "adgroups where CampaignId in ({0}))";
                        instance.SelectAllDataIds = "select id from " + DataBase + "campaigns where AccountId={0}";
                        break;
                    }
                case EntityType.AdGroup:
                    {

                        instance.DataTableName = DataBase + "adgroups";
                        instance.DataFieldId = "adsGroupID";
                        instance.FilterDataFieldId = "AdId";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign2";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "select id from " + DataBase + "ads where AdGroupId in ({0})";
                        instance.SelectAllDataIds = @"select " + DataBase + "adgroups.id FROM " + DataBase + "adgroups INNER JOIN " + DataBase + "campaigns ON " + DataBase + "adgroups.CampaignId = " + DataBase + "campaigns.Id where  AccountId={0}";
                        break;
                    }
                case EntityType.Ad:
                    {
                        instance.DataTableName = DataBase + "ads";
                        instance.DataFieldId = "AdId";
                        instance.FilterDataFieldId = "AdId";
                        instance.DataFieldName = "Name";
                        instance.FactStatTable = "fact_stat_campaign2";
                        instance.DayFactStatTable = "fact_stat_campaign_day";
                        instance.WeekFactStatTable = "fact_stat_campaign_week";
                        instance.MonthFactStatTable = "fact_stat_campaign_month";
                        instance.AccountIdFieldName = "accountId";
                        instance.SelectDataIds = "{0}";
                        instance.SelectAllDataIds = @"SELECT " + DataBase + "ads.Id FROM " + DataBase + "ads INNER JOIN " + DataBase + "adgroups ON " + DataBase + "ads.AdGroupId = " + DataBase + "adgroups.Id INNER JOIN " + DataBase + "campaigns on " + DataBase + "adgroups.CampaignId = campaigns.Id WHERE " + DataBase + "campaigns.AccountId ={0}";
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
                        break;
                    }
            }

            if ((string.IsNullOrWhiteSpace(instance.Criteria.ItemsList)) && (instance.Count < instance.Threshold))
            {
                instance.Criteria.ItemsList = instance.AccountIds;//string.Format(instance.SelectAllDataIds, instance.AccountId.ToString());
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
            List<DateTime> partialDates;
            switch (args.Criteria.SummaryBy)
            {
                case 0: //Hour
                    {
                        var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = GetDateTables(args, dates, SummaryBy.Day);
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
                        var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = GetDateTables(args, dates, SummaryBy.Day);
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
                            partialDates = GetPartialDates(args, (SummaryBy)args.Criteria.SummaryBy,
                                                               out newDateFrom, out newDateTo);


                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = GetDateTables(args, partialDates, SummaryBy.Day);
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
                            var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            var tableInfo = GetDateTables(args, dates, SummaryBy.Week);
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

                case 3: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = GetPartialDates(args, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);


                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = GetDateTables(args, partialDates, SummaryBy.Day);
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
                            var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = GetDateTables(args, dates, SummaryBy.Month);
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


            reportCommand.AppendLine(GenerateTempTableCountCommand(GenerateSelectCountCommand(args)));
            reportCommand.AppendLine(GenerateTempTableSelectCommand(args));
            reportCommand.AppendLine("DROP TABLE IF EXISTS report_data;");
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString();
        }
        public static string GenerateChartSelectScript(ReportGeneratorArgs args)
        {
            if (args.Type != Type.Chart)
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
                        var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = GetDateTables(args, dates, SummaryBy.Hour);
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
                        var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Hour);
                        var tableInfo = GetDateTables(args, dates, SummaryBy.Hour);
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

                        var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Day);
                        var tableInfo = GetDateTables(args, dates, SummaryBy.Day);
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
                            partialDates = GetPartialDates(args, SummaryBy.Week,out newDateFrom, out newDateTo);


                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = GetDateTables(args, partialDates, SummaryBy.Day);
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
                            var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            var tableInfo = GetDateTables(args, dates, SummaryBy.Week);
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
                            partialDates = GetPartialDates(args, SummaryBy.Month,
                                                               out newDateFrom, out newDateTo);

                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = GetDateTables(args, partialDates, SummaryBy.Day);
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
                            var dates = GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = GetDateTables(args, dates, SummaryBy.Month);
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
            reportCommand.AppendLine("DROP TABLE IF EXISTS report_data;");
            args.Criteria.FromDate = oldFromDate;
            args.Criteria.ToDate = oldToDate;
            return reportCommand.ToString();
        }

        private static string GenerateTempTableCommand(ReportGeneratorArgs args)
        {
            var str = string.Empty;


            switch (args.Type)
            {
                case Type.Report:
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
`DateID` int(11) NOT NULL,{0}
`Requests` int(11) DEFAULT '0',
`Impressions` int(11) DEFAULT '0',
`Clicks` int(11) DEFAULT '0',
`Revenue` decimal(17,5) DEFAULT '0.00000',{1}
 UNIQUE KEY operator_by_day(DateID{2})
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
`DateID` int(11) NOT NULL,{0}
`Impressions` int(11) DEFAULT '0',
`Clicks` int(11) DEFAULT '0',
`Spend` decimal(17,5) DEFAULT '0.00000',{1}
UNIQUE KEY operator_by_day(DateID{2})
) ENGINE=MyISAM;", filterByColumn, idColumn, key);
                                    break;
                                }

                        }
                        break;
                    }
                case Type.Chart:
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

            switch (args.Type)
            {
                case Type.Report:
                    {
                        str = GenerateReportTempTableSelectCommand(args);
                        break;
                    }
                case Type.Chart:
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
            var dummyWhere = " WHERE  DateID>0 ";
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
LEFT OUTER JOIN {5}localizedstrings Locs
ON {3} = Locs.LocalizedStringID
WHERE (isnull(Locs.Culture) or Locs.Culture ='{4}') ",
                    args.TableIdName, args.IdFieldName,
                    args.Fieldname.Trim(','), args.LocalizedStringFieldName,
                    args.Criteria.Culture, ReportGeneratorArgs.DataBase);
                dummyWhere = " AND  DateID>0 ";
            }

            //string.Format("ORDER BY {0} {1}", args.Criteria.OrderColumn, args.Criteria.OrderType);
            var paging = string.Format("LIMIT {0}, {1}", args.Criteria.PageNumber * args.Criteria.ItemsPerPage,
                                       args.Criteria.ItemsPerPage);



            switch (args.EntityType)
            {
                case EntityType.App:
                    {
                        str = string.Format(
                            @"SELECT SQL_NO_CACHE {0} @TotalCount as TotalCount,
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
                            @"SELECT SQL_NO_CACHE {0} @TotalCount as TotalCount,
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
                    selectCommand = string.Format("((SELECT 1 as c FROM report_data  group by DateID,{0}{1} )as temp)", args.DataFieldId, args.Fieldname);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(args.Fieldname))
                {
                    selectCommand = string.Format("((SELECT 1 as c FROM report_data  group by DateID{0} )as temp)", args.Fieldname);
                }
            }*/
            return selectCommand;
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
            var idColumn = string.Empty;
            var duplicateKey = string.Empty;
            var selectSql = string.Empty;

            if (args.Criteria.Layout.Equals("Detailed", StringComparison.OrdinalIgnoreCase))
            {
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
                    groupBy = string.Format(" group by DateID{0}", groupByFields);
                }
            }
            else
            {
                groupByFields = string.Format(",{0}{1}", args.FilterDataFieldId, args.Fieldname);
                groupBy = string.Format(" group by DateID{0}", groupByFields);
                selectSql = string.Format(",IFNULL({0},0){1}", args.Fieldname.Trim(','), idColumn);
            }

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
                                    dateFieldName = "DATE_FORMAT(DateID - INTERVAL ( DATE_FORMAT(DateID, \'%w\') ) DAY , \'%Y%m%d\') as newDateID";
                                    groupBy = string.Format(" group by {0}", groupByFields.Trim(','));  //string.Format(" group by {0}", groupByFields.Trim(','));

                                    break;
                                }
                            case 2: //week
                                {
                                    dateFieldName = "DATE_FORMAT(DateID - INTERVAL ( DATE_FORMAT(DateID, \'%w\') ) DAY , \'%Y%m%d\') as newDateID";
                                    groupBy = string.Format(" group by newDateID{0}", groupByFields);
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
                case SummaryBy.Month:
                    {
                        FactStatTable = args.MonthFactStatTable;
                        break;
                    }
            }


            var populateReportSQL = string.Empty;

            switch (args.Type)
            {
                case Type.Report:
                    {
                        switch (args.EntityType)
                        {
                            case EntityType.App:
                                {
                                    populateReportSQL = string.Format(@"insert into report_data (Requests,Impressions,Revenue,Clicks{0}{1},DateID) 
                                                            select SQL_NO_CACHE sum(Requests) as Requests, sum(Impressions) as Impressions, sum(Revenue) as Revenue, sum(Clicks) as Clicks{2}",
                                            args.Fieldname, idColumn, selectSql);
                                    duplicateKey = "on duplicate key update  Requests=Requests+values(Requests), Impressions = Impressions + values(Impressions), Clicks = Clicks + values(Clicks), Revenue = Revenue + values(Revenue)";
                                    break;
                                }
                            case EntityType.Campaign:
                            case EntityType.AdGroup:
                            case EntityType.Ad:
                                {
                                    populateReportSQL = string.Format(@"insert into report_data (Impressions,Spend,Clicks{0}{1},DateID) 
                                                            select  SQL_NO_CACHE sum(Impressions) as Impressions, sum(Spend) as Spend, sum(Clicks) as Clicks {2}",
                                            args.Fieldname, idColumn, selectSql);
                                    duplicateKey = "on duplicate key update  Impressions = Impressions + values(Impressions), Clicks = Clicks + values(Clicks), Spend = Spend + values(Spend)";
                                    break;
                                }
                        }
                        break;
                    }
                case Type.Chart:
                    {
                        throw new NotImplementedException();
                    }

            }
            var selectCommand = string.Format("{0},{1} from {2} where DateID in ({3})  and {4} ={5} {6} {7} {8} {9} {10};",
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
            var dateFieldName = "DateID";
            var populateReportSQL = string.Empty;
            groupBy = string.Format(" group by Xaxis");
            if (!string.IsNullOrWhiteSpace(args.Criteria.ItemsList))
            {
                appIDsSql = string.Format("AND {0} IN ({1})", args.FilterDataFieldId, string.Format(args.Criteria.ItemsList));
            }
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
                        if (summaryBy == SummaryBy.Month)
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

        private static List<DateTime> GetDates(DateTime dateFrom, DateTime dateTo, SummaryBy summaryBy)
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

        private static List<DateTime> GetPartialDates(ReportGeneratorArgs args, SummaryBy summaryBy, out DateTime newDateFrom, out DateTime newDateTo)
        {
            var result = new List<DateTime>();
            var dateFrom = args.Criteria.FromDate;
            var dateTo = args.Criteria.ToDate;

            newDateFrom = dateFrom;
            newDateTo = dateTo;

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

        private static Dictionary<string, string> GetDateTables(ReportGeneratorArgs args, List<DateTime> dates, SummaryBy summaryBy)
        {
            var result = new Dictionary<string, string>();
            var tableName = args.DayFactStatTable;
            foreach (var date in dates)
            {
                switch (summaryBy)
                {
                    case SummaryBy.Hour:
                        {
                            var dateWeekStart = StartOfWeek(date, DayOfWeek.Sunday);
                            tableName = args.FactStatTable + dateWeekStart.ToString("_yyyy_MM_dd");
                            break;

                        }
                    case SummaryBy.Day:
                        {
                            var dateMonthStart = new DateTime(date.Year, date.Month, 1);
                            tableName = args.DayFactStatTable + dateMonthStart.ToString("_yyyy_MM_dd");
                            break;

                        }
                    case SummaryBy.Week:
                        {
                            tableName = args.WeekFactStatTable;
                            break;
                        }
                    case SummaryBy.Month:
                        {
                            tableName = args.MonthFactStatTable;
                            break;
                        }
                }
                if (result.ContainsKey(tableName))
                {
                    result[tableName] += date.ToString("yyyyMMdd") + ",";
                }
                else
                {
                    result.Add(tableName, date.ToString("yyyyMMdd") + ",");
                }

            }
            foreach (var key in result.Keys.ToList())
            {
                result[key] = result[key].Trim(',');
            }
            return result;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }


    public enum SummaryBy
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3
    }
    public class ReportCriteriaDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SummaryBy { get; set; }

        public string Layout { get; set; }

        public string ItemsList { get; set; }

        public string AdvancedCriteria { get; set; }


        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public string OrderColumn { get; set; }

        public string OrderType { get; set; }

        public string MetricCode { get; set; }

        public string Culture
        {
            get;
            set;
        }
    }

}

