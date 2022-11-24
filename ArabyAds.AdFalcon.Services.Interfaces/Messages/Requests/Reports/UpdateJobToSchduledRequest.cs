using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UpdateJobToSchduledRequest
    {
        [ProtoMember(1)]
        public int JobID { set; get; }
        [ProtoMember(2)]
        public string JobName { set; get; }
        [ProtoMember(3)]
        public string TriggerName { set; get; }
        [ProtoMember(4)]
        public string TriggerGroupName { set; get; }
        [ProtoMember(5)]
        public DateTime? NextFireTime { set; get; }
    }
}
