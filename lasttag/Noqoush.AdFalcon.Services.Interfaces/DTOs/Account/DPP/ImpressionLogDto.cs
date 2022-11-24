using Noqoush.AdFalcon.Domain.Common.Model.Account.DPP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP
{
    [DataContract]

    public class ImpressionLogDto
    {
        [DataMember]

        public int ID { set; get; }
        [DataMember]

        public BusinessPartnerDto Provider { set; get; }
        [DataMember]

        public DateTime CreationDate
        {
            get;
            set;
        }
        [DataMember]

        public DateTime LastUpdate
        {
            get;
            set;
        }
        [DataMember]

        public DateTime Day { set; get; }
        [DataMember]

        public bool IsDeleted { get; set; }

        [DataMember]

        public string Path { get; set; }

        [DataMember]

        public ImpressionLogType Type { get; set; }

        [DataMember]

        public string LogTypeString { get
            {
                if (Type==ImpressionLogType.AdMarkup)
                {
                    return "Ad Markup";
                }
                else
                {
                    return "Impression";
                }


            } set { } }
    }

    [DataContract]
    public class ImpressionLogListResultDto
    {
        [DataMember]
        public IEnumerable<ImpressionLogDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}
