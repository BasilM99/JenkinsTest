using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages.Response
{
    [ProtoContract]
    public class KPIDTO
    {
        [ProtoMember(1)]
        public string  DBField { get; set; }
        [ProtoMember(2)]
        public decimal? MetricValue { get; set; }
        [ProtoMember(3)]
        public double? MetricGrowthValue { get; set; }

        public string Title { get; set; }
        public string MainValue { get; set; }
        public string Icon { get; set; }
        public string PercentValue { get; set; }
        public string PercentValueState { get; set; }
        public string PercentValueDisc { get; set; }
        public int Id { get; set; }
    }
}
