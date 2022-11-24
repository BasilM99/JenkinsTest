using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class SettingsDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual int TestingModeId
        {
            get;
            set;
        }
        [DataMember]
        [Range(0, int.MaxValue)]
        public virtual int RefreshInterval
        {
            get;
            set;
        }
        [DataMember]
        public virtual int RefreshModeId
        {
            get;
            set;
        }
        [DataMember]
        public virtual int AppSiteId
        {
            get;
            set;
        }
        [DataMember]
        public virtual string AppSiteName
        {
            get;
            set;
        }
    }
}
