using Noqoush.AdFalcon.Domain.Model.Campaign;
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
    class CampaignEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedCampaigns = new List<ChangedEntity>();

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

                            if (entity is Campaign) { changedEntity = HandleCampaign(eventData, entity as Campaign); }
                           else if (entity is AdGroup) { changedEntity = HandleAdGroupCampaign(eventData, entity as AdGroup); }
                            else
                            {
                                if (entity is CampaignServerSetting)
                                {
                                    changedEntity = HandleCampaignServerSetting(eventData, entity as CampaignServerSetting);
                                }
                          
                                else if (entity is CampaignAssignedAppsite && !(entity is AdGroupInventorySource))
                                {
                                    changedEntity = HandleCampaignAssignedAppsite(eventData, entity as CampaignAssignedAppsite);
                                    var results = HandleCampaignAssignedAppsiteAdGroup(eventData, entity as CampaignAssignedAppsite);

                                    if (results != null)
                                    {
                                        foreach (var itemt in results)
                                        {
                                            if (itemt != null && changedCampaigns.Where(p => p.DirtyProperties.ContainsValue(itemt.DirtyProperties["id"].ToString())).Count() == 0)
                                            {
                                                changedCampaigns.Add(itemt);
                                            }
                                        }
                                    }
                                }

                            }

                            // If changedEntity is not null and changedCampaigns doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntity != null && changedCampaigns.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedCampaigns.Add(changedEntity);
                              

                            }
                        }
                    }
                }
            }

            return changedCampaigns;
        }

        private ChangedEntity HandleCampaignServerSetting(EntityEventData eventData, CampaignServerSetting campaignServerSetting)
        {
            ChangedEntity changedCampaign = null;

            changedCampaign = new ChangedEntity(EntityType.Campaign);
            changedCampaign.DirtyProperties = new Dictionary<string, object>();
            // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
            changedCampaign.DirtyProperties.Add("id", campaignServerSetting.Campaign.ID.ToString());

            return changedCampaign;
        }

        private ChangedEntity HandleCampaign(EntityEventData eventData, Campaign campaign)
        {
            ChangedEntity changedCampaign = null;
            if (eventData.ActionType == ObjectActionEnum.Insert ||  eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection ||  eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedCampaign = new ChangedEntity(EntityType.Campaign);
                changedCampaign.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedCampaign.DirtyProperties.Add("id", campaign.ID.ToString());
                campaign.PublishCampBillingInforForKafka(eventData);
            }

            return changedCampaign;
        }


        private ChangedEntity HandleAdGroupCampaign(EntityEventData eventData, AdGroup campaignAdGroup)
        {
            ChangedEntity changedCampaign = null;
            if ((eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection) && campaignAdGroup.Campaign!=null )
            {
                changedCampaign = new ChangedEntity(EntityType.Campaign);
                changedCampaign.DirtyProperties = new Dictionary<string, object>();
                
                changedCampaign.DirtyProperties.Add("id", campaignAdGroup.Campaign.ID.ToString());
               
            }

            return changedCampaign;
        }
        private ChangedEntity HandleCampaignAssignedAppsite(EntityEventData eventData, CampaignAssignedAppsite campaignAssignedAppsite)
        {
            ChangedEntity changedAdGroup = null;
            if (campaignAssignedAppsite != null)
            {
                changedAdGroup = new ChangedEntity(EntityType.Campaign);
                changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdGroup.DirtyProperties.Add("id", campaignAssignedAppsite.Campaign.ID.ToString());
            }

            return changedAdGroup;

        }
        private IList<ChangedEntity> HandleCampaignAssignedAppsiteAdGroup(EntityEventData eventData, CampaignAssignedAppsite campaignAssignedAppsite)
        {
            IList<ChangedEntity> changedAdGroups = null;
            ChangedEntity changedAdGroup = null;
            if (campaignAssignedAppsite != null)
            {
                changedAdGroups = new List<ChangedEntity>();
                foreach (var  adgroup in campaignAssignedAppsite.Campaign.GetGroups())
                {
                    changedAdGroup = new ChangedEntity(EntityType.AdGroup);
                    changedAdGroup.DirtyProperties = new Dictionary<string, object>();
                    // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                    changedAdGroup.DirtyProperties.Add("id", adgroup.ID.ToString());
                    changedAdGroups.Add(changedAdGroup);
                }
            }

            return changedAdGroups;

        }
    }
}
