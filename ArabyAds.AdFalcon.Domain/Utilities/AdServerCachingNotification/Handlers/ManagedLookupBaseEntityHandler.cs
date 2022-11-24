using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.AdFalcon.EventDTOs;
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
    /// Get lookup changed entites from EventArgsBase to be refreshed in AdServer cache.
    /// </summary>
    class ManagedLookupBaseEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedLookups = new List<ChangedEntity>();

            if (args != null && args.Data != null)
            {
                foreach (var item in args.Data)
                {
                    if (item is EntityEventData)
                    {
                        var eventData = item as EntityEventData;

                        var entity = eventData.Entity;
                        if (entity is ManagedLookupBase)
                        {
                            ChangedEntity changedEntity = null;

                            if (entity is Keyword)
                            {
                                changedEntity = HandleKeyword(eventData, entity as Keyword);
                            }
                            else
                                if (entity is LocationBase)
                                {
                                    changedEntity = HandleLocation(eventData, entity as LocationBase);
                                }
                                else
                                {
                                    if (entity is CostElement)
                                    {
                                        changedEntity = HandleCostElement(eventData, entity as CostElement);
                                    }
                                    else if (entity is Fee)
                                    {
                                        changedEntity = HandleFee(eventData, entity as Fee);
                                    }
                                else if (entity is Advertiser)
                                    {
                                        changedEntity = HandleAdvertiser(eventData, entity as Advertiser);
                                    }
                                    else
                                    {
                                        changedEntity = HandleGeneric(eventData, entity as ManagedLookupBase);
                                    }
                                }

                            if (changedEntity != null && changedLookups.Where(p => p.EntityType == changedEntity.EntityType).Count() == 0)
                            {
                                changedLookups.Add(changedEntity);
                            }
                        }
                    }

                }
            }

            return changedLookups;
        }

        private ChangedEntity HandleLocation(EntityEventData eventData, LocationBase locationBase)
        {
            ChangedEntity changedLocation = new ChangedEntity(EntityType.Location);
            changedLocation.DirtyProperties = new Dictionary<string, object>();

            return changedLocation;
        }

        private ChangedEntity HandleCostElement(EntityEventData eventData, CostElement costElement)
        {
            ChangedEntity changedCostElement = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedCostElement = new ChangedEntity(EntityType.CostItem);
                changedCostElement.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later

                changedCostElement.DirtyProperties.Add("id", costElement.ID);
            }

            return changedCostElement;
        }
        private ChangedEntity HandleFee(EntityEventData eventData, Fee costElement)
        {
            ChangedEntity changedCostElement = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedCostElement = new ChangedEntity(EntityType.CostItem);
                changedCostElement.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later

                changedCostElement.DirtyProperties.Add("id", costElement.ID);
            }

            return changedCostElement;
        }

        private ChangedEntity HandleGeneric(EntityEventData eventData, ManagedLookupBase entity)
        {
            ChangedEntity changedLookup = null;

            EntityType entityType;
            bool parseResult = Enum.TryParse<EntityType>(entity.GetType().Name, true, out entityType);

            if (parseResult)
            {
                changedLookup = new ChangedEntity(entityType);

            }
            return changedLookup;
        }

        private ChangedEntity HandleKeyword(EntityEventData eventData, Keyword keyword)
        {
            ChangedEntity changedKeyword = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedKeyword = new ChangedEntity(EntityType.Keyword);
            }

            return changedKeyword;
        }


        private ChangedEntity HandleAdvertiser(EntityEventData eventData, Advertiser Advertiser)
        {
            ChangedEntity changedAdvertiser = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedAdvertiser = new ChangedEntity(EntityType.Advertiser);
            }

            return changedAdvertiser;
        }
    }
}
