using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{


   /* public virtual Document Document { get; set; }
    public virtual string URL { get; set; }

    public virtual bool AutoCloseOption { get; set; }
    public virtual double AutoCloseDuration { get; set; }
    public virtual string ClickURL { get; set; }

    public virtual VideoEndCardType Type { get; set; }
    public virtual CreativeUnit CreativeUnit { get; set; }
    public virtual bool IsDeleted { get; set; }
    public virtual InStreamVideoCreative AdCreative { get; set; }*/



    [ProtoContract]
    public class VideoEndCardTrackerDto
    {
       [ProtoMember(1)]
        public string URL { get; set; }
       [ProtoMember(2)]
        public  int ID { get; set; }
       [ProtoMember(3)]
        public  bool IsDeleted { get; set; }

       [ProtoMember(4)]
        public int CardId { get; set; }
    }


    [ProtoContract]
    public class VideoEndCardDto
    {
       [ProtoMember(1)]

        public int DocumentId { get; set; }

       [ProtoMember(2)]

        public bool AutoCloseOption { get; set; }


       [ProtoMember(3)]

        public double AutoCloseDuration { get; set; }



       [ProtoMember(4)]
        public string URL { get; set; }

       [ProtoMember(5)]
        public string ClickURL { get; set; }



       [ProtoMember(6)]
        public VideoEndCardType Type { get; set; }


       [ProtoMember(7)]

        public int ID { get; set; }

       [ProtoMember(8)]
  
        public int CreativeUnitId { get; set; }

       [ProtoMember(9)]
        public int AdCreativeId { get; set; }

   
    }
}
