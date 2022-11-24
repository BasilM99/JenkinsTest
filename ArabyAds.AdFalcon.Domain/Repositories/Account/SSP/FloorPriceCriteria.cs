using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{


    public class FloorPriceCriteria : CriteriaBase<FloorPrice>
    {
        public FloorPriceConfigType ConfigType { get; set; }
        public int SiteId { get; set; }
        public int ZoneId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP.FloorPriceCriteria Commoncr)
        {





            ConfigType = Commoncr.ConfigType;

            SiteId = Commoncr.SiteId;
            ZoneId = Commoncr.ZoneId;
            Name = Commoncr.Name;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            SiteId = Commoncr.SiteId;


        }

        private string ConvertToString(object String)
        {

            return String.ToString();

        }
        public override Expression<Func<FloorPrice, bool>> GetExpression()
        {
            Expression<Func<FloorPrice, bool>> filter = null;
            //Expression<Func<FloorPrice, bool>> filter = (c => c.IsDeleted == false && c.Zone.ID == ZoneId && c.Site.ID == SiteId && (string.IsNullOrEmpty(Name) || ConvertToString(c.ConfigType).ToLower().Contains(Name)));
            if (ConfigType==FloorPriceConfigType.Undefined)
           filter = (c => c.IsDeleted == false && c.Zone.ID == ZoneId && c.Site.ID == SiteId);
            else
                filter = (c => c.IsDeleted == false && c.Zone.ID == ZoneId && c.Site.ID == SiteId&& c.ConfigType== ConfigType);
            return filter;
        }
        public override Func<FloorPrice, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<FloorPrice, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
