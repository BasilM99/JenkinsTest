using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device
{
    public class DeviceTargetingBase : IEntity<int>, ITargetingBase
    {
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual AdGroup AdGroup { get; set; }
        public virtual bool IsAll { get; set; }
        public virtual DeviceTargeting DeviceTargeting { get; set; }
        public virtual string GetDescription()
        {
            return  string.Empty;
        }

        public virtual ITargetingBase Copy()
        {
            var cloneObj = new DeviceTargetingBase()
                               {
                                   IsDeleted = this.IsDeleted,
                                   AdGroup = this.AdGroup,
                                   IsAll = this.IsAll,
                                   DeviceTargeting = this.DeviceTargeting
                               };
            return cloneObj;
        }
    }
}
