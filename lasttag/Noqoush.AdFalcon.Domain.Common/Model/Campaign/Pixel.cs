using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign
{

    [DataContract(Name = "PixelStatus")]
    public enum PixelStatus
    {
        [EnumMember]
        [EnumText("Undefined", "AccountDSPRequest")]
        None = 0,
        [EnumMember]
        [EnumText("InActiveAdvertisers", "Global")]
        InActive = 2,

        [EnumMember]
        [EnumText("Active", "JobGrid")]
        Active = 1
    }
    /*public  class Pixel : IEntity<int>
    {

        public virtual int ID { get; set; }
        //public virtual string Name { get; set; }
        //public virtual int Status { get; set; }

        public virtual PixelStatus Status { set; get; }
        public virtual int Code { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        //public virtual MasterAppSiteType Type { get; set; }
        public virtual Account.Account Account { get; set; }

        public virtual AdvertiserAccount Link { get; set; }
        public virtual DateTime LastModifiedDate { get; set; }
        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public virtual void Delete()
        {
            this.IsDeleted = true;
        }

        public virtual void Activate()
        {
            this.Status = PixelStatus.Active;
        }
        public virtual void DeActivate()
        {
            this.Status = PixelStatus.InActive;
        }

        public virtual IList<AudienceSegmentPixelMap> AudienceSegmentListsMap { get; set; }
    }


    public class AudienceSegmentPixelMap : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual int ID { get; set; }
        public virtual Pixel Pixel { get; set; }
        public virtual AudienceSegment AudienceSegment { get; set; }
        public virtual string GetDescription()
        {
            return Pixel.GetDescription() + "-" + AudienceSegment.Name.GetValue();
        }
    }*/
}
