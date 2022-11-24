using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.DomainServices.EventBroker;
using Noqoush.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Utility.ServicesSubscribers
{
    public class CampaignSubscriber : Framework.EventBroker.ISubscriberHandler
    {
        protected const string Save_Campaign = "Campaign Save";
        protected const string Clone_Campaign = "Copy Campaign";
        protected const string EventArgsMissingForEvent = "Event args Missing for '{0}' Event";

        #region Private Members

        protected ICampaignRepository campaignRepository = Framework.IoC.Instance.Resolve<ICampaignRepository>();

        #endregion

        public void HandleEvent(EventArgsBase args)
        {
            switch (args.EventName)
            {
                case Clone_Campaign:
                case Save_Campaign:
                    CreateCampaignFolder(args);
                    break;
            }
        }

        #region Private Members

        private void CreateCampaignFolder(EventArgsBase args)
        {
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is Campaign)
                {
                    eventData = eventDataItem;
                    break;
                }
            }
          
            if (eventData == null)
            {
                throw new Exception(string.Format(EventArgsMissingForEvent, args.EventName));
            }

            if (eventData.ActionType != ObjectActionEnum.Insert)
                return;

            var campaignObj = campaignRepository.Get((eventData.Entity as IEntity<int>).ID);

            if (campaignObj == null)
            {
                return;
            }

            campaignObj.CreateCampaignFolder();
        }

        #endregion
    }
}
