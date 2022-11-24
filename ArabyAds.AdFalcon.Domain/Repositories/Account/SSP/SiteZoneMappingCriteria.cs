using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{


    public class SiteZoneMappingCriteria : CriteriaBase<SiteZoneMapping>
    {


        public string AdFalconSubPublisherId { get; set; }
        public string AppSiteName { get; set; }
        public int? AdTypeId { get; set; }
        public int? DeviceTypeId { get; set; }
        public int AppSiteId { get; set; }
        public bool? IsInterstitial { get; set; }
        public int ZoneId { get; set; }
        public int SiteId { get; set; }
        public int BusinessId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string MappingName { set; get; }




        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP.SiteZoneMappingCriteria Commoncr)
        {



         


        AdFalconSubPublisherId = Commoncr.AdFalconSubPublisherId;



        AppSiteName = Commoncr.AppSiteName;

        AdTypeId = Commoncr.AdTypeId;




        DeviceTypeId = Commoncr.DeviceTypeId;



        AppSiteId = Commoncr.AppSiteId;

        IsInterstitial = Commoncr.IsInterstitial;


        BusinessId = Commoncr.BusinessId;


            MappingName = Commoncr.MappingName;

            ZoneId = Commoncr.ZoneId;
            SiteId = Commoncr.SiteId;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;



        }
        public override Expression<Func<SiteZoneMapping, bool>> GetExpression()
        {

            Expression<Func<SiteZoneMapping, bool>> filter = (c => c.IsDeleted == false && c.Zone.ID == ZoneId && c.Site.ID == SiteId);
            return filter;
        }

        public  Expression<Func<SiteZoneMapping, bool>> GetFullSearchExpression()
        {

            Expression<Func<SiteZoneMapping, bool>> filter = (c => c.IsDeleted == false && c.Zone.ID == ZoneId && c.Site.ID == SiteId && c.AppSite.ID == AppSiteId && (!AdTypeId.HasValue || c.AdType.ID == AdTypeId) && (!DeviceTypeId.HasValue || c.DeviceType.ID == DeviceTypeId) &&(!IsInterstitial.HasValue ||  c.IsInterstitial == IsInterstitial) && (string.IsNullOrEmpty(AdFalconSubPublisherId) || c.AdFalconSubPublisherId.Equals(AdFalconSubPublisherId)));
            return filter;
        }
        public override Func<SiteZoneMapping, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<SiteZoneMapping, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
