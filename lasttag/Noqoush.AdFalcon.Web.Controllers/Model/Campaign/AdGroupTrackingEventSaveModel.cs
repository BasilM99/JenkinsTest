using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Campaign
{
    class AdGroupTrackingEventSaveModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }


        [DataMember]
        public List<string> PreRequisitesList
        {
            get;
            set;
        }


        [DataMember]
        public bool AllPreRequisitesRequired { get; set; }

        [DataMember]
        public bool IsCustom { get; set; }


        [DataMember]
        public bool IsBillable { get; set; }

        [DataMember]
        public bool AllowDuplicate { get; set; }

        //[DataMember]
        //public int ValidFor { get; set; }


        [DataMember]
        public string SegmentString { get; set; }


        [DataMember]
        public string SegmentsId { get; set; }

        [DataMember]
        public string SegmentsMapId { get; set; }


    }
}
