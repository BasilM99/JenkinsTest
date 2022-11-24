using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{


    public class PartnerSiteCriteria : CriteriaBase<PartnerSite>
    {
        public int PartnerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string SiteName { get; set; }
        public string SiteId { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP.PartnerSiteCriteria Commoncr)
        {





            PartnerId = Commoncr.PartnerId;

            SiteName = Commoncr.SiteName;
            SiteId = Commoncr.SiteId;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;



        }

        public override Expression<Func<PartnerSite, bool>> GetExpression()
        {
            
            Expression<Func<PartnerSite, bool>> filter = (c => c.IsDeleted == false && c.Partner.ID == PartnerId


                   && (string.IsNullOrEmpty(SiteId) || c.SiteID.Contains(SiteId)) && (string.IsNullOrEmpty(SiteName) || c.SiteName.Contains(SiteName)));
            return filter;
        }
        public override Func<PartnerSite, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<PartnerSite, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
