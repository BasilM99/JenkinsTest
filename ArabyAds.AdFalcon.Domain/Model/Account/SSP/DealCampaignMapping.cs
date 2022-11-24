
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArabyAds.AdFalcon.Business.Domain.Exceptions;
using ArabyAds.AdFalcon.Domain.Model.AppSite.Filtering;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.DataAnnotations;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Repositories.Account.SSP;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
namespace ArabyAds.AdFalcon.Domain.Model.Account.SSP
{

   
    public class DealCampaignMapping : IEntity<int>
    {
        private static IDealCampaignMappingRepository _dealCampaignMappingRepository = null;
        private static IDealCampaignMappingRepository DealCampaignMappingRepository
        {
            get
            {
                if (_dealCampaignMappingRepository == null)
                {
                    _dealCampaignMappingRepository = Framework.IoC.Instance.Resolve<IDealCampaignMappingRepository>();
                }
                return _dealCampaignMappingRepository;
            }
        }
        public virtual bool IsDeleted
        {
            get;
            set;
        }
      
        public virtual int ID
        {
            get;
            protected set;
        }
        public virtual string DealId
        {
            get;
            set;
        }
        public virtual BusinessPartner Partner
        {
            get;
            set;
        }

        public virtual ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign Campaign
        {
            get;
            set;
        }

        /*public virtual int AdFalconCampaignId
        {
            get;
            set;
        }*/
        public virtual void Delete()
        {

            this.IsDeleted = true;
        }
     
        public virtual string GetDescription()
        {
            return this.ID.ToString();
        }

        public virtual void DealCampaignMappingValidation(int? Id)
        {
            List<DealCampaignMapping> result = new List<DealCampaignMapping>();
            if (!Id.HasValue) Id = -1;


            result = DealCampaignMappingRepository.Query(x => x.Partner.ID == this.Partner.ID && x.ID != (int)Id && !x.IsDeleted && x.Campaign.ID == this.Campaign.ID && x.DealId == this.DealId).ToList();
            if (result.Count > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedDealCampaignMapping") });
            }


            result = DealCampaignMappingRepository.Query(x => x.Partner.ID == this.Partner.ID && x.ID != (int)Id && !x.IsDeleted &&   x.DealId == this.DealId).ToList();
            if (result.Count > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedDealCampaignMapping") });
            }


        }

    }
}
