using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
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
    class AccountEntityHandler : ICachingNotificationEntityHandler
    {
        public List<ChangedEntity> HandleEntities(EventArgsBase args)
        {
            List<ChangedEntity> changedAccounts = new List<ChangedEntity>();
            List<ChangedEntity> changedContentLists = new List<ChangedEntity>();

            List<ChangedEntity> changedPixelLists = new List<ChangedEntity>();
            List<ChangedEntity> changedAdGroupEventLists = new List<ChangedEntity>();
            //ChangedEntity changedEntityC = null;
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
                            ChangedEntity changedEntityB = null;
                            ChangedEntity changedEntityC = null;
                            if (entity is Account) { changedEntity = HandleAccount(eventData, entity as Account); }
                            else
                                if (entity is AccountSummary) { changedEntity = HandleAccountSummary(eventData, entity as AccountSummary); }
                            else
                                    if (entity is AccountDiscount) { changedEntity = HandleAccountDiscount(eventData, entity as AccountDiscount); }
                            else
                                        if (entity is User) {


                                User usr = entity as User;
                                foreach (var usracc in usr.UserAccounts)
                                {
                                    changedEntity = HandleUserAccount(eventData, usracc.Account.ID);

                                    if (changedEntity != null && changedAccounts.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0)
                                    {
                                        changedAccounts.Add(changedEntity);
                                    }
                                }


                            }
                            else
                            if ( entity is AdvertiserAccountMasterAppSite)
                            {
                                changedEntityC= HandleAdvertiserAccountMasterAppSite(eventData, entity as AdvertiserAccountMasterAppSite);

                            }
                            else
                            if (entity is Pixel)
                            {
                                changedEntityB = HandleAdvertiserPixel(eventData, entity as Pixel);

                            }
                            else
                            if (entity is AudienceSegment)
                            {
                                changedEntityE = HandleAudienceSegment(eventData, entity as AudienceSegment);

                            }

                            //else
                            //if (entity is AdGroupTrackingEvent)
                            //{
                            //    changedEntityE = HandleAdGroupTrackingEvent(eventData, entity as AdGroupTrackingEvent);

                            //}

                            //else
                            //if (entity is AdGroupConversionEvent)
                            //{
                            //    changedEntityE = HandleAdGroupConversionEvent(eventData, entity as AdGroupConversionEvent);

                            //}
                            //else
                            //if (entity is AdvertiserAccountMasterAppSiteItem)
                            //{
                            //    changedEntityC= HandleAdvertiserAccountMasterAppSiteItem(eventData, entity as AdvertiserAccountMasterAppSiteItem);

                            //}
                            // If changedEntity is not null and changedAccounts doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntity != null && changedAccounts.Where(p => p.DirtyProperties.ContainsValue(changedEntity.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAccounts.Add(changedEntity);
                            }

                            // If changedEntity is not null and changedAccounts doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntityC != null && changedContentLists.Where(p => p.DirtyProperties.ContainsValue(changedEntityC.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedContentLists.Add(changedEntityC);
                                //changedAccounts.Add(changedEntityC);
                            }


                            // If changedEntity is not null and changedAccounts doesnt contain this entity ( to prevent duplicate refresh the cache for the same entity
                            if (changedEntityB != null && changedPixelLists.Where(p => p.DirtyProperties.ContainsValue(changedEntityB.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedPixelLists.Add(changedEntityB);
                                //changedAccounts.Add(changedEntityC);
                            }
                            if (changedEntityE != null && changedAdGroupEventLists.Where(p => p.DirtyProperties.ContainsValue(changedEntityE.DirtyProperties["id"].ToString())).Count() == 0)
                            {
                                changedAdGroupEventLists.Add(changedEntityE);
                                //changedAccounts.Add(changedEntityC);
                            }
                        }
                    }
                }
            }
            if (changedContentLists!=null)
            {
                foreach (var item in changedContentLists)
                {
                    changedAccounts.Add(item);
                }
            }
            if (changedPixelLists != null)
            {
                foreach (var item in changedPixelLists)
                {
                    changedAccounts.Add(item);
                }
            }
            if (changedAdGroupEventLists != null)
            {
                foreach (var item in changedAdGroupEventLists)
                {
                    changedAccounts.Add(item);
                }
            }

            
            return changedAccounts;
        }
        private ChangedEntity HandleAudienceSegment(EntityEventData eventData, AudienceSegment item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.AudienceSegment);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.ID.ToString());
            }

            return changedAccount;

        }
        private ChangedEntity HandleAccount(EntityEventData eventData, Account account)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert  || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedAccount = new ChangedEntity(EntityType.Account);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", account.ID.ToString());
            }

            return changedAccount;
            
        }

        private ChangedEntity HandleAccountSummary(EntityEventData eventData, AccountSummary accountSummary)
        {
            ChangedEntity changedAccount = null;

            if ((eventData.ActionType == ObjectActionEnum.Insert ||  eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete) && accountSummary != null && accountSummary.Account != null)
            {
                changedAccount = new ChangedEntity(EntityType.Account);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", accountSummary.Account.ID.ToString());

                accountSummary.PublishAccountAmountForKafka(); 
            }

            return changedAccount;
            
        }

        private ChangedEntity HandleAccountDiscount(EntityEventData eventData, AccountDiscount accountDiscount)
        {
            ChangedEntity changedAccount = null;
            if (accountDiscount != null && accountDiscount.Account != null)
            {
                changedAccount = new ChangedEntity(EntityType.Account);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", accountDiscount.Account.ID.ToString());
            }

            return changedAccount;
        }

        /*private ChangedEntity HandleUser(EntityEventData eventData, User user)
       {
            ChangedEntity changedAccount = null;

            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedAccount = new ChangedEntity(EntityType.Account);
                changedAccount.DirtyProperties = new Dictionary<string, object>();

                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", user.Account.ID.ToString());
            }

            return changedAccount;
        }
        */
        private ChangedEntity HandleUserAccount(EntityEventData eventData, int AccountId)
        {
            ChangedEntity changedAccount = null;

            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete)
            {
                changedAccount = new ChangedEntity(EntityType.Account);
                changedAccount.DirtyProperties = new Dictionary<string, object>();

                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", AccountId.ToString());
            }

            return changedAccount;
        }


        private ChangedEntity HandleAdvertiserAccountMasterAppSite(EntityEventData eventData, AdvertiserAccountMasterAppSite item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.ContentList);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.ID.ToString());
            }

            return changedAccount;

        }
        private ChangedEntity HandleAdvertiserPixel(EntityEventData eventData, Pixel item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.Pixel);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.ID.ToString());
            }

            return changedAccount;

        }
        private ChangedEntity HandleAdGroupTrackingEvent(EntityEventData eventData, AdGroupTrackingEvent item)
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
        private ChangedEntity HandleAdvertiserAccountMasterAppSiteItem(EntityEventData eventData, AdvertiserAccountMasterAppSiteItem item)
        {
            ChangedEntity changedAccount = null;
            if (eventData.ActionType == ObjectActionEnum.Insert || eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.Delete || eventData.ActionType == ObjectActionEnum.UpdatCollection || eventData.ActionType == ObjectActionEnum.RemoveCollection || eventData.ActionType == ObjectActionEnum.RecreateCollection)
            {
                changedAccount = new ChangedEntity(EntityType.ContentList);
                changedAccount.DirtyProperties = new Dictionary<string, object>();
                // Use (ToString()) to make all Ids in string so we can filter duplicate objects later
                changedAccount.DirtyProperties.Add("id", item.Link.ID.ToString());
            }

            return changedAccount;

        }
    }
}
