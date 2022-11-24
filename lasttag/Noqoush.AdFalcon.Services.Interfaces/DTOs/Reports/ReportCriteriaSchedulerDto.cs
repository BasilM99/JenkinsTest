using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{

    [DataContract]
    public class ReportCriteriaSchedulerDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int? UserId { get; set; }
        [DataMember]
        public int? AccountId { get; set; }
        [DataMember]
        public string Criteria { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ReportSectionType SectionType { get; set; }

        [DataMember]
        public ReportCriteriaScope ReportScope { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }

    }
}
