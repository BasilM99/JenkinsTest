using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{

    [ProtoContract]

    public class PMPTargetingGetDto
    {
       [ProtoMember(1)]
        public IList<PMPTargetingBaseDto> Geographies { get; set; }

       [ProtoMember(2)]
        public IList<PMPTargetingBaseDto> AdFormats { get; set; }
       [ProtoMember(3)]
        public IList<PMPTargetingBaseDto> AdSizes { get; set; }
    }

    [ProtoContract]
   
    public class PMPTargetingBaseDto
    {
       [ProtoMember(1)]
        public int ID { get;  set; }

       [ProtoMember(2)]
        public int RawID { get; set; }
       [ProtoMember(3)]
        public int PMPDealID { get; set; }
       [ProtoMember(4)]
        public PMPDealTargetingType Type { get; set; }
    }

    [ProtoContract]
    public class PMPTargetingSaveDto
    {
       [ProtoMember(1)]
        public int PMPDealID { get; set; }

        [ProtoMember(2)]
        public IList<int> Geographies { get; set; } = new List<int>();

       [ProtoMember(3)]
        public IList<int> AdFormats { get; set; } = new List<int>();

        [ProtoMember(4)]
        public string RawAdFormats { get; set; }
       [ProtoMember(5)]
        public IList<int> AdSizes { get; set; } = new List<int>();
    }
    }
