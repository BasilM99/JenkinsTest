using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteTypeDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public LocalizedStringDto Name { get; set; }
        [DataMember]
        public string ViewName { get; set; }
        [DataMember]
        public virtual bool IsApp
        {
            get;
            set;
        }
    }
}
