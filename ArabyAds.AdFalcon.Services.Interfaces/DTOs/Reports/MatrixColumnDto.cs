using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.Core
{
    [ProtoContract]
    public class metriceColumnDto
    {
       [ProtoMember(1)]

        public int Id { get; set; }
       [ProtoMember(2)]

        public bool Advertiser { get; set; }
       [ProtoMember(3)]

        public bool Publisher { get; set; }
       [ProtoMember(4)]

        public bool DSP { get; set; }
       [ProtoMember(5)]

        public bool IsSelected { get; set; }

       [ProtoMember(6)]

        public string Header { get; set; }


       [ProtoMember(7)]

        public string HeaderResourceKey { get; set; }

       [ProtoMember(8)]

        public string HeaderResourceSet { get; set; }
       [ProtoMember(9)]

        public string GroupKey { get; set; }
       [ProtoMember(10)]

        public string DataBaseFieldName { get; set; }
       [ProtoMember(11)]

        public bool Hide { get; set; }

       [ProtoMember(12)]

        public string AppFieldName { get; set; }
       [ProtoMember(13)]

        public string Format { get; set; }

       [ProtoMember(14)]

        public int Order { get; set; }
    }
}
