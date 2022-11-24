using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdvertiserAccountMasterAppSiteResultDto
    {

        [DataMember]
        public IEnumerable<AdvertiserAccountMasterAppSiteDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class AdvertiserAccountMasterAppSiteDto
    {
        [DataMember]

        public int Id { get; set; }
        [DataMember]

        public int LinkId { get; set; }

        [DataMember]

        public MasterAppSiteStatus Status { get; set; }

        [DataMember]

        public MasterAppSiteType Type { get; set; }

        [DataMember]

        public bool GlobalScope { get; set; }
        
        [DataMember]

        public string ScopeString { get {
                if (GlobalScope)
                    return "fa fa-globe";
                else  if (LinkId>0)
                    return "";
                else
                    return "";
               
            } set {


            } }

        [DataMember]

        public string StatusString { get { return this.Status.ToText(); } set { } }
        [DataMember]

        public string TypeString { get { return this.Type.ToText(); } set { } }
        [DataMember]

        public int UserId { get; set; }
        [DataMember]

        public int AccountId { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ContentListItems { get; set; }
    }



    [DataContract]
    public class PixelResultDto
    {

        [DataMember]
        public IEnumerable<PixelDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class PixelDto
    {
        [DataMember]

        public int Id { get; set; }
        [DataMember]

        public int LinkId { get; set; }

        [DataMember]

        public PixelStatus Status { get; set; }

      

        [DataMember]

        public bool GlobalScope { get; set; }

        [DataMember]

        public string ScopeString
        {
            get
            {
                if (GlobalScope)
                    return "fa fa-globe";
                else if (LinkId > 0)
                    return "";
                else
                    return "";

            }
            set
            {


            }
        }

        [DataMember]

        public string StatusString { get { return this.Status.ToText(); } set { } }
       
        [DataMember]

        public int UserId { get; set; }
        [DataMember]

        public int AccountId { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ContentListItems { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string SegmentString { get; set; }


        [DataMember]
        public string SegmentsId { get; set; }

        [DataMember]
        public string SegmentsMapId { get; set; }
    }


}
