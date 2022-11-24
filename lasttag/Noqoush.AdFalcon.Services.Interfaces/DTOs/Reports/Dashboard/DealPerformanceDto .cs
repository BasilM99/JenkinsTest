using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class DealPerformanceDto : BaseDealResultDto
    {
 
        private string _secondsubname = string.Empty;
        [DataMember]

        public int Date { get; set; }
        [DataMember]

        public int Id { get; set; }
     


        [DataMember]
        public int? TimeId { get; set; }
        [DataMember]
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



        [DataMember]
        public string SecondSub { get; set; }





        [DataMember]
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
