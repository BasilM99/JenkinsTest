using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    
    [ProtoContract]
    public class ReportCriteriaDto : BasePagingCriteriaDto

    {


       [ProtoMember(1)]
        public List<int> ColumnsIds { get; set; }
       [ProtoMember(2)]
        public List<int> MeasuresIds { get; set; }
       [ProtoMember(3)]
        public string QueryJsonData { get; set; }
       [ProtoMember(4)]
        public string ColumnsIdsString { get; set; }

        public IDictionary<int, IList<object>> DimensionValues { get; set; } = new Dictionary<int, IList<object>>();

        [ProtoMember(5)]
        public string MeasuresIdsString { get; set; }

       [ProtoMember(6)]
        public int fact { get; set; }
       [ProtoMember(7)]
        public bool IncludeId { get; set; }


       [ProtoMember(8)]
        public int userId { get; set; }
       [ProtoMember(9)]
        public bool IsPrimaryUser { get; set; }


       [ProtoMember(10)]
        public int SummaryBy { get; set; }

  
       [ProtoMember(11)]
        public bool IsAccumulated { get {

            if (SummaryBy==4)
        {
            return true;
        
        }
            return false;

        } set { } }

       [ProtoMember(12)]
        public int AccountId { get; set; }

       [ProtoMember(13)]
        public string Layout { get; set; }

       [ProtoMember(14)]
        public string ItemsList { get; set; }


       [ProtoMember(15)]
        public int CampName { get; set; }
       [ProtoMember(16)]
        public int CompanyName { get; set; }
      
       [ProtoMember(17)]
        public string AdvancedCriteria { get; set; }
       [ProtoMember(18)]
        public string SecondAdvancedCriteria { get; set; }
       [ProtoMember(19)]
        public string MetricCode { get; set; }
       [ProtoMember(20)]
        public CampaignType CampaignType { get; set; }
       [ProtoMember(21)]
        public CampaignType NotInCampaignType { get; set; }

       [ProtoMember(22)]
        public bool GroupByName { get; set; }
        
       [ProtoMember(23)]
        public string TabId { get; set; }
        
       [ProtoMember(24)]
        public string DeviceCategory { get; set; }
       [ProtoMember(25)]
        public string CriteriaOpt { get; set; }

       [ProtoMember(26)]

        public string FromDateString
        {
            get;

            set;
        }
       [ProtoMember(27)]

        public string ToDateString
        {
            get;

            set;
        }

        [ProtoMember(28)]
        public int ID { get; set; }


        [ProtoMember(29)]
        public string Name { get; set; }

        [ProtoMember(30)]
        public ReportSectionType SectionType { get; set; }


        [ProtoMember(31)]
        public string TypeSave { get; set; }

        [ProtoMember(32)]
        public string chartType { get; set; }



      
       
    }
}
