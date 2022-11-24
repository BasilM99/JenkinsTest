using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class PlatfromTree
    {
        [ProtoMember(1)]
        public int Id { set; get; }
        [ProtoMember(2)]
        public List<ManuTree> Manu { set; get; } = new List<ManuTree>();
        [ProtoMember(3)]
        public bool IsAll { set; get; }

    }

    [ProtoContract]

    public class ManuTree
    {
        [ProtoMember(1)]
        public int Id { set; get; }
        [ProtoMember(2)]
        public List<int> Devices { set; get; } = new List<int>();
        [ProtoMember(3)]
        public bool IsAll { set; get; }
    }

}
