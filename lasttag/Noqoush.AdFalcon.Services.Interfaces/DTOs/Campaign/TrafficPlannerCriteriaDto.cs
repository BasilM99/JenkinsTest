using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class TrafficPlannerCriteriaDto
    {
        [DataMember]
        public int[] Operators { get; set; }
        [DataMember]
        public int[] Countries { get; set; }
        [DataMember]
        public int[] Continents { get; set; }
        [DataMember]
        public int[] AdSizes { get; set; }
        [DataMember]
        public int[] Platforms { get; set; }
        [DataMember]
        public int[] AppSites { get; set; }

        [DataMember]
        public int[] SubAppSites { get; set; }
        [DataMember]
        public int AgeGroup { get; set; }
        [DataMember]
        public int DeviceTypeId { get; set; }
        [DataMember]
        public int GenderType { get; set; }
        [DataMember]
        public int EnvironmentType { get; set; }
        [DataMember]
        public int[] languages { get; set; }

        [DataMember]
        public int[] AdFormats { get; set; }

        [DataMember]
        public int Weekid { get; set; }
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int Size { get; set; }

        public int Type { get; set; }
        public bool isChartType { get; set; }
        [DataMember]
        public string Culture
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture.Name; }
            set {; }
        }
    }
}
