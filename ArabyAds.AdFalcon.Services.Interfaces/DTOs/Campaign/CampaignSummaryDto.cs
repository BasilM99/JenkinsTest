using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignSummaryDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual DateTime StartDate { get; set; }
        [DataMember]
        public virtual DateTime? EndDate { get; set; }
        [DataMember]
        public virtual decimal Budget { get; set; }
        [DataMember]
        public virtual decimal Spend { get; set; }
        [DataMember]
        public virtual decimal? DailyBudget { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
    }
}
