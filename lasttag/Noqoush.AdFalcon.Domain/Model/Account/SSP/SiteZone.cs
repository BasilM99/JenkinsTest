using System;

using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Domain.Model.Account.SSP
{
    public class SiteZone : IEntity<int>
    {

        private readonly ISiteZoneRepository _siteZoneRepository = Framework.IoC.Instance.Resolve<ISiteZoneRepository>();
        public virtual string Description { get; set; }

        public virtual int ID
        {
            get;
             set;
        }
        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual string ZoneID
        {
            get;
             set;
        }
        public virtual string ZoneName
        {
            get;
            set;
        }
        public virtual PartnerSite Site
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

        public virtual void SiteZoneValidation(string Name, string ZoneID, int SiteID, int? Id)
        {
            List<SiteZone> result = new List<SiteZone>();
            if (!Id.HasValue) Id = -1;


            result = _siteZoneRepository.Query(x => x.ZoneID == ZoneID  && x.ID != (int)Id && !x.IsDeleted && x.Site.ID == SiteID).Where(M => M.ZoneID.Equals(ZoneID, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (result.Count > 0)
                {
                    throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedZoneId") });
                }
                result = _siteZoneRepository.Query(x => x.ZoneName == Name && x.ID != (int)Id && !x.IsDeleted  && x.Site.ID == SiteID).ToList();
                if (result.Count > 0)
                {
                    throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedZoneName") });
                }
           

        }
    }
}
