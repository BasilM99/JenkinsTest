using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class VideoInfo
    {
        [ProtoMember(1)]
        public int Duration { set; get; }

        [ProtoMember(2)]
        public int BitRate { set; get; }

        [ProtoMember(3)]
        public int Width { set; get; }
        [ProtoMember(4)]
        public int Height { set; get; }

        [ProtoMember(5)]
        public string str { set; get; }
        [ProtoMember(6)]
        public string path { set; get; }

        [ProtoMember(7)]
        public string info { set; get; }

        [ProtoMember(8)]
        public int result { set;get; }



    }
}
