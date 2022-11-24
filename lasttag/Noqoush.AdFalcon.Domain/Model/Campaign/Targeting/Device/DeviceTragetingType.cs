
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device
{
    //[DataContract()]
    //public enum DeviceTargetingTypeEnum
    //{
    //    [EnumMember]
    //    Platform = 1,
    //    [EnumMember]
    //    Manufacturer = 2,
    //    [EnumMember]
    //    ModelTargeting = 3,
    //    [EnumMember]
    //    ActionTypeTargeting = 4,
    //    [EnumMember]
    //    DeviceCapability = 5
    //}

    public class DeviceTargetingType : LookupBase<DeviceTargetingType, int>
    {

        public const int PlatformTypeId = 1;
        public const int ManufacturerTypeId = 2;
        public const int ModelTargetingTypeId = 3;
        public const int ActionTypeTargetingTypeId = 4;
        public const int DeviceCapabilityTypeId = 5;
        private static IDeviceTargetingTypeRepository _deviceTargetingTypeRepository = null;
        private static IDeviceTargetingTypeRepository deviceTargetingTypeRepository
        {
            get
            {
                if (_deviceTargetingTypeRepository == null)
                {
                    _deviceTargetingTypeRepository = Framework.IoC.Instance.Resolve<IDeviceTargetingTypeRepository>();
                }
                return _deviceTargetingTypeRepository;
            }
        }
        static readonly object lockObj = new object();

        static DeviceTargetingType _Platform = null;
        static DeviceTargetingType _Manufacturer = null;
        static DeviceTargetingType _Model = null;
        static DeviceTargetingType _ActionType = null;

        public static DeviceTargetingType Platform
        {
            get
            {
                if (_Platform == null)
                {
                    lock (lockObj)
                    {
                        if (_Platform == null)
                        {
                            _Platform = deviceTargetingTypeRepository.Get(PlatformTypeId);
                        }
                    }
                }
                return _Platform;
            }
        }
        public static DeviceTargetingType Manufacturer
        {
            get
            {
                if (_Manufacturer == null)
                {
                    lock (lockObj)
                    {
                        if (_Manufacturer == null)
                        {
                            _Manufacturer = deviceTargetingTypeRepository.Get(ManufacturerTypeId);
                        }
                    }
                }
                return _Manufacturer;
            }
        }
        public static DeviceTargetingType Model
        {
            get
            {
                if (_Model == null)
                {
                    lock (lockObj)
                    {
                        if (_Model == null)
                        {
                            _Model = deviceTargetingTypeRepository.Get(ModelTargetingTypeId);
                        }
                    }
                }
                return _Model;
            }
        }
        public static DeviceTargetingType ActionType
        {
            get
            {
                if (_ActionType == null)
                {
                    lock (lockObj)
                    {
                        if (_Model == null)
                        {
                            _ActionType = deviceTargetingTypeRepository.Get(ActionTypeTargetingTypeId);
                        }
                    }
                }
                return _ActionType;
            }
        }
    }
}
