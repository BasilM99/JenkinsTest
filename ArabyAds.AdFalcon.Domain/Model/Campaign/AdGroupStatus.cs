using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdGroupStatus : LookupBase<AdGroupStatus, int>
    {
        private const int EmptyStatusId = 1;
        private const int AttentionActionNeededStatusId = 2;
        private const int RunningStatusId = 3;
        private const int RunningWithAttentionActionNeededStatusId = 4;
        private const int CompletedStatusId = 5;
        private static IAdGroupStatusRepository _adGroupStatusRepository = null;

        private static IAdGroupStatusRepository AdGroupStatusRepository
        {
            get {
                if(_adGroupStatusRepository == null)
                {
                       _adGroupStatusRepository = Framework.IoC.Instance.Resolve<IAdGroupStatusRepository>();
                    //by mosab load all statuses in advance 
                    _adGroupStatusRepository.GetAll();

                }
                return _adGroupStatusRepository;

            }
        }
        static AdGroupStatus _empty = null;
        static AdGroupStatus _attentionActionNeeded = null;
        static AdGroupStatus _running = null;
        static AdGroupStatus _runningWithAttentionActionNeeded = null;
        static AdGroupStatus _completed = null;
        static readonly object LockObj = new object();

        private static bool checkStatus(AdGroupStatus status)
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
        public static AdGroupStatus Empty
        {
            get
            {
                if (checkStatus(_empty))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_empty))
                        {
                            _empty = AdGroupStatusRepository.Get(EmptyStatusId);
                        }
                    }
                }
                return _empty;
            }
        }
        public static AdGroupStatus AttentionActionNeeded
        {
            get
            {
                if (checkStatus(_attentionActionNeeded))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_attentionActionNeeded))
                        {
                            _attentionActionNeeded = AdGroupStatusRepository.Get(AttentionActionNeededStatusId);
                        }
                    }
                }

                return _attentionActionNeeded;
            }
        }
        public static AdGroupStatus Running
        {
            get
            {
                if (checkStatus(_running))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_running))
                        {
                            _running = AdGroupStatusRepository.Get(RunningStatusId);
                        }
                    }
                }

                return _running;
            }
        }
        public static AdGroupStatus RunningWithAttentionActionNeeded
        {
            get
            {
                if (checkStatus(_runningWithAttentionActionNeeded))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_runningWithAttentionActionNeeded))
                        {
                            _runningWithAttentionActionNeeded = AdGroupStatusRepository.Get(RunningWithAttentionActionNeededStatusId);
                        }
                    }
                }

                return _runningWithAttentionActionNeeded;
            }
        }
        public static AdGroupStatus Completed
        {
            get
            {
                if (checkStatus(_completed))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_completed))
                        {
                            _completed = AdGroupStatusRepository.Get(CompletedStatusId);
                        }
                    }
                }

                return _completed;
            }
        }
    }
}
