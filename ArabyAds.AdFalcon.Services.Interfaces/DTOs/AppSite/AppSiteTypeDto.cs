using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteTypeDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }

       [ProtoMember(2)]
        public LocalizedStringDto Name { get; set; }
       [ProtoMember(3)]
        public string ViewName { get; set; }
       [ProtoMember(4)]
        public virtual bool IsApp
        {
            get;
            set;
        }
    }
}
