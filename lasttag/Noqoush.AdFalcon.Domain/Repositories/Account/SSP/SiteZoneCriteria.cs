using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
namespace Noqoush.AdFalcon.Domain.Repositories.Account.SSP
{


    public class SiteZoneCriteria : CriteriaBase<SiteZone>
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int BusinessId { get; set; }
        public int SiteId { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string ZoneName { get; set; }
        public string ZoneId { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.SiteZoneCriteria Commoncr)
        {



            BusinessId = Commoncr.BusinessId;


            ZoneName = Commoncr.ZoneName;

            ZoneId = Commoncr.ZoneId;
            SiteId = Commoncr.SiteId;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;



        }
        public override Expression<Func<SiteZone, bool>> GetExpression()
        {

            Expression<Func<SiteZone, bool>> filter = (c => c.IsDeleted == false && c.Site.ID == SiteId


                   && (string.IsNullOrEmpty(ZoneId) || c.ZoneID.Contains(ZoneId)) && (string.IsNullOrEmpty(ZoneName) || c.ZoneName.Contains(ZoneName)));
            return filter;
        }
        public override Func<SiteZone, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<SiteZone, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
