using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{

  [ProtoContract]
    public class VideoConversionCreativeUnitDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int BitRate { get; set; }
       [ProtoMember(3)]
        public string Code { get; set; }
       [ProtoMember(4)]
        public int CreativeUnitId { get; set; }
       [ProtoMember(5)]

        public int AudioBitRate { get; set; }
       [ProtoMember(6)]

        public  int VideoFrameRate { get; set; }


    }
}
