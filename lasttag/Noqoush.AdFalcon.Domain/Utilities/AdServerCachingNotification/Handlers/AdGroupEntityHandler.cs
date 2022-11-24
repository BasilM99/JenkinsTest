using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.AdFalcon.EventDTOs;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.DomainServices.EventBroker;
using Noqoush.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers
{
    class AdGroupEntityHandler : ICachingNotificationEntityHandler
    {
        /// <summary>
        /// Get AdGroup changed entites from EventArgsBase to be refreshed in AdServer cache.
        /// </summary>
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedAdGroups = new List<ChangedEntity>();

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
                            ChangedEntity changedEntityE = null;
                            ChangedEntity changedEntityC = null;

                            if (entity is AdGroup)
                            {
                                changedEntity = HandleAdGroup(eventData, entity as AdGroup);
                            }
                            else
                                if (entity is AdGroupCostElement)
                                {
                                    changedEntity = HandleAdGroupCostElement(eventData, entity as AdGroupCostElement);
                                }
                            else
                                    if (entity is AdGroupFee)
                            {
                                changedEntity = HandleAdGroupFee(eventData, entity as AdGroupFee);
                            }
                            else
                                    if (entity is DeviceTargetingBase)
                                    {
                                        changedEntity = HandleDeviceTargetingBase(eventData, entity as DeviceTargetingBase);
                                    }
                                    else
                                        if (entity is ITargetingBase)
                                        {
                                            changedEntity = HandleTargeting(eventData, entity as ITargetingBase);
                                        }
                                        else
                                            if (entity is AdGroupTrackingEvent)
                                            {
                                                changedEntity = HandleAdGroupTrackingEventUpdaed(eventData, entity as AdGroupTrackingEvent);
                                            }
                                        else
                                        if (entity is AdGroupConversionEvent)
                                        {
                                            changedEntity = HandleAdGroupConversionEvent(eventData, entity as AdGroupConversionEvent);


                                                if ((entity as AdGroupConversionEvent).AdGroup != null)
                                                {
                                                    //results = HandleAdGroupCampaign(eventData, entity as AdGroup);


                                                    var changedEntityAdGroup = HandleAdGroup(eventData, (entity as AdGroupConversionEvent).AdGroup as AdGroup);

                                                    if (changedEntityAdGroup != null)
                                                    {
                                                        //foreach (var itemt in results)
                                                        //{
                                                            

                                                        if (changedEntityAdGroup != null && changedAdGroups.Where(p => p.DirtyProperties.ContainsValue(changedEntityAdGroup.DirtyProperties["id"].ToString())).Count() == 0)
                                                        {
                                                            changedAdGroups.Add(changedEntityAdGroup);
                                                        }

                                        //}
                                    }
                                                }

                            }
                                         else if (entity is AdGroupBidConfig)
                                            {
                                                changedEntity = HandleCampaignBidConfig(eventData, entity as AdGroupBidConfig);
                                            }
                                            else if (entity is AdGroupInventorySource)
                                            {
                                                changedEntity = HandleAdGroupInventorySource(eventData, entity as AdGroupInventorySource);
                                            }
                                            else if (entity is AdGroupDynamicBiddingConfig)
                                            {
                                                changedEntity = HandleAdGroupDynamicBiddingConfig(eventData, entity as AdGroupDynamicBiddingConfig);
                                            }
                                            else if (entity is PixelEventMap)
                                            {
                                                changedEntity = HandlePixelEventMap(eventData, entity as PixelEventMap);
                                             changedEntityC = HandlePixelMap(eventData, entity as PixelEventMap);
                                                }
                                            else if (entity is AudienceSegmentEventMap)
                                            {
                                                changedEntity = HandleAudienceSegmentEventMap(eventData, entity as AudienceSegmentEventMap);
                                                changedEntityE = HandleAudienceSegmentMap(eventData, entity as AudienceSegmentEventMap);
                                         }   




                            // If changedEntity is not null and changedAdGroups doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntity != null && changedAdGroups.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAdGroups.Add(changedEntity);
                            }

                            // If changedEntity is not null and changedAdGroups doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntityE != null && changedAdGroups.Where(p => p.DirtyProperties.ContainsValue(changedEntityE.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAdGroups.Add(changedEntityE);
                            }


                            // If changedEntity is not null and changedAdGroups doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntityC != null && changedAdGroups.Where(p => p.DirtyProperties.ContainsValue(changedEntityC.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAdGroups.Add(changedEntityC);
                            }
                        }
                    }
                }
            }

            return changedAdGroups;
        }

        private ChangedEntity HandleAudienceSegmentMap(EntityEventData eventData, AudienceSegmentEventMap item)
        {
            ChangedEntity changedAccount = null;
            if ((eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection) && item.Event != null)
            {
                changedAccount = new ChangedEntity(EntityType.AudienceSegment);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.AudienceSegment.ID.ToString());
            }

            return changedAccount;

        }

        private ChangedEntity HandlePixelMap(EntityEventData eventData, PixelEventMap item)
        {
            ChangedEntity changedAccount = null;
            if ((eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection) && item.Event != null)
            {
                changedAccount = new ChangedEntity(EntityType.Pixel);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.Pixel.ID.ToString());
            }

            return changedAccount;

        }
        private ChangedEntity HandleAudienceSegmentEventMap(EntityEventData eventData, AudienceSegmentEventMap item)
        {
            ChangedEntity changedAccount = null;
            if ((eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection) &&   item.Event != null)
            {
                changedAccount = new ChangedEntity(EntityType.AdGroupEvent);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.Event.ID.ToString());
            }

            return changedAccount;

        }

        private ChangedEntity HandlePixelEventMap(EntityEventData eventData, PixelEventMap item)
        {
            ChangedEntity changedAccount = null;
            if ((eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection) && item.Event!=null)
            {
                changedAccount = new ChangedEntity(EntityType.AdGroupEvent);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.Event.ID.ToString());
            }

            return changedAccount;

        }


        private ChangedEntity HandleAdGroupTrackingEventUpdaed(EntityEventData eventData, AdGroupTrackingEvent item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.AdGroupEvent);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.ID.ToString());
            }

            return changedAccount;

        }

        private ChangedEntity HandleAdGroupConversionEvent(EntityEventData eventData, AdGroupConversionEvent item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.AdGroupEvent);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.ID.ToString());
            }

            return changedAccount;

        }
        private ChangedEntity HandleAdGroupInventorySource(EntityEventData eventData, AdGroupInventorySource campaignAssignedAppsite)
        {
            ChangedEntity changedAdGroup = null;
            if (campaignAssignedAppsite != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", campaignAssignedAppsite.AdGroup.ID.ToString());
            }

            return changedAdGroup;

        }
        private ChangedEntity HandleAdGroupDynamicBiddingConfig(EntityEventData eventData, AdGroupDynamicBiddingConfig adGroupDynamicBiddingConfig)
        {
            ChangedEntity changedAdGroup = null;
            if (adGroupDynamicBiddingConfig != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adGroupDynamicBiddingConfig.AdGroup.ID.ToString());
            }

            return changedAdGroup;

        }
        private ChangedEntity HandleDeviceTargetingBase(EntityEventData eventData, DeviceTargetingBase deviceTargetingBase)
        {
            ChangedEntity changedAdGroup = null;
            if (deviceTargetingBase != null && (deviceTargetingBase.AdGroup != null || deviceTargetingBase.DeviceTargeting.AdGroup != null))
            {
                AdGroup adgroup = null;

                if (deviceTargetingBase.AdGroup != null)
                {
                    adgroup = deviceTargetingBase.AdGroup;
                }
                else
                {
                    adgroup = deviceTargetingBase.DeviceTargeting.AdGroup;
                }

                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adgroup.ID.ToString());

            }

            return changedAdGroup;
        }

        private ChangedEntity HandleTargeting(EntityEventData eventData, ITargetingBase targetingBase)
        {
            ChangedEntity changedAdGroup = null;
            if (targetingBase != null && (targetingBase.AdGroup != null))
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", targetingBase.AdGroup.ID.ToString());

            }

            return changedAdGroup;
        }

        private ChangedEntity HandleAdGroupCostElement(EntityEventData eventData, AdGroupCostElement adGroupCostElement)
        {
            ChangedEntity changedAdGroup = null;
            if (adGroupCostElement != null && adGroupCostElement.AdGroup != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adGroupCostElement.AdGroup.ID.ToString());
            }

            return changedAdGroup;
        }
        private ChangedEntity HandleAdGroupFee(EntityEventData eventData, AdGroupFee adGroupCostElement)
        {
            ChangedEntity changedAdGroup = null;
            if (adGroupCostElement != null && adGroupCostElement.AdGroup != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adGroupCostElement.AdGroup.ID.ToString());
            }

            return changedAdGroup;
        }
        private ChangedEntity HandleAdGroup(EntityEventData eventData, AdGroup adGroup)
        {
            ChangedEntity changedAdGroup = null;
            if (eventData.ActionType == ObjectActionEnum.Insert ||  eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adGroup.ID.ToString());
                adGroup.PublishAdGroupBillingInforForKafka(eventData);
            }

            return changedAdGroup;

        }

        private ChangedEntity HandleAdGroupTrackingEvent(EntityEventData eventData, AdGroupTrackingEvent adGroupTrackingEvent)
        {
            ChangedEntity changedAdGroup = null;
            if (adGroupTrackingEvent != null && adGroupTrackingEvent.AdGroup != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", adGroupTrackingEvent.AdGroup.ID.ToString());
            }

            return changedAdGroup;

        }

        private ChangedEntity HandleCampaignBidConfig(EntityEventData eventData, AdGroupBidConfig campaignBidConfig)
        {
            ChangedEntity changedAdGroup = null;
            if (campaignBidConfig != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", campaignBidConfig.AdGroup.ID.ToString());
            }

            return changedAdGroup;

        }
    }
}
