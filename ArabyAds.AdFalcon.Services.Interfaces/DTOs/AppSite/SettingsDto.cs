using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class SettingsDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public virtual int TestingModeId
        {
            get;
            set;
        }
       [ProtoMember(3)]
        [Range(0, int.MaxValue)]
        public virtual int RefreshInterval
        {
            get;
            set;
        }
       [ProtoMember(4)]
        public virtual int RefreshModeId
        {
            get;
            set;
        }
       [ProtoMember(5)]
        public virtual int AppSiteId
        {
            get;
            set;
        }
       [ProtoMember(6)]
        public virtual string AppSiteName
        {
            get;
            set;
        }
    }
}
