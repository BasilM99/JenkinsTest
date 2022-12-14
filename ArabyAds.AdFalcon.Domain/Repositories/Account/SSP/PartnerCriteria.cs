using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{


    public class PartnerCriteria : CriteriaBase<BusinessPartner>
    {
        public string PartnerName { get; set; }
        public int ZoneId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int TypeId { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP.PartnerCriteria Commoncr)
        {





            PartnerName = Commoncr.PartnerName;

            ZoneId = Commoncr.ZoneId;
            TypeId = Commoncr.TypeId;
      

            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

         

        }


        public override Expression<Func<BusinessPartner, bool>> GetExpression()
        {

            Expression<Func<BusinessPartner, bool>> filter = (c => c.IsDeleted == false && c.Type.ID == TypeId &&

                 (c is BusinessPartner || c is SSPPartner || c is DSPPartner || c is DPPartner)
                  && (string.IsNullOrEmpty(PartnerName) || c.Name.Contains(PartnerName)));
            return filter;
        }
        public override Func<BusinessPartner, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<BusinessPartner, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
