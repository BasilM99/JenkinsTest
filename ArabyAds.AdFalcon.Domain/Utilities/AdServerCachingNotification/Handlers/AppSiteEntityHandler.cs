using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.AppSite.Filtering;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.DomainServices.EventBroker;
using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers
{
    /// <summary>
    /// Get Account changed entites from EventArgsBase to be refreshed in AdServer cache.
    /// </summary>
    class AppSiteEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedAppSites = new List<ChangedEntity>();

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

                            if (entity is AppSite) { changedEntity = HandleAppSite(eventData, entity as AppSite); }
                            else
                                 if (entity is AppSiteServerSetting) { changedEntity = HandleAppSiteAppSiteServerSetting(eventData, entity as AppSiteServerSetting); }
                                else
                                    if (entity is AppSiteSetting) { changedEntity = HandleAppSiteSetting(eventData, entity as AppSiteSetting); }
                                    else
                                        if (entity is AppSiteFilter) { changedEntity = HandleAppSiteFilter(eventData, entity as AppSiteFilter); }
                                        else
                                            if (entity is AppSiteRevenueCalculationSetting) { changedEntity = HandleAppSiteRevenueCalculationSetting(eventData, entity as AppSiteRevenueCalculationSetting); }
                                            else
                                                if (entity is AppSiteKeyword) { changedEntity = HandleAppSiteKeyword(eventData, entity as AppSiteKeyword); }
                                                else
                                                    if (entity is AppSiteEvent) { changedEntity = HandleAppSiteEvent(eventData, entity as AppSiteEvent); }
                            
                            // If changedEntity is not null and changedAppSites doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntity != null && changedAppSites.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAppSites.Add(changedEntity);
                            }
                        }
                    }
                }

            }

            return changedAppSites;
        }

        private ChangedEntity HandleAppSiteKeyword(EntityEventData eventData, AppSiteKeyword appSiteKeyword)
        {
            ChangedEntity changedAppsite = null;

            if (appSiteKeyword != null && appSiteKeyword.AppSite != null)
            {
                changedAppsite = new ChangedEntity(EntityType.AppSite);
                changedAppsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAppsite.DirtyProperties.Add("id", appSiteKeyword.AppSite.ID.ToString());

            }

            return changedAppsite;
        }

        private ChangedEntity HandleAppSiteRevenueCalculationSetting(EntityEventData eventData, AppSiteRevenueCalculationSetting revenueSetting)
        {
            ChangedEntity changedAppsite = null;

            changedAppsite = new ChangedEntity(EntityType.AppSite);
            changedAppsite.DirtyProperties = new Dictionary<string, object>();
            // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
            changedAppsite.DirtyProperties.Add("id", revenueSetting.AppSite.ID.ToString());

            return changedAppsite;
        }

        private ChangedEntity HandleAppSiteFilter(EntityEventData eventData, AppSiteFilter appSiteFilter)
        {
            ChangedEntity changedAppsite = null;

            changedAppsite = new ChangedEntity(EntityType.AppSite);
            changedAppsite.DirtyProperties = new Dictionary<string, object>();
            // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
            changedAppsite.DirtyProperties.Add("id", appSiteFilter.CAppSiteId.ToString());

            return changedAppsite;
        }

        private ChangedEntity HandleAppSiteSetting(EntityEventData eventData, AppSiteSetting appSiteSetting)
        {
            ChangedEntity changedAppsite = null;

            if (appSiteSetting != null && appSiteSetting.AppSite != null)
            {
                changedAppsite = new ChangedEntity(EntityType.AppSite);
                changedAppsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAppsite.DirtyProperties.Add("id", appSiteSetting.AppSite.ID.ToString());
            }


            return changedAppsite;
        }

        private ChangedEntity HandleAppSiteAppSiteServerSetting(EntityEventData eventData, AppSiteServerSetting appSiteServerSetting)
        {
            ChangedEntity changedAppsite = null;

            if (eventData.ActionType == ObjectActionEnum.Update|| eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.Update)
            {
                changedAppsite = new ChangedEntity(EntityType.AppSite);
                changedAppsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAppsite.DirtyProperties.Add("id", appSiteServerSetting.AppSite.ID.ToString());
            }

            
            return changedAppsite;
        }

        private ChangedEntity HandleAppSite(EntityEventData eventData, AppSite appSite)
        {
            ChangedEntity changedAppsite = null;
            if (eventData.ActionType == ObjectActionEnum.Insert ||  eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedAppsite = new ChangedEntity(EntityType.AppSite);
                changedAppsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAppsite.DirtyProperties.Add("id", appSite.ID.ToString());
            }

            return changedAppsite;
        }


        private ChangedEntity HandleAppSiteEvent(EntityEventData eventData, AppSiteEvent appSiteEvent)
        {
            ChangedEntity changedAppsite = null;

            if (appSiteEvent != null && appSiteEvent.AppSiteServerSetting != null && appSiteEvent.AppSiteServerSetting.AppSite != null)
            {
                changedAppsite = new ChangedEntity(EntityType.AppSite);
                changedAppsite.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAppsite.DirtyProperties.Add("id", appSiteEvent.AppSiteServerSetting.AppSite.ID.ToString());
            }


            return changedAppsite;
        }
    }
}
