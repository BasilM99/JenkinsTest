using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    
    [DataContract]
    public class ReportCriteriaDto : BasePagingCriteriaDto

    {


        [DataMember]
        public List<int> ColumnsIds { get; set; }
        [DataMember]
        public List<int> MeasuresIds { get; set; }
        [DataMember]
        public string QueryJsonData { get; set; }
        [DataMember]
        public string ColumnsIdsString { get; set; }

        [DataMember]
        public string MeasuresIdsString { get; set; }

        [DataMember]
        public int fact { get; set; }
        [DataMember]
        public bool IncludeId { get; set; }


        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public bool IsPrimaryUser { get; set; }


        [DataMember]
        public int SummaryBy { get; set; }

  
        [DataMember]
        public bool IsAccumulated { get {

            if (SummaryBy==4)
        {
            return true;
        
        }
            return false;

        } set { } }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string Layout { get; set; }

        [DataMember]
        public string ItemsList { get; set; }


        [DataMember]
        public int CampName { get; set; }
        [DataMember]
        public int CompanyName { get; set; }
      
        [DataMember]
        public string AdvancedCriteria { get; set; }
        [DataMember]
        public string SecondAdvancedCriteria { get; set; }
        [DataMember]
        public string MetricCode { get; set; }
        [DataMember]
        public CampaignType CampaignType { get; set; }
        [DataMember]
        public CampaignType NotInCampaignType { get; set; }

        [DataMember]
        public bool GroupByName { get; set; }
        
        [DataMember]
        public string TabId { get; set; }
        
        [DataMember]
        public string DeviceCategory { get; set; }
        [DataMember]
        public string CriteriaOpt { get; set; }

        [DataMember]

        public string FromDateString
        {
            get;

            set;
        }
        [DataMember]

        public string ToDateString
        {
            get;

            set;
        }
    }
}
