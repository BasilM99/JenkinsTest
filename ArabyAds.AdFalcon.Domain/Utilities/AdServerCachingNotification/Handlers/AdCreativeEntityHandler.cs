using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
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
    class AdCreativeEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
             List<ChangedEntity> changedEntities = new List<ChangedEntity>();
            IList<int> adIdsTobeBulish = new List<int>();
            IList<int> adIdsTobeBulishReSubmit = new List<int>();
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
                             ChangedEntity changedAdCreativeEntity = null;
                             ChangedEntity changedAdCreativeUnitEntity = null;

                             if (entity is AdCreative) { changedAdCreativeEntity = HandleAdCreative(eventData, entity as AdCreative, adIdsTobeBulish, adIdsTobeBulishReSubmit); }
                             else
                                 if (entity is AdActionValue) { changedAdCreativeEntity = HandleAdActionValue(eventData, entity as AdActionValue); }
                                 else
                                     if (entity is AdActionValueTracker) { changedAdCreativeEntity = HandleAdActionValueTracker(eventData, entity as AdActionValueTracker); }
                                     else
                                     if (entity is AppSiteAdQueue) { changedAdCreativeEntity = HandleAppSiteAdQueue(eventData, entity as AppSiteAdQueue); }
                                     else
                                         if (entity is AdCreativeAttribute) { changedAdCreativeUnitEntity = HandleAdCreativeAttribute(eventData, entity as AdCreativeAttribute); }
                                         else
                                             if (entity is AdCreativeUnitTracker)
                                             {
                                                 var adCreativeUnitTracker = entity as AdCreativeUnitTracker;
                                                 var adCreativeUnit = adCreativeUnitTracker.CreativeUnit;
                                                 changedAdCreativeEntity = HandleAdCreativeUnit(eventData, adCreativeUnit);
                                                 changedAdCreativeUnitEntity = HandleAdCreativeUnitForAdCreativeUnit(eventData, adCreativeUnit);
                                             }
                                             else
                                                 if (entity is AdCreativeUnit)
                                                 {
                                                     var adCreativeUnit = entity as AdCreativeUnit;
                                                     changedAdCreativeEntity = HandleAdCreativeUnit(eventData, adCreativeUnit);
                                                     changedAdCreativeUnitEntity = HandleAdCreativeUnitForAdCreativeUnit(eventData, adCreativeUnit);
                                                 }

                             // If changedAdCreativeEntity is not null and changedEntities doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                             if (changedAdCreativeEntity != null && changedEntities.Where(p => p.EntityType == EntityType.Ad && p.DirtyProperties.ContainsValue(changedAdCreativeEntity.DirtyProperties["id"].ToString())).Count() == 0)
                             {
                                 changedEntities.Add(changedAdCreativeEntity);
                             }


                             // If changedAdCreativeUnitEntity is not null and changedEntities doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                             if (changedAdCreativeUnitEntity != null && changedEntities.Where(p => p.EntityType == EntityType.AdCreativeUnit && p.DirtyProperties.ContainsValue(changedAdCreativeUnitEntity.DirtyProperties["id"].ToString())).Count() == 0)
                             {
                                 changedEntities.Add(changedAdCreativeUnitEntity);
                             }
                         }
                     }
                 }


             }
             if(adIdsTobeBulish!=null && adIdsTobeBulish.Count>0)
            PublishAdCreatPauseEventForKafka(adIdsTobeBulish);

            if (adIdsTobeBulishReSubmit != null && adIdsTobeBulishReSubmit.Count > 0)
                PublishAdCreatResubmitAdEventEventForKafka(adIdsTobeBulishReSubmit);
            return changedEntities;
        }

        private ChangedEntity HandleAdCreativeAttribute(EntityEventData eventData, AdCreativeAttribute adCreativeAttribute)
        {
            ChangedEntity changedAdCreativeUnit = null;

            
            return changedAdCreativeUnit;
        }

        private ChangedEntity HandleAdCreativeUnitForAdCreativeUnit(EntityEventData eventData, AdCreativeUnit adCreativeUnit)
        {
            ChangedEntity changedAdCreativeUnit = null;
            if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.Insert)
            {
                changedAdCreativeUnit = new ChangedEntity(EntityType.AdCreativeUnit);
                changedAdCreativeUnit.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdCreativeUnit.DirtyProperties.Add("id", adCreativeUnit.ID.ToString());
            }
            return changedAdCreativeUnit;
        }

        private ChangedEntity HandleAdCreativeUnit(EntityEventData eventData, AdCreativeUnit adCreativeUnit)
        {
            ChangedEntity changedAdCreative = null;

            if (adCreativeUnit != null && adCreativeUnit.AdCreative != null)
            {
                changedAdCreative = new ChangedEntity(EntityType.Ad);
                changedAdCreative.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdCreative.DirtyProperties.Add("id", adCreativeUnit.AdCreative.ID.ToString());
            }

            return changedAdCreative;
        }

        private ChangedEntity HandleAppSiteAdQueue(EntityEventData eventData, AppSiteAdQueue appSiteAdQueue)
        {
            ChangedEntity changedAdCreative = null;

            if (appSiteAdQueue != null && (appSiteAdQueue.Ad != null || eventData.OldState != null))
            {
                int? adId = null;

                if (appSiteAdQueue.Ad != null)
                {
                    adId = appSiteAdQueue.Ad.ID;
                }
                else
                {
                    foreach (var item in eventData.OldState)
                    {
                        if (item is AdCreative)
                        {
                            adId = (item as AdCreative).ID;
                            break;
                        }
                    }
                }

                if (adId.HasValue)
                {
                    changedAdCreative = new ChangedEntity(EntityType.Ad);
                    changedAdCreative.DirtyProperties = new Dictionary<string, object>();
                    // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                    changedAdCreative.DirtyProperties.Add("id", adId.Value.ToString());
                }
            }

            return changedAdCreative;
        }

        private ChangedEntity HandleAdActionValue(EntityEventData eventData, AdActionValue adActionValue)
        {
            ChangedEntity changedAdCreative = null;

            if (adActionValue != null && adActionValue.AdCreative != null)
            {
                changedAdCreative = new ChangedEntity(EntityType.Ad);
                changedAdCreative.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdCreative.DirtyProperties.Add("id", adActionValue.AdCreative.ID.ToString());
            }

            return changedAdCreative;
        }

        public ChangedEntity HandleAdActionValueTracker(EntityEventData eventData, AdActionValueTracker adActionValueTracker)
        {
            ChangedEntity changedAdCreative = null;

            if (adActionValueTracker != null && adActionValueTracker.AdActionValue != null && adActionValueTracker.AdActionValue.AdCreative != null)
            {
                changedAdCreative = new ChangedEntity(EntityType.Ad);
                changedAdCreative.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdCreative.DirtyProperties.Add("id", adActionValueTracker.AdActionValue.AdCreative.ID.ToString());
            }

            return changedAdCreative;
        }

        private ChangedEntity HandleAdCreative(EntityEventData eventData, AdCreative adCreative, IList<int> ids,IList<int> idsResubmit)
        {
            ChangedEntity changedAdCreative = null;
            if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.Insert)
            {
                changedAdCreative = new ChangedEntity(EntityType.Ad);
                changedAdCreative.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAdCreative.DirtyProperties.Add("id", adCreative.ID.ToString());

               bool statusSent= adCreative.PublishAdStatusInforForKafka(eventData);
                if (statusSent)
                {
                    ids.Add(adCreative.ID);
                }
                bool statusResubmitSent = adCreative.PublishAdStatusInforForKafkaReSubmit(eventData);

                if (statusResubmitSent)
                {
                    idsResubmit.Add(adCreative.ID);
                }

            }

            return changedAdCreative;
        }

        public virtual void PublishAdCreatPauseEventForKafka(IList<int> ids)
        {



            if (Configuration.KafkaEnabled)
                Configuration.KafkaEventPublisher.Publish(new PauseAdEvent { AdIds = ids.ToList(), });
        }
        public virtual void PublishAdCreatResubmitAdEventEventForKafka(IList<int> ids)
        {



            if (Configuration.KafkaEnabled)
                Configuration.KafkaEventPublisher.Publish(new ResubmitAdEvent { AdIds = ids.ToList(), });
        }
    }

}
