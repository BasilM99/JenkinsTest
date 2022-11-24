using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class TrafficPlannerCriteriaDto
    {
       [ProtoMember(1)]
        public int[] Operators { get; set; }
       [ProtoMember(2)]
        public int[] Countries { get; set; }




       [ProtoMember(3)]
        public int[] Continents { get; set; }
       [ProtoMember(4)]
        public int[] AdSizes { get; set; }
       [ProtoMember(5)]
        public int[] Platforms { get; set; }
       [ProtoMember(6)]
        public int[] AppSites { get; set; }

       [ProtoMember(7)]
        public int[] SubAppSites { get; set; }
       [ProtoMember(8)]
        public int AgeGroup { get; set; }
       [ProtoMember(9)]
        public int DeviceTypeId { get; set; }
       [ProtoMember(10)]
        public int GenderType { get; set; }
       [ProtoMember(11)]
        public int EnvironmentType { get; set; }
       [ProtoMember(12)]
        public int[] languages { get; set; }

       [ProtoMember(13)]
        public int[] AdFormats { get; set; }

       [ProtoMember(14)]
        public int Weekid { get; set; }
       [ProtoMember(15)]
        public int PageIndex { get; set; }

       [ProtoMember(16)]
        public int Size { get; set; }

        public int Type { get; set; }
        public bool isChartType { get; set; }
       [ProtoMember(17)]
        public string Culture
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture.Name; }
            set {; }
        }


        [ProtoMember(18)]
        public int[] Segments { get; set; }

        
    }
}
