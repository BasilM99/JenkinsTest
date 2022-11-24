using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Account.SSP;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account.SSP
{
    

    public class SiteZoneMapping : IEntity<int>
    {

        private static ISiteZoneMappingRepository _siteZoneMappingRepository = null;
        private static ISiteZoneMappingRepository SiteZoneMappingRepository
        {
            get
            {
                if (_siteZoneMappingRepository == null)
                {
                    _siteZoneMappingRepository = Framework.IoC.Instance.Resolve<ISiteZoneMappingRepository>();
                }
                return _siteZoneMappingRepository;
            }
        }
        public virtual string Description { get; set; }

        public virtual int ID
        {
            get;
            protected set;
        }
        public virtual bool IsDeleted
        {
            get;
             set;
        }
    
        public virtual bool? IsInterstitial
        {
            get;
            set;
        }
        public virtual AdType AdType
        {
            get;
            set;
        }
        public virtual DeviceType DeviceType
        {
            get;
            set;
        }
        public virtual ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite AppSite
        {
            get;
            set;
        }

        public virtual bool IsNative
        {
            get;
            set;
        }
        public virtual SubAppsite SubAppSite { get; set; }

        public virtual string AdFalconSubPublisherId
        {
            get;
            set;
        }
        
        public virtual void Delete()
        {

            this.IsDeleted = true;
        }
        public virtual SiteZone Zone
        {
            get;
            set;
        }
        public virtual PartnerSite Site
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return this.Description;
        }

        public virtual int IsUniqueMapping(SiteZoneMappingCriteria criteria)
        {
           criteria.AppSiteId = this.AppSite.ID;
           if (this.AdType!=null)
            criteria.AdTypeId = this.AdType.ID;
           if (this.DeviceType != null)
            criteria.DeviceTypeId = this.DeviceType.ID;
            criteria.IsInterstitial = this.IsInterstitial;
            criteria.AdFalconSubPublisherId = this.AdFalconSubPublisherId;
            var result = SiteZoneMappingRepository.Query(criteria.GetFullSearchExpression()).ToList();
            if (result!=null)
            {
                if (result.Count==1)
                {

                    return result[0].ID;
                
                }
            
            }

            return 0;

        }
    }
}
