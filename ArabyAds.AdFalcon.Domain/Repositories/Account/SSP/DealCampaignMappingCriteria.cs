using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{


    public class DealCampaignMappingCriteria : CriteriaBase<DealCampaignMapping>
    {
        public string  CampaignName { get; set; }
        public int PartnerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string DealIdName { get; set; }
        public int? SiteId { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP.DealCampaignMappingCriteria Commoncr)
        {







            PartnerId = Commoncr.PartnerId;
            CampaignName = Commoncr.CampaignName;
            DealIdName =Commoncr.DealIdName;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            SiteId = Commoncr.SiteId;


        }
        public override Expression<Func<DealCampaignMapping, bool>> GetExpression()
        {

            Expression<Func<DealCampaignMapping, bool>> filter = (c => c.IsDeleted == false && c.Partner.ID == PartnerId


                   );
            return filter;
        }
        public override Func<DealCampaignMapping, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<DealCampaignMapping, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
