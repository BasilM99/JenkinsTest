using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class ObjectTypeDto
    {
        [ProtoMember(1)]
        public int ID { get; set; }
        [ProtoMember(2)]
        public int RootID { get; set; }
        [ProtoMember(3)]
        public string ObjectTypeName { get; set; }
    }
    [ProtoContract]

    public class TrialDto
    {
       [ProtoMember(1)]
        public string ObjectName { get; set; }
       [ProtoMember(2)]
        public long ID { get; set; }
       [ProtoMember(3)]
        public DateTime ActionTime { get; set; }
       [ProtoMember(4)]
        public string ActionTimeString { get; set; }
       [ProtoMember(5)]
        public string Type { get; set; }
       [ProtoMember(6)]
        public string Name { get; set; }

       [ProtoMember(7)]
        public string UserName { get; set; }
       [ProtoMember(8)]
        public string SessionId { get; set; }

       [ProtoMember(9)]
        public int UserId { get; set; }
       [ProtoMember(10)]
        public int ObjectActionID { get; set; }
       [ProtoMember(11)]
        public int RootId { get; set; }

        [ProtoMember(12)]
        public IList<TrialDto> Childs { get; set; } = new List<TrialDto>();
       [ProtoMember(13)]
        public int ObjectRootTypeId { get; set; }
       [ProtoMember(14)]
        public int ObjectId { get; set; }
       [ProtoMember(15)]
        public int ObjectTypeId { get; set; }
       [ProtoMember(16)]
        public string Details { get; set; }


       [ProtoMember(17)]
        public string ObjectActionString { get; set; }

       [ProtoMember(18)]
        public string ObjectActionConstantString { get; set; }
       
       [ProtoMember(19)]
        public string PropertyName { get; set; }
       [ProtoMember(20)]
        public string NewValue { get; set; }
       [ProtoMember(21)]
        public string OldValue { get; set; }

        [ProtoMember(22)]
        public string PropertyNameStr { get { if (!String.IsNullOrEmpty(PropertyName)) return PropertyName; else return string.Empty;  } set { } }
        [ProtoMember(23)]
        public string NewValueStr { get { if (!String.IsNullOrEmpty(NewValue)) return NewValue; else return string.Empty; } set { } }
        [ProtoMember(24)]
        public string OldValueStr { get { if (!String.IsNullOrEmpty(OldValue)) return OldValue; else return string.Empty; } set { } }
    }
}
