using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{

  [DataContract]
    public class VideoConversionCreativeUnitDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int BitRate { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int CreativeUnitId { get; set; }
        [DataMember]

        public int AudioBitRate { get; set; }
        [DataMember]

        public  int VideoFrameRate { get; set; }


    }
}
