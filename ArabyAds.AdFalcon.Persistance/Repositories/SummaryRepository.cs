using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using NHibernate;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    public class SummaryRepository : RepositoryBase<ChartDto, int>, ISummaryRepository
    {
        public SummaryRepository(RepositoryImplBase<ChartDto, int> repository)
            : base(repository)
        {

        }

        public decimal GetAccountTotalRevenue(DateTime fromDate, DateTime toDate, int accountId)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery sqlQuery =
                nhibernateSession.CreateSQLQuery("call sp_GetAccountTotalRevenue(:FromDate,:ToDate,:AccountId)");
            sqlQuery.SetTimeout(120);
            sqlQuery.SetDateTime("FromDate", fromDate);
            sqlQuery.SetDateTime("ToDate", toDate);
            sqlQuery.SetInt32("AccountId", accountId);

            return sqlQuery.UniqueResult<decimal>();
        }

        public decimal GetAdvertiserAccountTotalSpend(DateTime fromDate, DateTime toDate, int AdvertiserAssociationId)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery sqlQuery =
                nhibernateSession.CreateSQLQuery("call sp_GetAdvertiserAccountTotalSpend(:FromDate,:ToDate,:AdvertiserAssociationId)");
            sqlQuery.SetTimeout(120);
            sqlQuery.SetDateTime("FromDate", fromDate);
            sqlQuery.SetDateTime("ToDate", toDate);
            sqlQuery.SetInt32("AdvertiserAssociationId", AdvertiserAssociationId);

            return sqlQuery.UniqueResult<decimal>();
        }
        public decimal GetAccountTotalSpend(DateTime fromDate, DateTime toDate, int accountId)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery sqlQuery =
                nhibernateSession.CreateSQLQuery("call sp_GetAccountTotalSpend(:FromDate,:ToDate,:AccountId)");
            sqlQuery.SetTimeout(120);
            sqlQuery.SetDateTime("FromDate", fromDate);
            sqlQuery.SetDateTime("ToDate", toDate);
            sqlQuery.SetInt32("AccountId", accountId);

            return sqlQuery.UniqueResult<decimal>();
        }


        #region Performance

        public List<AppSitePerformance> GetAppSitesPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query = nhibernateSession.CreateSQLQuery("call sp_GetAppAllStatByIds(:AppSiteIds,:CampaignType,:OtherCampaignType,:DateFrom,:DateTo)");
            query.SetTimeout(120);
            query.SetString("AppSiteIds", criteria.GetIds());
            query.SetInt32("CampaignType", (int)criteria.CampaignType);
            query.SetInt32("OtherCampaignType", (int)criteria.OtherCampaignType);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("DateFrom", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("DateFrom", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("DateTo", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("DateTo", null);
            }
            query.SetResultTransformer(Transformers.AliasToBean<AppSitePerformance>());
            var items = query.List<AppSitePerformance>().ToList() ?? new List<AppSitePerformance>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.AppSiteID == id) where item == null select id)
            {
                items.Add(new AppSitePerformance() { AppSiteID = id });
            }
            return items;
        }

        public List<CampaignPerformanceDto> GetCampaignsPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetCampaignAllStatByCampaignID(:CampaignIDs,:FromDate,:ToDate)");
            query.SetTimeout(120);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("FromDate", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("ToDate", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            query.SetString("CampaignIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<CampaignPerformanceDto>());
            var items = query.List<CampaignPerformanceDto>().ToList() ?? new List<CampaignPerformanceDto>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.DimCampaignID == id) where item == null select id)
            {
                items.Add(new CampaignPerformanceDto() { DimCampaignID = id });
            }
            return items;
        }

        public CampaignPerformanceDto GetCampaignPerformance(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetCampaignStatByCampaignID(:CampaignIDs)");
            query.SetTimeout(120);
            query.SetString("CampaignIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<CampaignPerformanceDto>());
            var item = query.UniqueResult<CampaignPerformanceDto>() ?? new CampaignPerformanceDto();
            return item;
        }

        public List<AdGroupPerformanceDto> GetAdGroupsPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetCampaignAllStatByAdsGroupID(:AdsGroupIDs,:FromDate,:ToDate)");
            query.SetTimeout(120);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("FromDate", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("ToDate", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            query.SetString("AdsGroupIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdGroupPerformanceDto>());
            var items = query.List<AdGroupPerformanceDto>().ToList() ?? new List<AdGroupPerformanceDto>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.AdsGroupID == id) where item == null select id)
            {
                items.Add(new AdGroupPerformanceDto() { AdsGroupID = id });
            }
            return items;
        }

        public AdGroupPerformanceDto GetAdGroupPerformance(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetCampaignAdsGroupsStatByIDs(:AdsGroupIDs)");
            query.SetTimeout(120);
            query.SetString("AdsGroupIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdGroupPerformanceDto>());
            var item = query.UniqueResult<AdGroupPerformanceDto>() ?? new AdGroupPerformanceDto();
            return item;
        }

        public List<AdPerformance> GetAdsPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetCampaignAllStatByAdsID(:AdsIDs,:FromDate,:ToDate)");
            query.SetTimeout(120);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("FromDate", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("ToDate", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            query.SetString("AdsIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdPerformance>());
            var items = query.List<AdPerformance>().ToList() ?? new List<AdPerformance>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.AdsID == id) where item == null select id)
            {
                items.Add(new AdPerformance() { AdsID = id });
            }
            return items;
            return items;
        }
        #endregion
        public IList<AdStatus> GetAdsByCampaign(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAdsAllByCampaignID(:CampaignIDs)");
            query.SetTimeout(120);
            query.SetString("CampaignIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdStatus>());
            var items = query.List<AdStatus>().ToList() ?? new List<AdStatus>();
            return items;
        }
        public IList<AdStatus> GetAdsByAdvertiser(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAdsAllByAccountAdvID(:AdvIDs)");
            query.SetTimeout(120);
            query.SetString("AdvIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdStatus>());
            var items = query.List<AdStatus>().ToList() ?? new List<AdStatus>();
            return items;
        }

        public IList<AdStatus> GetAdsByAdGroups(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAdsAllByAdGroupID(:AdGroupIDs)");
            query.SetTimeout(120);
            query.SetString("AdGroupIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdStatus>());
            var items = query.List<AdStatus>().ToList() ?? new List<AdStatus>();
            return items;
        }
        public string CalculateStatus(IList<AdStatus> localAds)
        {

            string Status = string.Empty;

          

            //if no ad then the status is empty
            if ((localAds == null) || localAds.Count == 0)
            {
                Status = AdGroupStatus.Empty.Name.ToString();
                return Status;
            }
            //get not Submitted count
            var noneSubmittedCount = localAds.Count(item => item.StatusId == AdCreativeStatus.Submitted.ID);
            //if count is zero then the ad group is empty
            if (noneSubmittedCount == localAds.Count)
            {
                Status = AdGroupStatus.Empty.Name.ToString();
                return Status;
            }


            //get all Completed/Expired status count
            var completedCount = localAds.Count(item => (item.StatusId == AdCreativeStatus.Completed.ID || item.StatusId == AdCreativeStatus.Expired.ID));
            //if Completed/Expired Count is Ads Count then the ad group is Completed
            if (completedCount == localAds.Count)
            {
                Status = AdGroupStatus.Completed.Name.ToString();
                return Status;
            }


            //get all Active status count
            var activeCount = localAds.Count(item => (item.StatusId == AdCreativeStatus.Active.ID  || item.StatusId == AdCreativeStatus.ActiveAdServer.ID));
            //if Active count is Ads Count then the ad group is Running
            if (activeCount == localAds.Count)
            {
                Status = AdGroupStatus.Running.Name.ToString();
                return Status;
            }



            //check some ads are active then status is "Running With Attention Action Needed"
            if ((activeCount != 0))
            {
                Status = AdGroupStatus.RunningWithAttentionActionNeeded.Name.ToString();
                return Status;
            }



            ////get not disapproved ,Inactive,Paused , Budget Paused,Completed or Expired status count
            //var count = localAds.Count(item => (item.StatusId == AdCreativeStatus.DisApproved.ID ||
            //                                    item.StatusId == AdCreativeStatus.Inactive.ID ||
            //                                    item.StatusId == AdCreativeStatus.Paused.ID ||
            //                                    item.StatusId == AdCreativeStatus.BudgetPaused.ID ||
            //                                    item.StatusId == AdCreativeStatus.Completed.ID ||
            //                                    item.StatusId == AdCreativeStatus.Expired.ID));
            ////if count is Ads Count then the ad group is "Attention/Action Needed"
            //if (count == localAds.Count)
            //{
                Status = AdGroupStatus.AttentionActionNeeded.Name.ToString();
                return Status;
            //}


            return Status;
        }
        public string CalculateAdvertiserStatus(IList<AdStatus> localAds)
        {

            string Status = string.Empty;

            

            //get all Active status count
            var activeCount = localAds.Count(item => (item.StatusId == AdCreativeStatus.Active.ID || item.StatusId == AdCreativeStatus.ActiveAdServer.ID));
            //if Active count is Ads Count then the ad group is Running
            if (activeCount == localAds.Count && activeCount>0)
            {
                Status = Framework.Resources.ResourceManager.Instance.GetResource( "Active", "JobGrid");
                return Status;
            }



            //check some ads are active then status is "Running With Attention Action Needed"
            if ((activeCount != 0))
            {
                Status = Framework.Resources.ResourceManager.Instance.GetResource( "Active", "JobGrid");
                return Status;
            }


            
            Status = Framework.Resources.ResourceManager.Instance.GetResource("InActiveAdvertisers", "Global");
            return Status;
      


           
        }
        public string CalculateAdvertiserStatusString(IList<AdStatus> localAds)
        {

            string Status = string.Empty;



            //get all Active status count
            var activeCount = localAds.Count(item => (item.StatusId == AdCreativeStatus.Active.ID || item.StatusId == AdCreativeStatus.ActiveAdServer.ID));
            //if Active count is Ads Count then the ad group is Running
            if (activeCount == localAds.Count && activeCount > 0)
            {
              
                return "Active";
            }



            //check some ads are active then status is "Running With Attention Action Needed"
            if ((activeCount != 0))
            {
                return "Active";
            }



           
            return "NotActive";




        }
        public List<AdvertiserPerformanceDto> GetAdvertisersPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAdvertiserAllStatByAdvertiserID(:AdvertiserIDs,:FromDate,:ToDate)");
            query.SetTimeout(120);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("FromDate", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("ToDate", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            query.SetString("AdvertiserIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdvertiserPerformanceDto>());
            var items = query.List<AdvertiserPerformanceDto>().ToList() ?? new List<AdvertiserPerformanceDto>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.AdvertiserAssociationId == id) where item == null select id)
            {
                items.Add(new AdvertiserPerformanceDto() { AdvertiserAssociationId = id });
            }
            return items;
        }

        public AdvertiserPerformanceDto GetAdvertiserPerformance(PerformanceCriteriaBase criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAdvertiserStatByAdvertiserID(:AdvertiserIDs)");
            query.SetTimeout(120);
            query.SetString("AdvertiserIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AdvertiserPerformanceDto>());
            var item = query.UniqueResult<AdvertiserPerformanceDto>() ?? new AdvertiserPerformanceDto();
            return item;
        }


        public List<AudienceListPerformanceDto> GetAudienceListsPerformance(PerformanceCriteria criteria)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query =
                nhibernateSession.CreateSQLQuery(
                    "call sp_GetAudienceAllStatByAdvertiserID(:AudienceListIDs,:FromDate,:ToDate)");
            query.SetTimeout(120);
            if (criteria.FromDate.HasValue)
            {
                query.SetDateTime("FromDate", criteria.FromDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.ToDate.HasValue)
            {
                query.SetDateTime("ToDate", criteria.ToDate.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            query.SetString("AudienceListIDs", criteria.GetIds());
            query.SetResultTransformer(Transformers.AliasToBean<AudienceListPerformanceDto>());
            var items = query.List<AudienceListPerformanceDto>().ToList() ?? new List<AudienceListPerformanceDto>();
            foreach (var id in from id in criteria.Ids let item = items.FirstOrDefault(x => x.AdvertiserAssociationId == id) where item == null select id)
            {
                items.Add(new AudienceListPerformanceDto() { AdvertiserAssociationId = id });
            }
            return items;
        }
    }
}
