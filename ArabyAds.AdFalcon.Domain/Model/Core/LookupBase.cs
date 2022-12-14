//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.Serialization;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.DomainServices.Localization;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;
// using ArabyAds.AdFalcon.Base;
namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    [KnownType(typeof(Device))]
    [KnownType(typeof(Currency))]
    [KnownType(typeof(DeviceType))]
    
    [KnownType(typeof(CompanyType))]
    [KnownType(typeof(Manufacturer))]
    [KnownType(typeof(Platform))]
    [KnownType(typeof(Keyword))]
    [KnownType(typeof(LocationBase))]
    [KnownType(typeof(Operator))]
    [KnownType(typeof(DeviceCapability))]
 //[KnownType(typeof(Core.CostElement.CostItem))]
    [KnownType(typeof(Core.CostElement.Fee))]
    [KnownType(typeof(Core.CostElement.CostElement))]
    [KnownType(typeof(JobPosition))]
    [KnownType(typeof(AdCreativeAttribute))]
    [KnownType(typeof(CostModelWrapper))]
    [KnownType(typeof(CostModel))]
    [KnownType(typeof(AppMarketingPartner))]
    [KnownType(typeof(Advertiser))]
    [KnownType(typeof(AudienceSegment))]
    [KnownType(typeof(CreativeUnit))]
    [KnownType(typeof(CreativeVendor))]
    [KnownType(typeof(ImpressionMetric))]
    // [KnownType(typeof(CreativeVendorKeyword))]
    [KnownType(typeof(Language))]


    [KnownType(typeof(InStreamPosition))]
    [KnownType(typeof(PlacementType))]
   
    [KnownType(typeof(PlaybackMethods))]
    [KnownType(typeof(SkippableAds))]
    [KnownType(typeof(Protocol))]

    [KnownType(typeof(KPIConfig))]
    //[KnownType(typeof(ViewAbilityVendor))]
    
    public class ManagedLookupBase : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual LocalizedString Name
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return Name.ToString();
        }
    }

    public class LookupBase<TEntity, TId> : IEntity<TId>
    {
        public virtual TId ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual LocalizedString Name
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return Name.ToString();
        }
    }
}

