using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class DealPerformanceDto : BaseDealResultDto
    {
 
        private string _secondsubname = string.Empty;
       [ProtoMember(1)]

        public int Date { get; set; }
       [ProtoMember(2)]

        public int Id { get; set; }
     


       [ProtoMember(3)]
        public int? TimeId { get; set; }
       [ProtoMember(4)]
        public string SecondSubName
        {
            get
            {

                if (!string.IsNullOrEmpty(_secondsubname))
                {
                    return _secondsubname.Trim();

                }
                return string.Empty;

            }
            set
            {

                _secondsubname = value;
            }
        }



       [ProtoMember(5)]
        public string SecondSub { get; set; }





       [ProtoMember(6)]
        public string FinalSecondSubName
        {
            get
            {
                
                if (string.IsNullOrEmpty(SubName))
                {
                    if (!GroupByCampId)
                        return Name;
                    else
                        return Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined");
                }
                if (!string.IsNullOrEmpty(SecondSubName))
                {
                    return SecondSubName.Trim();

                }
                return SubName;

            }
            set
            {

               
            }
        }
    }
}
