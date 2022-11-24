using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
 
[DataContract]

public class ThirdPartyTrackerDto
{
        [DataMember]
        public int ID { get; set; }


        [DataMember]
        public string VendorID { get; set; }
        [DataMember]
        public string ScriptURL { get; set; }




    [DataMember]
    public string ExecutionErrorTrackerURL { get; set; }
    [DataMember]
    public string ParametersURL { get; set; }

        [DataMember]
        public bool IsNotChanged { get; set; }
        




    [DataMember]
        public bool IsDeleted { get; set; }

    }

}
