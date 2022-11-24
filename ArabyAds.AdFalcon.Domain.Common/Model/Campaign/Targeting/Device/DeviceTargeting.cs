﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using System.Linq;
using System;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device
{
    public class DeviceTargeting : TargetingBase
    {
      
        //private IList<PlatformTargeting> platforms = new List<PlatformTargeting>();
        //private IList<ModelTargeting> devices = new List<ModelTargeting>();
        //private IList<ManufacturerTargeting> manufacturers = new List<ManufacturerTargeting>();
        //private IList<DeviceCapabilityTargeting> deviceCapabilities = new List<DeviceCapabilityTargeting>();

        public virtual bool IsAll { get; set; }
        public virtual DeviceTargetingType TargetingType { get; set; }
        public virtual IEnumerable<Model.Core.Device> Devices { get { return DevicesTargeting.Select(x => x.Device); } }
        public virtual IEnumerable<Manufacturer> Manufacturers { get { return ManufacturersTargeting.Select(x => x.Manufacturer); } }
        public virtual IEnumerable<Platform> Platforms { get { return PlatformsTargeting.Select(x => x.Platform); } }
        public virtual IEnumerable<DeviceCapability> DeviceCapabilities { get { return DeviceCapabilitiesTargeting.Select(x => x.Capability); } }
        public virtual IEnumerable<DeviceType> DeviceTypes { get { return DeviceTypeTargetings.Select(p => p.DeviceType); } }


        public virtual IEnumerable<Manufacturer> ManufacturersIsAll { get { return ManufacturersTargeting.Where(x => x.IsAll).Select(x => x.Manufacturer); } }
        public virtual IEnumerable<Platform> PlatformsIsAll
        {
            get
            {
                return PlatformsTargeting.Where(x => x.IsAll).Select(x => x.Platform).ToList();
            }
        }


        public virtual IList<ModelTargeting> DevicesTargeting { get; set; }
        public virtual IList<ManufacturerTargeting> ManufacturersTargeting { get; set; }
        public virtual IList<PlatformTargeting> PlatformsTargeting { get; set; }
        public virtual IList<DeviceCapabilityTargeting> DeviceCapabilitiesTargeting { get; set; }
        public virtual IList<DeviceTypeTargeting> DeviceTypeTargetings { get; set; }
        public virtual string GetDeviceTargetingsString(string t)
        {
            
            string result = string.Empty;
            if (!string.IsNullOrEmpty(t))
            {
                var itemsStr = t.Split(',');
                foreach (var item in itemsStr )
                {
                    int id= Convert.ToInt32(item);
                    var oobject = TargetingBaseRepository.Get(id);
                    if(oobject!=null)
                    result = result + oobject.GetDescription();

                }
                return result;


            }
            return string.Empty;
        

        }

   public DeviceTargeting()
        {
            DevicesTargeting = new List<ModelTargeting>();
            ManufacturersTargeting = new List<ManufacturerTargeting>();
            PlatformsTargeting = new List<PlatformTargeting>();
            DeviceCapabilitiesTargeting = new List<DeviceCapabilityTargeting>();
            DeviceTypeTargetings = new List<DeviceTypeTargeting>();

        }
        public virtual void ClearPlatforms()
        {
            PlatformsTargeting.Clear();
        }
        public virtual PlatformTargeting AddPatform(Platform platform, string minimumVersion, bool isAll = true)
        {
            this.IsAll = false;
            var item = new PlatformTargeting { Platform = platform, MinimumVersion = minimumVersion, DeviceTargeting = this, IsAll = isAll, AdGroup = this.AdGroup };
            PlatformsTargeting.Add(item);
            return item;
        }

        public virtual PlatformTargeting ChangePlatformTargeting(int platformId, string minimumVersion, bool isAll = true)
        {
            PlatformTargeting targeting = PlatformsTargeting.Where(p => p.Platform.ID == platformId).SingleOrDefault();
            targeting.MinimumVersion = minimumVersion;
            targeting.IsAll = isAll;


            return targeting;
        }
        //public virtual ManufacturerTargeting ChangeManufacturerTargeting(int manufacturerId, bool isAll = true)
        //{
        //    ManufacturerTargeting targeting = ManufacturersTargeting.Where(p => p.Manufacturer.ID == manufacturerId).SingleOrDefault();
        //    targeting.IsAll = isAll;
        //    return targeting;
        //}

        public virtual void RemovePatform(Platform platform)
        {
            var item = PlatformsTargeting.FirstOrDefault(x => x.Platform.ID == platform.ID);
            if (item != null)
                PlatformsTargeting.Remove(item);
        }
        public virtual void ClearDevices()
        {
            DevicesTargeting.Clear();
        }
        public virtual ModelTargeting AddDevice(Model.Core.Device device)
        {
            this.IsAll = false;

            var item = new ModelTargeting { Device = device, DeviceTargeting = this, IsAll = true, AdGroup = this.AdGroup };
            //add device platform and manufacture
            //check if the platform is found , if not then add it
            var platform = PlatformsTargeting.FirstOrDefault(x => x.Platform.ID == device.Platform.ID);
            if (platform == null)
                AddPatform(device.Platform, null, false);

            //check if the platform is found , if not then add it
            var manufacturer = ManufacturersTargeting.FirstOrDefault(x => x.Manufacturer.ID == device.Manufacturer.ID);
            if (manufacturer == null)
                AddManufacturer(device.Manufacturer, false);


            DevicesTargeting.Add(item);
            return item;
        }
        public virtual void RemoveDevice(Model.Core.Device device)
        {
            var item = DevicesTargeting.FirstOrDefault(x => x.Device.ID == device.ID);
            if (item != null)
                DevicesTargeting.Remove(item);
        }
        public virtual void ClearManufacturers()
        {
            ManufacturersTargeting.Clear();
        }
        public virtual ManufacturerTargeting AddManufacturer(Manufacturer manufacturer, bool isAll = true)
        {
            this.IsAll = false;
            var item = new ManufacturerTargeting { Manufacturer = manufacturer, DeviceTargeting = this, IsAll = isAll, AdGroup = this.AdGroup };
            ManufacturersTargeting.Add(item);
            return item;
        }
        public virtual void RemoveManufacturer(Manufacturer manufacturer)
        {
            var item = ManufacturersTargeting.FirstOrDefault(x => x.Manufacturer.ID == manufacturer.ID);
            if (item != null)
                ManufacturersTargeting.Remove(item);
        }
        public virtual void ClearDeviceCapabilities()
        {
            DeviceCapabilitiesTargeting.Clear();
        }

        public virtual DeviceCapabilityTargeting ChangeDeviceCapabilityIncludeStatus(DeviceCapability deviceCapability, bool isInclude = true)
        {
            var item = DeviceCapabilitiesTargeting.FirstOrDefault(x => x.Capability.ID == deviceCapability.ID);
            if (item != null)
                item.IsInclude = isInclude;
            return item;
        }

        public virtual DeviceCapabilityTargeting AddDeviceCapability(DeviceCapability deviceCapability, bool isInclude = true)
        {
            this.IsAll = false;
            var item = new DeviceCapabilityTargeting { Capability = deviceCapability, DeviceTargeting = this, IsAll = true, AdGroup = this.AdGroup, IsInclude = isInclude };
            DeviceCapabilitiesTargeting.Add(item);
            return item;
        }
        public virtual void RemoveDeviceCapability(DeviceCapability deviceCapability)
        {
            var item = DeviceCapabilitiesTargeting.FirstOrDefault(x => x.Capability.ID == deviceCapability.ID);
            if (item != null)
                DeviceCapabilitiesTargeting.Remove(item);
        }

        public virtual void AddDeviceType(DeviceType deviceType)
        {
            var item = new DeviceTypeTargeting() { AdGroup = this.AdGroup, DeviceTargeting = this };
            item.DeviceType = deviceType;
            DeviceTypeTargetings.Add(item);
        }

        public virtual void RemoveDeviceType(DeviceType deviceType)
        {
            var item = DeviceTypeTargetings.FirstOrDefault(x => x.DeviceType.ID == deviceType.ID);
            if (item != null)
                DeviceTypeTargetings.Remove(item);
        }

        public virtual void ClearDeviceTypeTargeting()
        {
            DeviceTypeTargetings.Clear();
        }

        public virtual void ChangeDeviceType(DeviceTypeTargeting deviceType)
        {
            var item = DeviceTypeTargetings.Where(p => p.ID == deviceType.ID).SingleOrDefault();

            if (item != null)
            {
                item = deviceType;
            }
        }

        public virtual DeviceTargetingBase GetTargeting(int id, DeviceTargetingTypeEnum type)
        {
            switch (type)
            {
                case DeviceTargetingTypeEnum.Platform:
                    {
                        return this.PlatformsTargeting.FirstOrDefault(item => item.ID == id);
                    }
                case DeviceTargetingTypeEnum.Manufacturer:
                    {
                        return this.ManufacturersTargeting.FirstOrDefault(item => item.ID == id);
                    }
                case DeviceTargetingTypeEnum.DeviceCapability:
                    {
                        return this.DeviceCapabilitiesTargeting.FirstOrDefault(item => item.ID == id);
                    }
                case DeviceTargetingTypeEnum.ModelTargeting:
                    {
                        return this.DevicesTargeting.FirstOrDefault(item => item.ID == id);
                    }
            }
            return null;
        }
        public override string GetDescription()
        {
            //return string.Empty;
            var builder = new StringBuilder();
            if (this.TargetingType == null)
                return string.Empty;
            //TODO:OSaleh to remove this and use more generic way
            builder.Append("<p>");
            builder.AppendFormat("<b>{0}</b><br/>", this.TargetingType.GetDescription());

            switch (this.TargetingType.ID)
            {
                case DeviceTargetingType.PlatformTypeId:
                    {
                        var result = this.Platforms.Aggregate(string.Empty, (current, platform) => current + (platform.GetDescription() + ","));
                        builder.AppendFormat(result.Trim(','));
                        break;
                    }
                case DeviceTargetingType.ManufacturerTypeId:
                    {
                        var result = this.Manufacturers.Aggregate(string.Empty, (current, manufacturer) => current + (manufacturer.GetDescription() + ","));
                        builder.AppendFormat(result.Trim(','));
                        break;
                    }
                case DeviceTargetingType.ModelTargetingTypeId:
                    {
                        var result = this.Devices.Aggregate(string.Empty, (current, device) => current + (device.GetDescription() + ","));
                        builder.AppendFormat(result.Trim(','));
                        break;
                    }
                case DeviceTargetingType.DeviceCapabilityTypeId:
                    {
                        var result = this.DeviceCapabilities.Aggregate(string.Empty, (current, deviceCapability) => current + (deviceCapability.GetDescription() + ","));
                        builder.AppendFormat(result.Trim(','));
                        break;
                    }
                case DeviceTargetingType.ActionTypeTargetingTypeId:
                    {
                        var flag = false;
                        var result = string.Empty;
                        foreach (var platformTargeting in this.PlatformsTargeting)
                        {
                            //if (platformTargeting.IsAll)
                            {
                                if (!flag)
                                {
                                    builder.AppendFormat("<br/><b>"+ Framework.Resources.ResourceManager.Instance.GetResource("Manufacturers", "Global") + "</b><br/>");
                                    flag = true;
                                }
                                result += platformTargeting.Platform.GetDescription() + ",";
                            }
                        }
                        builder.AppendFormat(result.Trim(','));
                        flag = false;
                        result = string.Empty;
                        foreach (var manufacturerTargeting in this.ManufacturersTargeting)
                        {
                            //if (manufacturerTargeting.IsAll)
                            {
                                if (!flag)
                                {
                                    builder.AppendFormat("<br/><b>"+ Framework.Resources.ResourceManager.Instance.GetResource("Manufacturers", "Global") +"</b><br/>");
                                    flag = true;
                                }
                                result += manufacturerTargeting.Manufacturer.GetDescription() + ",";
                            }
                        }
                        builder.AppendFormat(result.Trim(','));
                        flag = false;
                        result = string.Empty;
                        foreach (var deviceTargeting in this.DevicesTargeting)
                        {
                            //if (deviceTargeting.IsAll)
                            {
                                if (!flag)
                                {
                                    builder.AppendFormat("<br/><b>"+ Framework.Resources.ResourceManager.Instance.GetResource("Devices", "Global") + "</b><br/>");
                                    flag = true;
                                }
                                result += deviceTargeting.Device.GetDescription() + ",";
                            }
                        }
                        builder.AppendFormat(result.Trim(','));
                        break;
                    }

            }
            builder.Append("</p>");
            return builder.ToString();
        }

        public override TargetingBase Copy()
        {
            var cloneObj = new DeviceTargeting()
            {
                TargetingType = this.TargetingType,
                IsAll = this.IsAll,
                Type = this.Type,
                IsDeleted = this.IsDeleted,
                DevicesTargeting = new List<ModelTargeting>(),
                PlatformsTargeting = new List<PlatformTargeting>(),
                ManufacturersTargeting = new List<ManufacturerTargeting>(),
                DeviceCapabilitiesTargeting = new List<DeviceCapabilityTargeting>()
            };

            foreach (var modelTargeting in DevicesTargeting)
            {
                var modelTargetingClone = modelTargeting.Copy() as ModelTargeting;
                modelTargetingClone.DeviceTargeting = cloneObj;
                cloneObj.DevicesTargeting.Add(modelTargetingClone);
            }

            foreach (var manufacturerTargeting in ManufacturersTargeting)
            {
                var manufacturerTargetingClone = manufacturerTargeting.Copy() as ManufacturerTargeting;
                manufacturerTargetingClone.DeviceTargeting = cloneObj;

                cloneObj.ManufacturersTargeting.Add(manufacturerTargetingClone);
            }

            foreach (var platformTargeting in PlatformsTargeting)
            {
                var platformTargetingClone = platformTargeting.Copy() as PlatformTargeting;
                platformTargetingClone.DeviceTargeting = cloneObj;

                cloneObj.PlatformsTargeting.Add(platformTargetingClone);
            }

            foreach (var deviceCapabilityTargeting in DeviceCapabilitiesTargeting)
            {
                var deviceCapabilityTargetingClone = deviceCapabilityTargeting.Copy() as DeviceCapabilityTargeting;
                deviceCapabilityTargetingClone.DeviceTargeting = cloneObj;

                cloneObj.DeviceCapabilitiesTargeting.Add(deviceCapabilityTargetingClone);
            }

            foreach (var deviceTypeTargeting in DeviceTypeTargetings)
            {
                var deviceTypeTargetingClone = deviceTypeTargeting.Copy() as DeviceTypeTargeting;
                deviceTypeTargetingClone.DeviceTargeting = cloneObj;
                cloneObj.DeviceTypeTargetings.Add(deviceTypeTargetingClone);
            }
            return cloneObj;
        }

        public virtual void ResetIsAll(IList<int> ids, DeviceTargetingTypeEnum type)
        {
            switch (type)
            {
                case DeviceTargetingTypeEnum.Platform:
                    {
                        foreach (var item in PlatformsTargeting.Where(item => ids.Contains(item.Platform.ID)))
                        {
                            item.IsAll = false;
                        }
                        break;
                    }
                case DeviceTargetingTypeEnum.Manufacturer:
                    {
                        foreach (var item in ManufacturersTargeting.Where(item => ids.Contains(item.Manufacturer.ID)))
                        {
                            item.IsAll = false;
                        }
                        break;
                    }
            }
        }


    }
}

