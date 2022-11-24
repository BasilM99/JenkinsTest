using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class KPICardDto
    {

        [ProtoMember(1)]
        public int Metric_1 { get; set; }

       // [ProtoMember(2)]
       // public int Metric_2 { get; set; }

        [ProtoMember(2)]
        public decimal GR_OverTime_Metric_1 { get; set; }




        [ProtoMember(3)]
        public int Metric_2 { get; set; }

      

        [ProtoMember(4)]
        public decimal GR_OverTime_Metric_2 { get; set; }





        [ProtoMember(5)]
        public int Metric_3 { get; set; }

       

        [ProtoMember(6)]
        public decimal GR_OverTime_Metric_3 { get; set; }





        [ProtoMember(7)]
        public int Metric_4 { get; set; }

        

        [ProtoMember(8)]
        public decimal GR_OverTime_Metric_4 { get; set; }


    }
}
