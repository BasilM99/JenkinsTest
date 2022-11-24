using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{

    [DataContract]

    public class PMPTargetingGetDto
    {
        [DataMember]
        public IList<PMPTargetingBaseDto> Geographies { get; set; }

        [DataMember]
        public IList<PMPTargetingBaseDto> AdFormats { get; set; }
        [DataMember]
        public IList<PMPTargetingBaseDto> AdSizes { get; set; }
    }

    [DataContract]
   
    public class PMPTargetingBaseDto
    {
        [DataMember]
        public int ID { get;  set; }

        [DataMember]
        public int RawID { get; set; }
        [DataMember]
        public int PMPDealID { get; set; }
        [DataMember]
        public PMPDealTargetingType Type { get; set; }
    }

    [DataContract]
    public class PMPTargetingSaveDto
    {
        [DataMember]
        public int PMPDealID { get; set; }

        [DataMember]
        public IList<int> Geographies { get; set; }

        [DataMember]
        public IList<int> AdFormats { get; set; }

        [DataMember]
        public string RawAdFormats { get; set; }
        [DataMember]
        public IList<int> AdSizes { get; set; }
    }
    }
