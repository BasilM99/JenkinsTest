using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UpdateJobToSchduledAfterRunRequest
    {
        [ProtoMember(1)]
        public int JobID { set; get; }
        [ProtoMember(2)]
        public string ReportFilePath { set; get; }
    }
}
