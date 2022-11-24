using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteResultDto
    {

       [ProtoMember(1)]
        public IEnumerable<AdvertiserAccountMasterAppSiteDto> Items { get; set; } = new List<AdvertiserAccountMasterAppSiteDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteDto
    {
       [ProtoMember(1)]

        public int Id { get; set; }
       [ProtoMember(2)]

        public int LinkId { get; set; }

       [ProtoMember(3)]

        public MasterAppSiteStatus Status { get; set; }

       [ProtoMember(4)]

        public MasterAppSiteType Type { get; set; }

       [ProtoMember(5)]

        public bool GlobalScope { get; set; }
        


        public string ScopeString { get {
                if (GlobalScope)
                    return "fa fa-globe";
                else  if (LinkId>0)
                    return "";
                else
                    return "";
               
            } set {


            } }



        public string StatusString { get { return this.Status.ToText(); } set { } }


        public string TypeString { get { return this.Type.ToText(); } set { } }
       [ProtoMember(7)]

        public int UserId { get; set; }
       [ProtoMember(8)]

        public int AccountId { get; set; }

       [ProtoMember(9)]
        public string Name { get; set; }
       [ProtoMember(10)]
        public string ContentListItems { get; set; }


        [ProtoMember(11)]
        public string IsAdded { get; set; }
    }



    [ProtoContract]
    public class PixelResultDto
    {

       [ProtoMember(1)]
        public IEnumerable<PixelDto> Items { get; set; } = new List<PixelDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class PixelDto
    {
       [ProtoMember(1)]

        public int Id { get; set; }
       [ProtoMember(2)]

        public int LinkId { get; set; }

       [ProtoMember(3)]

        public PixelStatus Status { get; set; }

      

       [ProtoMember(4)]

        public bool GlobalScope { get; set; }

       [ProtoMember(5)]

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

       [ProtoMember(6)]

        public string StatusString { get { return this.Status.ToText(); } set { } }
       
       [ProtoMember(7)]

        public int UserId { get; set; }
       [ProtoMember(8)]

        public int AccountId { get; set; }

       [ProtoMember(9)]
        public string Name { get; set; }
       [ProtoMember(10)]
        public string ContentListItems { get; set; }

       [ProtoMember(11)]
        public int Code { get; set; }

       [ProtoMember(12)]
        public string SegmentString { get; set; }


       [ProtoMember(13)]
        public string SegmentsId { get; set; }

       [ProtoMember(14)]
        public string SegmentsMapId { get; set; }
    }


}
