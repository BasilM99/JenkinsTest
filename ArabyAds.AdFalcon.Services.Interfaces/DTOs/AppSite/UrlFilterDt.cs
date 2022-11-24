using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class UrlFilterDto
    {
       [ProtoMember(1)]
        public int  UrlFilterId { get; set; }

       [ProtoMember(2)]
        [Required]
        public string Url { get; set; }
     
    }
}
