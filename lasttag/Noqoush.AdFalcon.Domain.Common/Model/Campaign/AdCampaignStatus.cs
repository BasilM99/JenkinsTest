using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class AdCampaignStatus : LookupBase<AdCampaignStatus, int>
    {
        private const int EmptyStatusId = 1;
        private const int AttentionActionNeededStatusId = 2;
        private const int RunningStatusId = 3;
        private const int RunningWithAttentionActionNeededStatusId = 4;
        private const int CompletedStatusId = 5;
        private static ICampaignStatusRepository _campaignStatusRepository = null;
        private static ICampaignStatusRepository CampaignStatusRepository
        {
            get
            {
                if (_campaignStatusRepository == null)
                {
                    _campaignStatusRepository = Framework.IoC.Instance.Resolve<ICampaignStatusRepository>();
                }
                return _campaignStatusRepository;
            }
        }

        static AdCampaignStatus _empty = null;
        static AdCampaignStatus _attentionActionNeeded = null;
        static AdCampaignStatus _running = null;
        static AdCampaignStatus _runningWithAttentionActionNeeded = null;
        static AdCampaignStatus _completed = null;
        static readonly object LockObj = new object();

        private static bool checkStatus(AdCampaignStatus status)
        {
            if (status != null)
            {
                try
                {
                    status.Name.ToString();
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static AdCampaignStatus Empty
        {
            get
            {
                if (checkStatus(_empty))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_empty))
                        {
                            _empty = CampaignStatusRepository.Get(EmptyStatusId);
                            _empty.Name.ToString();
                        }
                    }
                }
                return _empty;
            }
        }
        public static AdCampaignStatus AttentionActionNeeded
        {
            get
            {
                if (checkStatus(_attentionActionNeeded))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_attentionActionNeeded))
                        {
                            _attentionActionNeeded = CampaignStatusRepository.Get(AttentionActionNeededStatusId);
                            _attentionActionNeeded.Name.ToString();
                        }
                    }
                }

                return _attentionActionNeeded;
            }
        }
        public static AdCampaignStatus Running
        {
            get
            {
                if (checkStatus(_running))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_running))
                        {
                            _running = CampaignStatusRepository.Get(RunningStatusId);
                            _running.Name.ToString();
                        }
                       
                    }
                }

                return _running;
            }
        }
        public static AdCampaignStatus RunningWithAttentionActionNeeded
        {
            get
            {
                if (checkStatus(_runningWithAttentionActionNeeded))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_runningWithAttentionActionNeeded))
                        {
                            _runningWithAttentionActionNeeded = CampaignStatusRepository.Get(RunningWithAttentionActionNeededStatusId);
                            _runningWithAttentionActionNeeded.Name.ToString();
                        }
                       
                    }
                }

                return _runningWithAttentionActionNeeded;
            }
        }
        public static AdCampaignStatus Completed
        {
            get
            {
                if (checkStatus(_completed))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_completed))
                        {
                            _completed = CampaignStatusRepository.Get(CompletedStatusId);
                            _completed.Name.ToString();
                        }
                    }
                }

                return _completed;
            }
        }
    }
}
