
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.Framework;
using ArabyAds.Framework.DomainServices.EventBroker;
using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification
{
    

    /// <summary>
    /// Notify AdServer for any change on particular domain entities to refresh the cache.
    /// </summary>
    public class CachingNotification : ISubscriberHandler
    {
        /// <summary>
        /// ICommunicationService field to be used to call CommunicationService in AdServer
        /// </summary>
       // private static IEntityUpdatesBroadcasterService _communicationService;

        /// <summary>
        /// List of ICachingNotificationEntityHandler handlers that will be called to handle event entities
        /// </summary>
        private static List<ICachingNotificationEntityHandler> _cachingHandlers;

        private const string AdServerCachingNotificationException = "AdServerCachingNotificationException";
 
        static CachingNotification()
        {
            // Get all event entities cache handlers
            _cachingHandlers = GetCachingHandlers();
         
            //_communicationService = IoC.Instance.Resolve<IEntityUpdatesBroadcasterService>();
        }

        public void HandleEvent(EventArgsBase args)
        {
            if (args.Data != null && args.Data.Count != 0)
            {
                // Get all changed entities
                List<ChangedEntity> changedItems = GetChangedItems(args);

                // Filter if there is duplicate entiries
                changedItems = FilterChangedItems(changedItems);

                // Call CommunicationService to refresh cache in AdServer
                CallCommunicationService(changedItems);
            }
        }



        #region Private Members

        /// <summary>
        /// Get the event entities cache handlers from the current Assembly by reflection
        /// </summary>
        /// <returns></returns>
        private static List<ICachingNotificationEntityHandler> GetCachingHandlers()
        {
            List<ICachingNotificationEntityHandler> cachingHandlers = new List<ICachingNotificationEntityHandler>();

            // Get the current running Assembly
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            // Get event entities cache handlers from the current assembly
            foreach (var type in currentAssembly.GetTypes())
            {
                if (!type.IsInterface && typeof(ICachingNotificationEntityHandler).IsAssignableFrom(type))
                {
                    var entityHandler = (ICachingNotificationEntityHandler)Activator.CreateInstance(type);
                    cachingHandlers.Add(entityHandler);
                }
            }

            return cachingHandlers;
        }

        /// <summary>
        /// Get all changed items by calling caching handlers
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<ChangedEntity> GetChangedItems(EventArgsBase args)
        {
            List<ChangedEntity> changedItems = new List<ChangedEntity>();

            foreach (var handler in _cachingHandlers)
            {
                var changedEntities = handler.HandleEntities(args);

                if (changedEntities != null && changedEntities.Count != 0) { changedItems.AddRange(changedEntities); }
            }

            return changedItems;
        }

        /// <summary>
        /// Call CommunicationService to refresh cache
        /// </summary>
        /// <param name="changedItems"></param>
        private void CallCommunicationService(List<ChangedEntity> changedItems)
        {
            bool isSuccess = true;
            IList<object> ids = null;
            IList<string> idstr = null;
            foreach (var item in changedItems)
            {
                try
                {
                    ids=item.DirtyProperties != null && item.DirtyProperties.Count == 1 ?  item.DirtyProperties.Values.ToList()  : null;
                    if(ids!=null)
                    idstr= ids.Select(o => o.ToString()).ToList();

                 Configuration.KafkaEventPublisher.Publish(new EntityUpdateEvent { EntityType = item.EntityType, EntityIds = idstr });

                   // _communicationService.Broadcast(item.EntityType, item.DirtyProperties != null && item.DirtyProperties.Count == 1 ? item.DirtyProperties.First().Value.ToString() : null);
                }
                catch (Exception x)
                {
                    isSuccess = false;
                    Framework.ApplicationContext.Instance.Logger.Error(AdServerCachingNotificationException, x);
                }
            }

            if (!isSuccess)
            {
                throw new Exception(AdServerCachingNotificationException);
            }
        }

        /// <summary>
        /// Filter the list of changed items and remove duplicates
        /// </summary>
        /// <param name="changedItems"></param>
        /// <returns></returns>
        private List<ChangedEntity> FilterChangedItems(List<ChangedEntity> changedItems)
        {
            List<ChangedEntity> filteredList = new List<ChangedEntity>();

            if (changedItems != null && changedItems.Count != 0)
            {
                foreach (var item in changedItems)
                {
                    if (item.DirtyProperties == null || item.DirtyProperties.Count == 0)
                    {
                        if (filteredList.Where(p => p.EntityType == item.EntityType).Count() == 0)
                        {
                            filteredList.Add(item);
                        }
                    }
                    else
                    {
                        if (filteredList.Where(p => p.EntityType == item.EntityType && p.DirtyProperties.First().Value.ToString()== item.DirtyProperties.First().Value.ToString()).Count() == 0)
                        {
                            filteredList.Add(item);
                        }
                    }
                }
            }

            return filteredList;
        }

        #endregion

    }
}
