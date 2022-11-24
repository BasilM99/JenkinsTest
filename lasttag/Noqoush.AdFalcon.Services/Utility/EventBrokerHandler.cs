using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.DomainServices.EventBroker;

namespace Noqoush.AdFalcon.Services.Utility
{
    public class EventBrokerHandler : IEventBrokerHandler
    {
        #region Implementation of IEventBrokerHandler

        public void RasiEvent(EventData data)
        {
            /*
            //var entity = data.entity as Noqoush.AdFalcon.Domain.Model.Account.AccountFundTransHistoryPgw;
            //if (entity != null)
            if (data.entity.GetType() == (typeof(Noqoush.AdFalcon.Domain.Model.Account.AccountFundTransHistoryPgw)))
            {
                var entity = data.entity as Noqoush.AdFalcon.Domain.Model.Account.AccountFundTransHistoryPgw;
                
                    if (data.ActionType == ObjectActionEnum.Update)
                    {
                        if (entity.FundTransStatus == AccountFundTransStatus.Committed)
                        {
                            Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase(entity, "Close Fund Transaction"));
                        }
                    }
            }
            else if (data.entity.GetType() == (typeof(Noqoush.AdFalcon.Domain.Model.Campaign.Campaign)))
            {
                //if campaign status is paused 
                var entity = data.entity as Noqoush.AdFalcon.Domain.Model.Campaign.Campaign;
                /*if (data.OldState[] == AccountFundTransStatus.Committed)
                {
                    Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase(entity, "Close Fund Transaction"));
                }*/
            //}
        }

        #endregion
    }
}
