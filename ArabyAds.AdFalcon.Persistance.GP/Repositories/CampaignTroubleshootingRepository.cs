using NHibernate;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ArabyAds.Framework;
using ArabyAds.Framework.Persistence;
using ArabyAds.Framework.Utilities;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.ReportsGP.Repositories
{
    public class CampaignTroubleshootingRepository : BaseReportGPRepository, ICampaignTroubleshootingRepository
    {
        private readonly IAdGroupRepository adGroupRepository;
        private readonly static string PRESENT_VIEW_QUERY = @$" fact_adgroups as (select dealid, adgroupid, sum(requests_dag)  filled
                                                              from fact_stat_d where dateid between  @fromDate and @toDate  and dealid =@dealid    and adgroupid =@adgroupId and advaccountid=@AccountId
	                                                          group by 1, 2 ) ";

        private readonly static string Reasons_QUERY = @" inner join dim_lookup_of_lookups reasons
                                                        on fact_troubleshooting_adgroups_d.reasonid = reasons.id ";

        private readonly static string Category_QUERY = @" inner join dim_lookup_of_lookups categ on categ.id = reasons.parentid ";

        private readonly static string Adgroup_QUERY = @" left join fact_adgroups fact on fact.adgroupid = fact_troubleshooting_adgroups_d.adgroupid ";

        private readonly static string FILTERS = @" where reasons.lookupid = 10 and categ.lookupid = 11 and fact_troubleshooting_adgroups_d.dealid =@dealid
            and fact_troubleshooting_adgroups_d.adgroupid = @adgroupId AND fact_troubleshooting_adgroups_d.dateid BETWEEN @fromDate
            and @toDate ";

        private readonly static string Fields = @" fact.filled as Filled, fact_troubleshooting_adgroups_d.adgroupid as AdGroupid, fact_troubleshooting_adgroups_d.dateid as DateId, fact_troubleshooting_adgroups_d.counter as Counter, fact_troubleshooting_adgroups_d.reasonid as ReasonId, reasons.description as ReasonDesc,
                                                categ.id as CategoryId, categ.description as CategoryDesc ,categ.order as CategoryOrder ";

        private readonly static string TableName = @" fact_troubleshooting_adgroups_d ";

        public CampaignTroubleshootingRepository(RepositoryImplBase<ChartDto, int> repository, ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager, IAdGroupRepository adGroupRepository)
           : base(repository, configurationManager)
        {
            this.adGroupRepository = adGroupRepository;
        }

        public List<CampaignTroubleshootingDto> GetResult(CampaignTroubleshootingCriteria args)
        {
            var fromDateInt = Convert.ToInt32(args.FromDate.ToString("yyyyMMdd"));
            var toDateInt = Convert.ToInt32(args.ToDate.ToString("yyyyMMdd"));
            var items = GetResult(args.AdGroupId, args.DealId, fromDateInt, toDateInt, args.AccountId).ToList();

            return items;
        }

        private string GenerateScript()
        {
            StringBuilder finalSelectStatement = new StringBuilder("with ");
            finalSelectStatement.Append(PRESENT_VIEW_QUERY);
            finalSelectStatement.Append(" SELECT ");
            finalSelectStatement.Append(Fields);
            finalSelectStatement.Append(" from ");
            finalSelectStatement.Append(TableName);
            finalSelectStatement.Append(Reasons_QUERY);
            finalSelectStatement.Append(Category_QUERY);
            finalSelectStatement.Append(Adgroup_QUERY);
            finalSelectStatement.Append(FILTERS);

            return finalSelectStatement.ToString();
        }

        protected List<CampaignTroubleshootingDto> GetResult(int adgroupId, int dealId, int fromDate, int toDate, int AccountId)
        {
            string ConnectionName = "ReportingGPDB";
            string Connectionstring = JsonConfigurationManager.ConnectionStrings[ConnectionName] ?? "";
            var result = new List<CampaignTroubleshootingDto>();
            var script = GenerateScript();
            script = script.Replace("@adgroupId", adgroupId.ToString());
            script = script.Replace("@dealId", dealId.ToString());
            script = script.Replace("@dealid", dealId.ToString());
            script = script.Replace("@fromDate", fromDate.ToString());
            script = script.Replace("@toDate", toDate.ToString());
            script = script.Replace("@AccountId", AccountId.ToString());
            result = GetResult<CampaignTroubleshootingDto>(script, string.Empty, "Get CampaignTroubleshooting").ToList();


            /*using (NpgsqlConnection connection = new NpgsqlConnection(Connectionstring))
            {
                connection.Open();

                //Get final Select Stateme
            
                using (NpgsqlCommand cmd = new NpgsqlCommand(script, connection))
                {
                    cmd.Parameters.AddWithValue("@adgroupId", adgroupId);
                    cmd.Parameters.AddWithValue("@dealId", dealId);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@AccountId", AccountId);

                    using (NpgsqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        var columns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();

                        while (dataReader.Read())
                        {
                            string[] row = new string[columns.Count()];
                            IDictionary<string, string> rowDic = new Dictionary<string, string>();
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                row[i] = dataReader.GetValue(i).ToString();
                                rowDic.Add(columns[i], row[i]);
                            }

                            result.Add(new CampaignTroubleshootingDto 
                            { CategoryId=int.Parse(rowDic["categoryid"]), 
                                CategoryDesc= rowDic["categorydesc"], 
                                CategoryOrder= int.Parse(rowDic["categoryorder"]), 
                                Counter= long.Parse(rowDic["counter"]), 
                                DateId= int.Parse(rowDic["dateid"]), 
                                ReasonId= int.Parse(rowDic["reasonid"]), 
                                ReasonDesc= rowDic["reasondesc"], 
                                Filled= !string.IsNullOrEmpty(rowDic["filled"])? long.Parse(rowDic["filled"]) : 0 });
                        }
                    }

                    connection.Close();
              
                }
            }*/
            return result;
        }

    }
}
