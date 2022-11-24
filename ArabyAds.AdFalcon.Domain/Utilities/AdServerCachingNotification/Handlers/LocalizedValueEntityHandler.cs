//using ArabyAds.AdFalcon.Base;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.DomainServices.EventBroker;
using ArabyAds.Framework.DomainServices.Localization;
using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers
{
    /// <summary>
    /// Get LocalizedValue changed entites from EventArgsBase to be refreshed in AdServer cache.
    /// </summary>
    class LocalizedValueEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedLocalizedValues = new List<ChangedEntity>();

            if (args != null && args.Data != null)
            {
                foreach (var item in args.Data)
                {
                    if (item is EntityEventData)
                    {
                        var eventData = item as EntityEventData;

                        // For now, we only handle changes in keyword
                        if (eventData.Entity is LocalizedValue && eventData.ActionType == ObjectActionEnum.Update)
                        {
                            var localizedValue = eventData.Entity as LocalizedValue;
                            if (localizedValue.LocalizedString != null)
                            {
                                switch (localizedValue.LocalizedString.GroupKey.ToLower())
                                {
                                    case "keyword":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Keyword).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Keyword);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "location":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Location).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Location);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "operator":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Operator).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Operator);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "device":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Device).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Device);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "platform":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Platform).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Platform);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "manufacturer":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Manufacturer).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Manufacturer);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "attributes":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.AdCreativeAttribute).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.AdCreativeAttribute);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    case "advertiser":
                                        if (changedLocalizedValues.Where(p => p.EntityType == EntityType.Advertiser).Count() == 0)
                                        {
                                            ChangedEntity changedEntity = new ChangedEntity(EntityType.Advertiser);
                                            changedEntity.DirtyProperties = new Dictionary<string, object>();
                                            changedLocalizedValues.Add(changedEntity);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return changedLocalizedValues;
        }
    }
}
