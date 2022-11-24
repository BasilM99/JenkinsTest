using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Account.SSP
{
    public class PartnerSite : IEntity<int>
    {
        private readonly IPartnerSiteRepository _partnerSiteRepository = Framework.IoC.Instance.Resolve<IPartnerSiteRepository>();

        public virtual string Description { get; set; }

        public virtual int ID
        {
            get;
             set;
        }
        public virtual string SiteID
        {
            get;
             set;
        }
        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual string SiteName
        {
            get;
            set;
        }
        public virtual BusinessPartner Partner
        {
            get;
            set;
        }
        public virtual void Delete()
        {

            this.IsDeleted = true;
        }
        public virtual string GetDescription()
        {
            return this.Description;
        }

        public virtual void PartnerSiteValidation(string Name, string SiteId, int PartnerId, int? Id)
        { 
            List<PartnerSite> result = new List<PartnerSite>();
            if (!Id.HasValue) Id = -1;

            result = _partnerSiteRepository.Query(x => x.SiteID == SiteId && x.ID != (int)Id && !x.IsDeleted && x.Partner.ID == PartnerId).Where(M=>M.SiteID.Equals(SiteId, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (result.Count > 0)
                {
                    throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedSiteId") });
                }
                result = _partnerSiteRepository.Query(x => x.SiteName == Name && x.ID != (int)Id && !x.IsDeleted  && x.Partner.ID == PartnerId).ToList();
                if (result.Count > 0)
                {
                    throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedSiteName") });
                }
        

        }
    }
}
