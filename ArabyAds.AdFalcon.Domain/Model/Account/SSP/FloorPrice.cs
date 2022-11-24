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
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;

namespace ArabyAds.AdFalcon.Domain.Model.Account.SSP
{
    //    [DataContract(Name = "FloorPriceConfigType")]
    //public enum FloorPriceConfigType
    //{
    //        [EnumMember]
    //        Undefined = 0,
    //   [EnumMember]
    //    Base = 1,
    //   [EnumMember]
    //    ActionType = 2,
    //    [EnumMember]
    //    Operator = 3,
    //    [EnumMember]
    //    Geographic = 4,
    //   [EnumMember]
    //    Keyword = 5,
    //    [EnumMember]
    //    Demographic = 6,
    //    [EnumMember]
    //    Platform = 7,
    //    [EnumMember]
    //    Manufacturer = 8,
    //    [EnumMember]
    //    DeviceType = 9
    //}

   
    public class FloorPrice : IEntity<int>
    {

        private static IFloorPriceRepository _floorPriceRepository = null;
        private static IFloorPriceRepository FloorPriceRepository
        {
            get
            {
                if (_floorPriceRepository == null)
                {
                    _floorPriceRepository = Framework.IoC.Instance.Resolve<IFloorPriceRepository>();
                }
                return _floorPriceRepository;
            }
        }
        public virtual bool IsDeleted
        {
            get;
            set;
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
        public virtual FloorPriceConfigType ConfigType
        {
            get;
            set;
        }
        
        public virtual string Description { get; set; }

        public virtual int ID
        {
            get;
            protected set;
        }
        public virtual int? TargetingId
        {
            get;
            set;
        }
        public virtual void Delete()
        {

            this.IsDeleted = true;
        }
        public virtual decimal Price
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return this.Description;
        }
        public virtual void FloorPriceValidation(  int? Id)
        {
            List<FloorPrice> result = new List<FloorPrice>();
            if (!Id.HasValue) Id = -1;


            result = FloorPriceRepository.Query(x => x.TargetingId == this.TargetingId && x.ID != (int)Id && !x.IsDeleted && x.Site.ID == this.Site.ID && x.Zone.ID == this.Zone.ID && x.ConfigType == this.ConfigType).ToList();
            if (result.Count > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedFloorPrice") });
            }
           
            


        }

    }
}
