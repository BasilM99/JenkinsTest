using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdCreativeStatus : LookupBase<AdCreativeStatus, int>
    {
        private const int SubmittedStatusId = 2;
        private const int DisApprovedStatusId = 3;
        private const int ActiveStatusId = 4;
        private const int ActiveAdServerStatusId = 10;
        private const int InactiveStatusId = 5;
        private const int PausedStatusId = 6;
        private const int BudgetPausedStatusId = 7;
        private const int ExpiredStatusId = 8;
        private const int CompletedStatusId = 9;

        private static IAdCreativeStatusRepository _adCreativeStatusRepository = null;
        private static IAdCreativeStatusRepository AdCreativeStatusRepository
        {
            get
            {
                if (_adCreativeStatusRepository == null)
                {
                    _adCreativeStatusRepository = Framework.IoC.Instance.Resolve<IAdCreativeStatusRepository>();
                }
                return _adCreativeStatusRepository;
            }
        }

        static AdCreativeStatus _submitted = null;
        static AdCreativeStatus _disApproved = null;
        static AdCreativeStatus _active = null;
        static AdCreativeStatus _activeAdServer = null;
        static AdCreativeStatus _inactive = null;
        static AdCreativeStatus _paused = null;
        static AdCreativeStatus _budgetPaused = null;
        static AdCreativeStatus _expired = null;
        static AdCreativeStatus _completed = null;
       

        static readonly object LockObj = new object();

        private static bool checkStatus(AdCreativeStatus status)
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

        public static AdCreativeStatus Submitted
        {
            get
            {
                if (checkStatus(_submitted))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_submitted))
                        {
                            _submitted = AdCreativeStatusRepository.Get(SubmittedStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _submitted.Name.ToString();
                        }
                    }
                }
                return _submitted;
            }
        }
        public static AdCreativeStatus DisApproved
        {
            get
            {
                if (checkStatus(_disApproved))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_disApproved))
                        {
                            _disApproved = AdCreativeStatusRepository.Get(DisApprovedStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _disApproved.Name.ToString();
                        }
                    }
                }
                return _disApproved;
            }
        }
        public static AdCreativeStatus Active
        {
            get
            {
                if (checkStatus(_active))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_active))
                        {
                            _active = AdCreativeStatusRepository.Get(ActiveStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _active.Name.ToString();
                        }
                    }
                }

                return _active;
            }
        }


        public static AdCreativeStatus ActiveAdServer
        {
            get
            {
                if (checkStatus(_activeAdServer))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_activeAdServer))
                        {
                            _activeAdServer = AdCreativeStatusRepository.Get(ActiveAdServerStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _activeAdServer.Name.ToString();
                        }
                    }
                }

                return _activeAdServer;
            }
        }
        public static AdCreativeStatus Inactive
        {
            get
            {
                if (checkStatus(_inactive))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_inactive))
                        {
                            _inactive = AdCreativeStatusRepository.Get(InactiveStatusId);

                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _inactive.Name.ToString();
                        }
                    }
                }

                return _inactive;
            }
        }
        public static AdCreativeStatus Paused
        {
            get
            {
                if (checkStatus(_paused))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_paused))
                        {
                            _paused = AdCreativeStatusRepository.Get(PausedStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _paused.Name.ToString();
                        }
                    }
                }

                return _paused;
            }
        }
        public static AdCreativeStatus BudgetPaused
        {
            get
            {
                if (checkStatus(_budgetPaused))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_budgetPaused))
                        {
                            _budgetPaused = AdCreativeStatusRepository.Get(BudgetPausedStatusId);
                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _budgetPaused.Name.ToString();
                        }
                    }
                }

                return _budgetPaused;
            }
        }
        public static AdCreativeStatus Expired
        {
            get
            {
                if (checkStatus(_expired))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_expired))
                        {
                            _expired = AdCreativeStatusRepository.Get(ExpiredStatusId);

                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _expired.Name.ToString();
                        }
                    }
                }

                return _expired;
            }
        }
        public static AdCreativeStatus Completed
        {
            get
            {
                if (checkStatus(_completed))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_completed))
                        {
                            _completed = AdCreativeStatusRepository.Get(CompletedStatusId);

                            //TODO:osaleh replace this code, this code added to make sure that this object is loaded not proxy
                            _completed.Name.ToString();
                        }
                    }
                }

                return _completed;
            }
        }
        
    }
}
