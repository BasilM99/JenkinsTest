using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
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



    [DataContract]
    public class VideoEndCardTrackerDto
    {
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public  int ID { get; set; }
        [DataMember]
        public  bool IsDeleted { get; set; }

        [DataMember]
        public int CardId { get; set; }
    }


    [DataContract]
    public class VideoEndCardDto
    {
        [DataMember]

        public int DocumentId { get; set; }

        [DataMember]

        public bool AutoCloseOption { get; set; }


        [DataMember]

        public double AutoCloseDuration { get; set; }



        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string ClickURL { get; set; }



        [DataMember]
        public VideoEndCardType Type { get; set; }


        [DataMember]

        public int ID { get; set; }

        [DataMember]
  
        public int CreativeUnitId { get; set; }

        [DataMember]
        public int AdCreativeId { get; set; }

   
    }
}
