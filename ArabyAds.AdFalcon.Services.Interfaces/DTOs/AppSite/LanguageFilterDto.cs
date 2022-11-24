using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class LanguageFilterDto
    {
       [ProtoMember(1)]
        public int  languageFilterId { get; set; }

       [ProtoMember(2)]
        [Required]
        public int LanguageId { get; set; }

       [ProtoMember(3)]
        public string LanguageName { get; set; }
    }
}
