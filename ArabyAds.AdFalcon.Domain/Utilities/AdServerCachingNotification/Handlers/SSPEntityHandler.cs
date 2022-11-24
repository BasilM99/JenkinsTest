using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.DomainServices.EventBroker;
using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;

namespace ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers
{

 

    /// <summary>
    /// Get Account changed entites from EventArgsBase to be refreshed in AdServer cache.
    /// </summary>
    class SSPEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedSSPs = new List<ChangedEntity>();

            if (args != null && args.Data != null)
            {
                foreach (var item in args.Data)
                {
                    if (item is EntityEventData)
                    {
                        var eventData = item as EntityEventData;

                        if (eventData.Entity is IEntity<int>)
                        {
                            IEntity<int> entity = eventData.Entity as IEntity<int>;
                            ChangedEntity changedEntity = null;

                            if (entity is PartnerSite) { changedEntity = HandlePartnerSite(eventData, entity as PartnerSite); }
                            else
                                if (entity is SiteZone) { changedEntity = HandleZoneSite(eventData, entity as SiteZone); }
                            else
                                    if (entity is SiteZoneMapping) { changedEntity = HandleSiteZoneMapping(eventData, entity as SiteZoneMapping); }
                            else
                                        if (entity is DealCampaignMapping) { changedEntity = HandleDealCampaignMapping(eventData, entity as DealCampaignMapping); }
                            else
                                        if (entity is FloorPrice) { changedEntity = HandleFloorPrice(eventData, entity as FloorPrice); }
                            else

                            if (entity is DPPartner || entity is DSPPartner || entity is SSPPartner || entity is BusinessPartner)
                            {

                                changedEntity = HandleBusinessPartner(eventData, entity as BusinessPartner);
                            }
                            else
                            if (entity is PMPDeal)
                            {
                                changedEntity = HandlePMPDeal(eventData, entity as PMPDeal);

                            }
                            else
                            if (entity is Buyer)
                            {
                                changedEntity = HandleBuyer(eventData, entity as Buyer);

                            }
                            // If changedEntity is not null and changedAccounts doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntity != null && changedEntity !=null /*&& changedEntity.DirtyProperties != null&& changedSSPs.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0*/)
                            {
                                changedSSPs.Add(changedEntity);
                            }
                        }
                    }
                }
            }

            return changedSSPs;
        }
        private ChangedEntity HandleFloorPrice(EntityEventData eventData, FloorPrice price)
        {
            ChangedEntity changedzone = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzone = new ChangedEntity(EntityType.DSPSite);
                changedzone.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedzone.DirtyProperties.Add("id", price.Site.ID.ToString());
            }

            return changedzone;

        }
        private ChangedEntity HandleBuyer(EntityEventData eventData, Buyer Buyer)
        {
            ChangedEntity changedzone = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzone = new ChangedEntity(EntityType.Buyer);
                //changedzone.DirtyProperties = new Dictionary<string, object>();
                //// Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedzone.DirtyProperties = new Dictionary<string, object>();
                changedzone.DirtyProperties.Add("id", Buyer.ID.ToString());
            }

            return changedzone;

        }
        private ChangedEntity HandlePMPDeal(EntityEventData eventData, PMPDeal deal)
        {
            ChangedEntity changedzone = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzone = new ChangedEntity(EntityType.BuyerDeal);
                changedzone.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedzone.DirtyProperties.Add("id", deal.ID.ToString());
            }

            return changedzone;

        }
        private ChangedEntity HandleBusinessPartner(EntityEventData eventData, BusinessPartner partner)
        {
            ChangedEntity changedzone = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzone = new ChangedEntity(EntityType.BusinessPartner);
                changedzone.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedzone.DirtyProperties.Add("id", partner.ID.ToString());
            }

            return changedzone;

        }
        private ChangedEntity HandlePartnerSite(EntityEventData eventData, PartnerSite site)
        {
            ChangedEntity changedsite = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedsite = new ChangedEntity(EntityType.DSPSite);
                changedsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedsite.DirtyProperties.Add("id", site.ID.ToString());
            }

            return changedsite;
            
        }
        private ChangedEntity HandleZoneSite(EntityEventData eventData, SiteZone zone)
        {
            ChangedEntity changedzone = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzone = new ChangedEntity(EntityType.DSPSiteZone);
                changedzone.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedzone.DirtyProperties.Add("id", zone.ID.ToString());
            }

            return changedzone;
            
        }

       private ChangedEntity HandleSiteZoneMapping(EntityEventData eventData, SiteZoneMapping zoneMapping)
        {
            ChangedEntity changedzoneMapping= null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedzoneMapping = new ChangedEntity(EntityType.DSPSiteMapping);
               // changedzoneMapping.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
               // changedzoneMapping.DirtyProperties.Add("id", zoneMapping.ID.ToString());
            }

            return changedzoneMapping;
            
        }
       private ChangedEntity HandleDealCampaignMapping(EntityEventData eventData, DealCampaignMapping dealcampaignmapping)
        {
            ChangedEntity changeddealcampaignmapping= null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changeddealcampaignmapping = new ChangedEntity(EntityType.DSPDeal);
                //changeddealcampaignmapping.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
               // changeddealcampaignmapping.DirtyProperties.Add("id", dealcampaignmapping.ID.ToString());
            }

            return changeddealcampaignmapping;
            
        }
       
    }
}
