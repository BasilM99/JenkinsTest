using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class TextFilterDto
    {
        [DataMember]
        public int  TextFilterId { get; set; }

        [DataMember]
        [Required]
        public string Text { get; set; }

        [DataMember]
        public string MatchTypeText { get; set; }

        [DataMember]
        [Required]
        public int MatchTypeId { get; set; }
    }
}
