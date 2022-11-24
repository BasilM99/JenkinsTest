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
    public class UrlFilterDto
    {
        [DataMember]
        public int  UrlFilterId { get; set; }

        [DataMember]
        [Required]
        public string Url { get; set; }
     
    }
}
