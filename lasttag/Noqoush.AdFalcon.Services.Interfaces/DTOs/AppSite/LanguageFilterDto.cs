using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class LanguageFilterDto
    {
        [DataMember]
        public int  languageFilterId { get; set; }

        [DataMember]
        [Required]
        public int LanguageId { get; set; }

        [DataMember]
        public string LanguageName { get; set; }
    }
}
