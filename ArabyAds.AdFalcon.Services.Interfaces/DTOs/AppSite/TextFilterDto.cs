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
    public class TextFilterDto
    {
       [ProtoMember(1)]
        public int  TextFilterId { get; set; }

       [ProtoMember(2)]
        [Required]
        public string Text { get; set; }

       [ProtoMember(3)]
        public string MatchTypeText { get; set; }

       [ProtoMember(4)]
        [Required]
        public int MatchTypeId { get; set; }
    }
}
