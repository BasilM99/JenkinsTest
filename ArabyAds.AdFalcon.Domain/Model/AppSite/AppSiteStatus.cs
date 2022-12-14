//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    //[DataContract()]
    //public enum AppSiteStatusEnum
    //{
    //    [EnumMember]
    //    Active = 1,
    //    [EnumMember]
    //    Inactive = 2,
    //    [EnumMember]
    //    Incomplete = 3,
    //    [EnumMember]
    //    Submitted = 4,
    //    [EnumMember]
    //    Rejected = 6

    //}
    public class AppSiteStatus : LookupBase<AppSiteStatus, int>
    {

        private static IAppSiteStatusRepository _appSiteStatusRepository = null;
        private static IAppSiteStatusRepository AppSiteStatusRepository
        {
            get
            {
                if (_appSiteStatusRepository == null)
                {
                    _appSiteStatusRepository = Framework.IoC.Instance.Resolve<IAppSiteStatusRepository>();
                }
                return _appSiteStatusRepository;
            }
        }


        static AppSiteStatus _active = null;
        static AppSiteStatus _inactive = null;
        static AppSiteStatus _incomplete = null;
        static AppSiteStatus _submitted = null;
        static AppSiteStatus _rejected = null;

        static readonly object LockObj = new object();


        public static AppSiteStatus Active
        {
            get
            {
                if (_active == null)
                {
                    lock (LockObj)
                    {
                        if (_active == null)
                        {
                            _active = AppSiteStatusRepository.Get((int)AppSiteStatusEnum.Active);
                        }
                    }
                }
                return _active;
            }
        }
        public static AppSiteStatus Inactive
        {
            get
            {
                if (_inactive == null)
                {
                    lock (LockObj)
                    {
                        if (_inactive == null)
                        {
                            _inactive = AppSiteStatusRepository.Get((int)AppSiteStatusEnum.Inactive);
                        }
                    }
                }
                return _inactive;
            }
        }
        public static AppSiteStatus Incomplete
        {
            get
            {
                if (_incomplete == null)
                {
                    lock (LockObj)
                    {
                        if (_incomplete == null)
                        {
                            _incomplete = AppSiteStatusRepository.Get((int)AppSiteStatusEnum.Incomplete);
                        }
                    }
                }
                return _incomplete;
            }
        }
        public static AppSiteStatus Submitted
        {
            get
            {
                if (_submitted == null)
                {
                    lock (LockObj)
                    {
                        if (_submitted == null)
                        {
                            _submitted = AppSiteStatusRepository.Get((int)AppSiteStatusEnum.Submitted);
                        }
                    }
                }
                return _submitted;
            }
        }
        public static AppSiteStatus Rejected
        {
            get
            {
                if (_rejected == null)
                {
                    lock (LockObj)
                    {
                        if (_rejected == null)
                        {
                            _rejected = AppSiteStatusRepository.Get((int)AppSiteStatusEnum.Rejected);
                        }
                    }
                }
                return _rejected;
            }
        }

    }
}
